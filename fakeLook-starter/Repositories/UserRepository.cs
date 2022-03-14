using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User getByUser(User user)
        {
            user.Password = user.Password.GetHashCode().ToString();
            return _context.Users.Where(item => item.Password == user.Password && item.UserName == user.UserName).SingleOrDefault();
        }

        public User GetById(Guid id)
        {
             return (_context.Users.SingleOrDefault(u => u.Id == id));
        }

        public async Task<User> Add(User item)
        {
            if (UserExists(item.Id))
            {
                return item;//TODO
            }
            else
            {
            item.Id = Guid.NewGuid();
            item.Password = item.Password.GetHashCode().ToString();
            var res = _context.Users.Add(item);
            await _context.SaveChangesAsync();
            return res.Entity;
           }          
              
        }

        private bool UserExists(Guid id)
        {
            return GetById(id) != null;
        }

        public async Task<User> Edit(User item)
        {
            var temp = _context.Users.SingleOrDefault(u => u.Id == item.Id);
            if (temp != null)
            {
                _context.Users.Remove(temp);
            }
            _context.Entry<User>(item).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
            return item;
        if (!UserExists(item.Id))
        {
                //TODO
        }
          
        }


        public async Task<User> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                //TODO
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;

        }

        public ICollection<User> GetByPredicate(Func<User, bool> predicate)
        {
            return _context.Users.Where(predicate).ToList();
        }


        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

       
    }
}
