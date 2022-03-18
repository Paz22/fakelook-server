using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fakeLook_starter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        private ICommentRepository _repo;

        public CommentsController(ICommentRepository commentRepository)
        {
            _repo = commentRepository;
        }
        // GET: api/<CommentsController>
        [HttpGet]
        public ICollection<Comment> GetAll()
        {
            return _repo.GetAll();
        }

        // GET api/<CommentsController>/5
        [HttpGet("{id}")]
        public Comment GetById(int id)
        {
            return _repo.GetById(id);
        }

        // POST api/<CommentsController>
        [HttpPost]
        public void Post(Comment comment)
        {
            _repo.Add(comment);
        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
