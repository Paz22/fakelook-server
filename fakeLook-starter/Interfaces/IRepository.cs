using fakeLook_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Interfaces
{ 
    public interface IRepository<T>
    {
        public Task<T> Add(T item);
        public ICollection<T> GetAll();
        public Task<T> Edit(T item);
        public Task<T> Delete(int id);

        public T GetById(int id);
        public ICollection<T> GetByPredicate(Func<T, bool> predicate);
    }
    public interface IUserRepository : IRepository<User>
    {
        public User getByUser(User user);

        public bool UserExists(string userName);
        public bool userNameTaken(string userName, int id);

        //public User addBlocked(int blockerId, int blockedId);

        //public ICollection<int> getAllBlockedByUser(int blockerId);


        //public ICollection<User> getAllFriends(int blockerId);


    }
    public interface IPostRepository : IRepository<Post>
    {
        public string getUsernameById(int id);
    }
}
