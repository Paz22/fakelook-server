using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.auth_example.Filters;
using fakeLook_starter.Interfaces;
using fakeLook_starter.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        //[TypeFilter(typeof(GetUserActionFilter))]
        public IEnumerable<Post> GetAll()
        {
            return _repo.GetAll();
        }

        // GET api/<PostsController>/5
        [HttpGet()]
        [Route("GetById")]
        [TypeFilter(typeof(GetUserActionFilter))]
        public Post GetById(int id)
        {
            return _repo.GetById(id);
        }

        // POST api/<PostsController>
        [HttpPost]
        [Authorize]
        [TypeFilter(typeof(GetUserActionFilter))]
        public void Post(Post post)
        {
        
            _repo.Add(post);
        }

        // PUT api/<PostsController>/5
        [HttpPut("Post")]
        [TypeFilter(typeof(GetUserActionFilter))]
        public void Put(Post post)
        {
            _repo.Edit(post);
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        [TypeFilter(typeof(GetUserActionFilter))]
        public Task<Post> Delete(int id)
        {
            return _repo.Delete(id);
        }

        [HttpPost]
        [Route("/Filter")]
        public async Task<Post> Filter(Filter filter)
        {
            var res = _repo.GetByPredicate(post =>
            {

                bool date = checkDate(post.Date, filter.startingDate, filter.endingDate);
                bool publishers = checkPublishers(post.UserId, filter.Publishers);
                bool taggs = checkTaggs(post.Tags, filter.tags);
                bool taggedUsers = checkTagged(post.UserTaggedPost, filter.taggedUsers);
                return date && publishers && taggedUsers && taggedUsers;
            });
            return null;
        }
        private bool checkDate(DateTime postDate, DateTime startingDate, DateTime endingDate)
        {
            bool date;
            if (startingDate == null && endingDate == null)
            {
                date = true;
            }
            else if (startingDate == null && endingDate != null)
            {
                date = (postDate < endingDate);
            }
            else if (startingDate != null && endingDate == null)
            {
                date = (postDate < startingDate);
            }
            else
            {
                date = (postDate < startingDate && postDate < endingDate);
            }
            return date;
        }

        private bool checkPublishers(int userId, ICollection<string> publishers)
        {
            if (publishers.Count == 0)
            {
                return true;
            }
            var userName = _repo.getUsernameById(userId);
            return publishers.Contains(userName);
        }
        private bool checkTaggs(ICollection<Tag> postTags, ICollection<string> taggs)
        {
            if (taggs.Count == 0)
            {
                return true;
            }
            foreach (var tag in taggs)
            {
                foreach (var postTag in postTags)
                {
                    if (tag.Equals(postTag.Content))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool checkTagged(ICollection<UserTaggedPost> taggedPost, ICollection<string> taggedFilter)
        {
            if (taggedFilter.Count == 0)
            {
                return true;
            }
            foreach (var postTag in taggedPost)
            {
                if (taggedFilter.Contains(postTag.User.UserName))
                {
                    return true;
                }
            }
            return false;
        }




    }
}