using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.auth_example.Filters;
using fakeLook_starter.Interfaces;
using fakeLook_starter.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fakeLook_starter.Controllers
{
    [Route("api/UserAPI")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserRepository _repo;
        private ITokenService _tokenService { get; }

        public UserController(IUserRepository userRepo, ITokenService tokenService)
        {
            _repo = userRepo;
            _tokenService = tokenService;
        }

        //GET: api/<User>
        [HttpGet()]
        [Route("GetAllUsers")]
        public IEnumerable<User> GetAllUsers()
        {
            return _repo.GetAll();
        }

        // GET api/<User>/5
        [HttpGet]
        [Route("GetById")]
        public User GetById(int id)
        {
            return _repo.GetById(id);
        }

    

        // POST api/<User>
        [HttpPost]
        [Route("Post")]
        public void Post(User user)
        {
            _repo.Add(user);
        }

        // POST api/<User>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(User user)
        {
            var dbUser = await _repo.Add(user);
            if (dbUser == null)
            {
                return Problem("Couldn't Add User");
            }
            var token = _tokenService.CreateToken(dbUser);
            return Ok(new { token, dbUser.Id, dbUser.UserName, dbUser.ProfilePic });
        }

        // PUT api/<User>/5
        [HttpPut]
        [Route("Put")]
        public Task<User> Put(User user)
        {
            return _repo.Edit(user);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(User user)
        {
            var dbUser = _repo.getByUser(user);
            if (dbUser == null)
            {
                return Problem("User can not be found");
            }
            var token = _tokenService.CreateToken(dbUser);
            return Ok(new{ token,dbUser.Id,dbUser.UserName,dbUser.ProfilePic});
        }

        //[HttpPost]
        //[Route("addBlocked")]
        //public User addBlocked(int blockerId,int blockedId)
        //{
        //    return _repo.addBlocked(blockerId, blockedId);
        //}

        //[HttpGet]
        //[Route("getBlocked")]
        //public ICollection<int> getAllBlockedByUser(int blockerId)
        //{
        //    return _repo.getAllBlockedByUser(blockerId);
        //}

        //[HttpGet]
        //[Route("getAllFriends")]
        //public ICollection<User> getAllFriends(int blockerId)
        //{
        //    return _repo.getAllFriends(blockerId);
        //}

        // DELETE api/<User>/5
        [HttpDelete]
        [Route("Delete")]
        public void DeleteUser(int id)
        {
            _repo.Delete(id);
        }

        [HttpGet]
        [Route("GetUserByToken")]
        [TypeFilter(typeof(GetUserActionFilter))]
        public User getUserByToken(string token)
        {
            var user = _repo.GetById(int.Parse(_tokenService.GetPayload(token)));
            return user;
        }

        [HttpPost]
        [Route("checkUserNameValidity")]

        public bool isUserNameExist(string userName,int id)
        {
            return _repo.userNameTaken(userName,id);
        }

        //[HttpPost]
        //[Route("getFriendsNotBlocked")]
        //public ICollection<User> getFriendsNotBlocked(int userId)
        //{
        //    ICollection<User> notBlocked=new List<User>();
        //    IList<User> allUsers= (IList<User>)GetAllUsers();
        //    var allBlocked=GetById(userId).blockedUser;
        //    for (int i=0;i<allUsers.Count;i++)
        //    {
        //        for (int j = 0; j < allBlocked.Count; j++)
        //        {
        //            if(allUsers.ElementAt(i).Id!=allBlocked.ElementAt(j).BlockedUserId)
        //            {
        //                notBlocked.Add(allUsers.ElementAt(i));
        //            }
        //        }
        //    }
        //    return notBlocked;
        //}


    }
}