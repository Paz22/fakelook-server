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
    public class CircleController : ControllerBase
    {
        private IRepository<Circle> _repo;

        public CircleController(IRepository<Circle> circleRepo)
        {
            _repo = circleRepo;
        }

        // GET: api/<CircleController>
        [HttpGet]
        public IEnumerable<Circle> GetAll()
        {
            return _repo.GetAll();
        }

        // GET api/<CircleController>/5
        [HttpGet("Post")]
        public Circle GetById(int id)
        {
            return _repo.GetById(id);
        }

        // POST api/<CircleController>
        [HttpPost]
        public Task<Circle> Add(Circle circle)
        {
            return _repo.Add(circle);
        }

        // PUT api/<CircleController>/5
        [HttpPut("{id}")]
        public Task<Circle> Put(Circle circle)
        {
            return _repo.Edit(circle);
        }

        // DELETE api/<CircleController>/5
        [HttpDelete("{id}")]
        public Task<Circle> Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}