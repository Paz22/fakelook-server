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
    public class UserTaggedPostController : ControllerBase
    {

        private IUneditableRepository<UserTaggedPost> _repo;

        public UserTaggedPostController(IUneditableRepository<UserTaggedPost> userTaggedPostRepository)
        {
            _repo = userTaggedPostRepository;
        }
        // GET: api/<UserTaggedPostController>
        [HttpGet]
        public ICollection<UserTaggedPost> GetAll()
        {
            return _repo.GetAll();
        }

        // GET api/<UserTaggedPostController>/5
        [HttpGet("{id}")]
        public UserTaggedPost GetById(int id)
        {
            return _repo.GetById(id);
        }

        [HttpPost]
        public Task<UserTaggedPost> Post(UserTaggedPost tag)
        {
            return _repo.Add(tag);
        }


               // DELETE api/<UserTaggedPostController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           _repo.Delete(id);
        }
    }
}
