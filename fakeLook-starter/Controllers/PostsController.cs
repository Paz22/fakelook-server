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
    public class PostsController : ControllerBase
    {
        private IPostRepository _repo;

        public PostsController(DataContext context)
        {
            _repo = new PostRepository(context);
        }
        // GET: api/<PostsController>
        [HttpGet]
        public IEnumerable<Post> GetAll()
        {
            return _repo.GetAll();
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public Post GetById(Guid id)
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
        [HttpPut("Post")]
        public void Put( Post post)
        {
            _repo.Edit(post);
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _repo.Delete(id);
        }
    }
}
