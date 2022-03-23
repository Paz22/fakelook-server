using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class UserTaggedCommentRepository : IUserTaggedCommentRepository
    {

        readonly private DataContext _context;
        private IDtoConverter _converter;
        public UserTaggedCommentRepository(DataContext context, IDtoConverter dtoConverter)
        {
            _context = context;
            _converter = dtoConverter;
        }
        public async Task<UserTaggedComment> Add(UserTaggedComment item)
        {
            if (!tagExist(item))
            {
                var res = _context.UserTaggedComments.Add(item);
                _context.SaveChanges();
                return res.Entity;
            }
            return null;
        }

        private bool tagExist(UserTaggedComment item)
        {
            var exist = _context.UserTaggedComments.SingleOrDefault(p => p.UserId == item.UserId && p.CommentId == item.CommentId);
            return exist != null;
        }

        public async Task<UserTaggedComment> Delete(int id)
        {
            var tag = GetById(id);
            if (tag == null)
            {
                return null;
            }
            var removed = _context.UserTaggedComments.Remove(tag);
            await _context.SaveChangesAsync();
            return removed.Entity;
        }

        public ICollection<UserTaggedComment> GetAll()
        {
            return _context.UserTaggedComments.ToList();
        }

        public UserTaggedComment GetById(int id)
        {
            return _context.UserTaggedComments.Where(_context => _context.UserId == id).FirstOrDefault();   
        }
    }
}
