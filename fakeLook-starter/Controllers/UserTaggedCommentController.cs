using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fakeLook_starter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaggedCommentController : ControllerBase
    {

        private IUserTaggedCommentRepository _repo;

        public UserTaggedCommentController(IUserTaggedCommentRepository userTaggedCommentRepository)
        {
            _repo = userTaggedCommentRepository;
        }
        // GET: api/<UserTaggedCommentController>
        [HttpGet]
        public ICollection<UserTaggedComment> GetAll()
        {
            return _repo.GetAll();
        }

        // GET api/<UserTaggedCommentController>/5
        [HttpGet("{id}")]
        public UserTaggedComment GetById(int id)
        {
            return _repo.GetById(id);
        }

        // POST api/<UserTaggedCommentController>
        [HttpPost]
        public Task<UserTaggedComment> Post(UserTaggedComment tag)
        {
            return _repo.Add(tag);
        }

       
        // DELETE api/<UserTaggedCommentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
