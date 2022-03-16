namespace fakeLook_starter
{
    using fakeLook_models.Models;
    using fakeLook_starter.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    namespace auth_example.Filters
    {
        public class GetUserActionFilter : ActionFilterAttribute
        {
            private readonly ITokenService _tokenService;
            private readonly IUserRepository _userRepository;

            public GetUserActionFilter(ITokenService tokenService, IUserRepository userRepository)
            {
                _tokenService = tokenService;
                _userRepository = userRepository;
            }


            public override void OnActionExecuting(ActionExecutingContext context)
            {
                try
                {
                    var token = context.HttpContext.Request.Headers.Where(header => header.Key == "Authorization").SingleOrDefault().Value.ToString().Split(" ")[1];
                    var user = _userRepository.GetById(int.Parse(_tokenService.GetPayload(token)));
                    context.HttpContext.Request.RouteValues.Add("user", user);
                }
                catch (Exception ex)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.HttpContext.Response.CompleteAsync();
                }

            }
        }
    }
}
