using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using fakeLook_starter.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fakeLook_starter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserRepository _repo;
        private ITokenService _tokenService { get; }

        public UserController(DataContext context, ITokenService tokenService)
        {
            _repo = new UserRepository(context);
            _tokenService = tokenService;
        }

        // GET: api/<User>
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return _repo.GetAll();
        }

        // GET api/<User>/5
        [HttpGet("{id}")]
        public User GetById(Guid id)
        {
            return _repo.GetById(id);

        }

        // POST api/<User>
        [HttpPost]
        public void Post(User user)
        {
            _repo.Add(user);
        }

        // POST api/<User>
        [HttpPost]
        [Route("/Register")]
        public  IActionResult Register(User user)
        {
            _repo.Add(user);
            return Login(user);
        }

        // PUT api/<User>/5
        [HttpPut]
        public void Put(User user)
        {
            _repo.Edit(user);
        }

        [HttpPost]
        [Route("/Login")]
        public IActionResult Login(User user)
        {
            var dbUser = _repo.getByUser(user);
            if (dbUser == null)
            {
                return Problem("User can not be found");
            }
            var token = _tokenService.CreateToken(dbUser);
            return Ok(new { token });


        }

        // DELETE api/<User>/5
        [HttpDelete("{id}")]
        public void DeleteUser(Guid id)
        {
            _repo.Delete(id);
        }
    }
}