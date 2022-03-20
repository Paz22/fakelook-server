using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fakeLook_starter.Controllers
{
    [Route("api/LikesAPI")]
    [ApiController]
    public class LikesController : ControllerBase
    {

        private ILikeRepository _repo;

        public LikesController(ILikeRepository likeRepository)
        {
            _repo = likeRepository;
        }

        // GET: api/<LikesController>
        [HttpGet]
        public IEnumerable<Like> GetAll()
        {
            return _repo.GetAll();
        }

        // GET api/<LikesController>/5
        [HttpGet("{id}")]
        public Like GetById(int id)
        {
            return _repo.GetById(id);
        }

        // POST api/<LikesController>
        [HttpPost]
        public void Post(Like like)
        {
            _repo.Add(like);
        }

        // DELETE api/<LikesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
