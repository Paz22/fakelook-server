using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class CircleRepository : IRepository<Circle>
    {
        readonly private DataContext _context;

        public CircleRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Circle> GetAll()
        {
            return _context.Circles.ToList();
        }

        public Circle GetById(int id)
        {
            return (_context.Circles.SingleOrDefault(u => u.Id == id));
        }

        public async Task<Circle> Add(Circle item)
        {
            if (CircleExists(item))
            {
                return item;//TODO
            }
            else
            {
                var res = _context.Circles.Add(item);
                await _context.SaveChangesAsync();
                return res.Entity;
            }
        }

        private bool CircleExists(Circle circle)
        {
            var res = _context.Circles.Where(item => item.Name == circle.Name).SingleOrDefault();
            return res != null;
        }

        public async Task<Circle> Edit(Circle item)
        {
            var temp = _context.Circles.FirstOrDefault(u => u.Id == item.Id);
            if (temp == null)
            {
                return null;//TODO
            }
            _context.Entry<Circle>(temp).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
            return item;
        }


        public async Task<Circle> Delete(int id)
        {
            var circle = _context.Circles.SingleOrDefault(p => p.Id == id);
            if (circle == null)
            {
                return null;//TODO
            }
            _context.Circles.Remove(circle);
            await _context.SaveChangesAsync();
            return circle;

        }


        public bool DeleteMember(int id, User user)
        {
            var circle = _context.Circles.SingleOrDefault(p => p.Id == id);
            if (circle == null)
            {
                return false;
            }
            var temp = circle.Members.FirstOrDefault(u => u.Id == user.Id);
            if (temp == null)
            {
                return false;
            }
            circle.Members.Remove(temp);
            return true;
        }

        public bool addMember(int id, User user)
        {
            var circle = _context.Circles.SingleOrDefault(p => p.Id == id);
            if (circle == null)
            {
                return false;
            }
            circle.Members.Remove(user);
            return true;
        }

        public ICollection<Circle> GetByPredicate(Func<Circle, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}