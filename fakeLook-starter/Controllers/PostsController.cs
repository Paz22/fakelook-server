using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.auth_example.Filters;
using fakeLook_starter.Interfaces;
using fakeLook_starter.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fakeLook_starter.Controllers
{
    [Route("api/PostsAPI")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostRepository _repo;

        public PostsController(IPostRepository postRepo)
        {
            _repo = postRepo;
        }


        //GET: api/<PostsController>
        [HttpGet]
        [Route("GetAll")]
        [TypeFilter(typeof(GetUserActionFilter))]

        public IEnumerable<Post> GetAll()
        {
            return _repo.GetAll();
        }



        // GET api/<PostsController>/5
        [HttpGet()]
        [Route("GetById")]
        public Post GetById(int id)
        {
            return _repo.GetById(id);
        }

        // POST api/<PostsController>
        [HttpPost]
        public void Post( Post post)
        {
            _repo.Add(post);
        }

        // PUT api/<PostsController>/5
        [HttpPut]
        public void Put( Post post)
        {
            _repo.Edit(post);

        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
