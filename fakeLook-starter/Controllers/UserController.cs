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

        public UserController(DataContext context)
        {
            _repo = new UserRepository(context);
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
        public void Register(User user)
        {
            _repo.Add(user);
        }

        // PUT api/<User>/5
        [HttpPut]
        public void Put(User user)
        {
            _repo.Edit(user);
        }

        [HttpPost]
        [Route("/Login")]
        public User Login(User user)
        {
            return _repo.getByUser(user);
        }

        // DELETE api/<User>/5
        [HttpDelete("{id}")]
        public void DeleteUser(Guid id)
        {
            _repo.Delete(id);
        }
    }
}