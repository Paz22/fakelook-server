using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.auth_example.Filters;
using fakeLook_starter.Interfaces;
using fakeLook_starter.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [Route("GetAllPosts")]
        //[TypeFilter(typeof(GetUserActionFilter))]
        public IEnumerable<Post> GetAll()
        {
            return _repo.GetAll();
        }

        // GET api/<PostsController>/5
        [HttpGet]
        [Route("GetPostById")]
        //[TypeFilter(typeof(GetUserActionFilter))]
        public Post GetById(int id)
        {
            return _repo.GetById(id);
        }

        // POST api/<PostsController>
        [HttpPost]
        [Route("AddPost")]
        //[TypeFilter(typeof(GetUserActionFilter))]
        public void Post(Post post)
        {
            _repo.Add(post);
        }

        // PUT api/<PostsController>/5
        [HttpPut]
        [Route("EditPost")]
        [TypeFilter(typeof(GetUserActionFilter))]
        public void Put(Post post)
        {
            _repo.Edit(post);
        }

        // DELETE api/<PostsController>/5
        [HttpDelete]
        [Route("DeletePost")]
        [TypeFilter(typeof(GetUserActionFilter))]
        public Task<Post> Delete(int id)
        {
            return _repo.Delete(id);
        }

  
        [HttpPost]
        [Route("Filter")]
        public ICollection<Post> Filter(Filter filter)
        {
            var res = _repo.GetByPredicate(post =>
            {
                bool tags = checkTags(post.Tags, filter.tags);
                bool taggedUsers = checkTagged(post.UserTaggedPost, filter.taggedUsers);
                bool publishers = checkPublishers(post.UserId, filter.Publishers);
                bool date = checkDate(post.Date, filter.startingDate, filter.endingDate);
                return date && publishers && tags && taggedUsers;
            });
            return res;
        }
        private bool checkDate(DateTime postDate, Nullable<DateTime> startingDate, Nullable<DateTime> endingDate)
        {
            bool date;
            if (!startingDate.HasValue && !endingDate.HasValue)
            {
                date = true;
            }
            else if (!startingDate.HasValue && endingDate.HasValue)
            {
                date = (postDate <= endingDate);
            }
            else if (startingDate.HasValue && !endingDate.HasValue)
            {
                date = (postDate >= startingDate);
            }
            else
            {
                date = (postDate >= startingDate && postDate <= endingDate);
            }
            return date;
        }

        private bool checkPublishers(int userId, ICollection<string> publishers)
        {
            if (publishers == null)
            {
                return true;
            }
            var userName = _repo.getUsernameById(userId);
            return publishers.Contains(userName);
        }
        private bool checkTags(ICollection<Tag> postTags, ICollection<string> tags)
        {
            if (tags==null)
            {
                return true;
            }
            if(postTags == null)
            {
                return false;
            }
            foreach (var tag in tags)
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
            if (taggedFilter == null || taggedFilter.Count == 0)
            {
                return true;
            }
            if (taggedFilter.Count > 0 && taggedPost == null)
            {
                return false;
            }

            foreach (var postTag in taggedPost)
            {
                var userName = _repo.getUsernameById(postTag.UserId);
                if (taggedFilter.Contains(userName))
                {
                    return true;
                }
            }
            return false;
        }




    }
}