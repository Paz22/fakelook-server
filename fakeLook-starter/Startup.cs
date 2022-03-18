
using fakeLook_starter.Services;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using fakeLook_starter.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using fakeLook_dal.Data;
using Microsoft.EntityFrameworkCore;

namespace fakeLook_starter
{
    public class Startup
    {
        private readonly string _MyAllowSpecificOrigin = "myAlloworigin";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
  
            #region Configure jwt Auth
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = Configuration["Jwt:Issuer"],
                      ValidAudience = Configuration["Jwt:Issuer"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                  };
              });
            #endregion

            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<IDtoConverter, DtoConverter>();

            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            //services.AddScoped<IPostRepository, PostRepository>();

            services.AddScoped<IDtoConverter, DtoConverter>();
            services.AddScoped<ILikeRepository, LikesRepository>();
            services.AddScoped<ICommentRepository, CommentsRepository>();
            services.AddScoped<IUserTaggedCommentRepository, UserTaggedCommentRepository>();
            services.AddScoped<IUserTaggedPostRepository, UserTaggedPostRepository>();
            services.AddScoped<ITagsRepository, TagsRepository>();

            //services.AddTransient<ITokenService, TokenService>();
            //services.AddTransient<IUserRepository, UserRepository>();
            //services.AddTransient<IPostRepository, PostRepository>();
            //services.AddTransient<IPostRepository, PostRepository>();

            //services.AddTransient<ILikeRepository, LikesRepository>();
            //services.AddTransient<ICommentRepository, CommentsRepository>();
            //services.AddTransient<IUserTaggedCommentRepository, UserTaggedCommentRepository>();
            //services.AddTransient<IUserTaggedPostRepository, UserTaggedPostRepository>();
            //services.AddTransient<ITagsRepository, TagsRepository>();



            #region Setting DB configuration
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            services.AddHttpContextAccessor();
            #endregion            

            #region Setting cors policy
            services.AddCors(options =>
            {
                options.AddPolicy(name: _MyAllowSpecificOrigin, builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "fakelook_starter", Version = "v1" });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,DataContext data)
        {
        //    data.Database.EnsureDeleted();
            data.Database.EnsureCreated();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "fakelook_starter v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(_MyAllowSpecificOrigin);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}