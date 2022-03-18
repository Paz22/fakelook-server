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
    public class TagsController : ControllerBase
    {

        private IUneditableRepository<Tag> _repo;

        public TagsController(IUneditableRepository<Tag> tagsRepository)
        {
            _repo = tagsRepository;
        }
        // GET: api/<TagsController>
        [HttpGet]
        public ICollection<Tag> GetAll()
        {
            return _repo.GetAll();
        }

        // GET api/<TagsController>/5
        [HttpGet("{id}")]
        public Tag GetById(int id)
        {
            return _repo.GetById(id);
        }

        // POST api/<TagsController>
        [HttpPost]
        public Task<Tag> Add(Tag tag)
        {
            return _repo.Add(tag);
        }

       

        // DELETE api/<TagsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
