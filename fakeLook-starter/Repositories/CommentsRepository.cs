using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class CommentsRepository : IUneditableRepository<Comment>
    {

        readonly private DataContext _context;
        private readonly IUneditableRepository<UserTaggedComment> _userTaggedRepo;
        private IDtoConverter _converter;
        public CommentsRepository(DataContext context, IDtoConverter dtoConverter, IUneditableRepository<UserTaggedComment> userTaggedRepo)
        {
            _context = context;
            _converter = dtoConverter;
            _userTaggedRepo = userTaggedRepo;
        }

        public async Task<Comment> Add(Comment item)
        {
            foreach(UserTaggedComment userTagged in item.UserTaggedComment)
            {
                await _userTaggedRepo.Add(userTagged);
            }
            var addUserTagged = _context.Add(item);
            await _context.SaveChangesAsync();
            return addUserTagged.Entity;
        }

        public async Task<Comment> Delete(int id)
        {
            var commentToDelete=GetById(id);
            foreach (UserTaggedComment userTagged in commentToDelete.UserTaggedComment)
            {
                await _userTaggedRepo.Delete(userTagged.Id);
            }
            var removed = _context.Comments.Remove(commentToDelete);
            await _context.SaveChangesAsync();
            return removed.Entity;
        }

        public ICollection<Comment> GetAll()
        {
            return _context.Comments.ToList();
        }

        public Comment GetById(int id)
        {
            return _context.Comments.Where(c => c.Id == id).FirstOrDefault();
        }

       
    }
}
