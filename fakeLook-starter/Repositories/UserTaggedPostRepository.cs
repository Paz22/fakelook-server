using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class UserTaggedPostRepository: IUserTaggedPostRepository
    {


        readonly private DataContext _context;
        private IDtoConverter _converter;
        public UserTaggedPostRepository(DataContext context, IDtoConverter dtoConverter)
        {
            _context = context;
            _converter = dtoConverter;
        }
        public async Task<UserTaggedPost> Add(UserTaggedPost item)
        {
            if (!TagExist(item))
            {
                var res = _context.UserTaggedPosts.Add(item);
                await _context.SaveChangesAsync();
                return res.Entity;
            }
            return null;

        }

        private bool TagExist(UserTaggedPost item)
        {
            var exist = _context.UserTaggedPosts.Where(p => p.UserId == item.UserId && p.PostId == item.PostId);
            return exist != null;
        }

        public async Task<UserTaggedPost> Delete(int id)
        {
            var tag = GetById(id);
            if (tag == null)
            {
                return null;
            }
            var removed = _context.UserTaggedPosts.Remove(tag);
            await _context.SaveChangesAsync();
            return removed.Entity;
        }

        public ICollection<UserTaggedPost> GetAll()
        {
            return _context.UserTaggedPosts.ToList();
        }

        public UserTaggedPost GetById(int id)
        {
            return _context.UserTaggedPosts.Where(p=>p.Id == id).SingleOrDefault();
        }


    }
}

