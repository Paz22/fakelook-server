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
            return _context.Users.Where(item => item.Password == user.Password && item.UserName == user.UserName).SingleOrDefault();
        }

        public User GetById(int id)
        {
            return (_context.Users.SingleOrDefault(u => u.Id == id));
        }

        public async Task<User> Add(User item)
        {
            if (UserExists(item.UserName))
            {
                return item;//TODO
            }
            else
            {
                var res = _context.Users.Add(item);
                await _context.SaveChangesAsync();
                return res.Entity;
            }
        }

        public bool UserExists(string userName)
        {
            var res = _context.Users.Where(item => item.UserName == userName).SingleOrDefault();
            return res != null;
        }

       

        public async Task<User> Edit(User item)
        {
            var temp = _context.Users.FirstOrDefault(u => u.Id == item.Id);
            if (temp == null)
            {
                return null;//TODO
            }
            _context.Entry<User>(temp).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
            if (!UserExists(item.UserName))
            {
                //TODO
            }
            return item;
        }



        public async Task<User> Delete(int id)
        {
            var user = _context.Users.SingleOrDefault(p => p.Id == id);
            if (user == null)
            {
                return user;//TODO
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