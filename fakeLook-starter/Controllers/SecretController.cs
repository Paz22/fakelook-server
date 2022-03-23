using fakeLook_models.Models;
using fakeLook_starter.auth_example.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auth_example.Controllers
{
    [Route("api/SceretAPI")]
    [ApiController]
    [Authorize]
    public class SecretController : ControllerBase
    {
      
        [HttpGet]
        [Route("Authenticated")]
        [TypeFilter(typeof(GetUserActionFilter))]
        public IActionResult Authenticated()
        {
            Request.RouteValues.TryGetValue("user", out var obj);
            var user = obj as User;
            return Ok(new { msg = $"only authenticated gets this {user.FirstName} {user.LastName}" });
        }
 
    }
}