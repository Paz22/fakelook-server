using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class CommentsRepository : IRepository<Comment>
    {

        readonly private DataContext _context;
        private readonly IUserTaggedCommentRepository _userTaggedRepo;
        private readonly ITagsRepository _tagsRepo;

        private IDtoConverter _converter;
        public CommentsRepository(DataContext context, IDtoConverter dtoConverter, IUserTaggedCommentRepository userTaggedRepo,ITagsRepository tagsRepo)
        {
            _context = context;
            _converter = dtoConverter;
            _userTaggedRepo = userTaggedRepo;
            _tagsRepo = tagsRepo;
        }

        public async Task<Comment> Add(Comment item)
        {
            ICollection<Tag> varTags = new List<Tag>();
            for (var i = 0; i < item.Tags.Count; i++)
            {
                varTags.Add(await _tagsRepo.Add(item.Tags.ElementAt(i)));
            }
            item.Tags = varTags;
            var addComment = _context.Add(item);
            _context.SaveChanges();
            return addComment.Entity;
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

        public Task<Comment> Edit(Comment item)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Comment> GetAll()
        {
            return _context.Comments.ToList();
        }

        public Comment GetById(int id)
        {
            return _context.Comments.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Comment> GetByPredicate(System.Func<Comment, bool> predicate)
        {
            throw new System.NotImplementedException();
        }
    }
}
