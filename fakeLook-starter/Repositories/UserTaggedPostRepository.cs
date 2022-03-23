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

        //Adding tag to the DB via the data context
        public async Task<UserTaggedPost> Add(UserTaggedPost item)
        {
            if (!TagExist(item))
            {
                var res = _context.UserTaggedPosts.Add(item);
                 _context.SaveChanges();
                return res.Entity;
            }
            return null;

        }

        //Returning whether or not tag exists in the DB
        private bool TagExist(UserTaggedPost item)
        {
            var exist = _context.UserTaggedPosts.Where(p => p.UserId == item.UserId && p.PostId == item.PostId).SingleOrDefault();
            return exist != null;
        }


        //Deleting existing tag from the DB
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


        //Returning all the tags from the DB
        public ICollection<UserTaggedPost> GetAll()
        {
            return _context.UserTaggedPosts.ToList();
        }


        //Given it's id,returning tag
        public UserTaggedPost GetById(int id)
        {
            return _context.UserTaggedPosts.Where(p=>p.Id == id).SingleOrDefault();
        }


    }
}

