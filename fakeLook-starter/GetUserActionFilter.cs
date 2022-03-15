namespace fakeLook_starter
{
    using fakeLook_models.Models;
    using fakeLook_starter.Interfaces;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace auth_example.Filters
    {
        public class GetUserActionFilter : ActionFilterAttribute
        {
            private readonly ITokenService _tokenService;
            private readonly IRepository<User> _userRepository;

            public GetUserActionFilter(ITokenService tokenService, IRepository<User> userRepository)
            {
                _tokenService = tokenService;
                _userRepository = userRepository;
            }


            public override void OnActionExecuting(ActionExecutingContext context)
            {
                var token = context.HttpContext.Request.Headers.Where(header => header.Key == "Authorization").SingleOrDefault().Value.ToString().Split(" ")[1];
                var user = _userRepository.GetById(int.Parse(_tokenService.GetPayload(token)));
                context.HttpContext.Request.RouteValues.Add("user", user);
            }
        }
    }
}
