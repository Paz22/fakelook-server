using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class PostRepository : IPostRepository
    {
        readonly private DataContext _context;
        private readonly IUneditableRepository<Comment> _commentRepo;
        private readonly IUneditableRepository<Like> _likeRepo;
        private readonly IUneditableRepository<Tag> _tagRepo;
        private readonly IUneditableRepository<UserTaggedPost> _userTaggedPostRepo;

        private IDtoConverter _converter;
        public PostRepository(DataContext context, IDtoConverter dtoConverter, IUneditableRepository<UserTaggedPost> userTaggedrepo,
            IUneditableRepository<Tag> tagsRepo, IUneditableRepository<Like> likeRepo, IUneditableRepository<Comment> commentsRepo)
        {
            _context = context;
            _converter = dtoConverter;
            _commentRepo = commentsRepo;
            _likeRepo=likeRepo;
            _tagRepo=tagsRepo;
            _userTaggedPostRepo = userTaggedrepo;
        }

        public async Task<Post> Add(Post item)
        {
            foreach(Tag tag in item.Tags)
            {
                await _tagRepo.Add(tag);
            } 
            foreach(UserTaggedPost userTagged in item.UserTaggedPost)
            {
                await _userTaggedPostRepo.Add(userTagged);
            }
            var res = _context.Posts.Add(item);
            await _context.SaveChangesAsync();
            return _converter.DtoPost(res.Entity);
        }

        public async Task<Post> Delete(int id)
        {
            var post = _context.Posts.SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return null;
            }
            foreach (Tag tag in post.Tags)
            {
                 await _tagRepo.Delete(tag.Id);
            }
            foreach (UserTaggedPost userTagged in post.UserTaggedPost)
            {
                 await _userTaggedPostRepo.Delete(userTagged.Id);
            }
            foreach (Like like in post.Likes)
            {
                await _likeRepo.Delete(like.Id);
            }
            foreach (Comment comment in post.Comments)
            {
                await _userTaggedPostRepo.Delete(comment.Id);
            }
            foreach (Comment comment in post.Comments)
            {
                await _userTaggedPostRepo.Delete(comment.Id);
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return _converter.DtoPost(post);
        }

        public string getUsernameById(int id) //for internal use only,hence not writing dto
        {
            return _context.Users.SingleOrDefault(u => u.Id == id).UserName;
        }

        public async Task<Post> Edit(Post item) //TODO rewrite and split to cases!
        {
            item.IsEdited = true;            
            var temp = _context.Posts.FirstOrDefault(u => u.Id == item.Id);
            if (temp == null)
            {
                return null;//TODO
            }
            _context.Entry<Post>(temp).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
            return _converter.DtoPost(item);
        }

        public ICollection<Block> getAllBlockedByUser(int id)
        {
            return _context.Blocks.Where(p => p.BlockerUserId == id).ToList();
        }

        public ICollection<Post> GetAll()
        {
            return _context.Posts
                .Include(p=>p.Likes).ThenInclude(p=>p.User)
                .Include(p => p.User)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p=>p.UserTaggedPost).ThenInclude(t=>t.User)
                .Select( dtoLogic).ToList();
        }

        public Post GetById(int id)
        {
            return _context.Posts
                .Include(p => p.Likes).ThenInclude(p => p.User)
                .Include(p => p.User)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Tags)
                .Include(p => p.UserTaggedPost).ThenInclude(t => t.User)
                .Select(p => dtoLogic(p)).SingleOrDefault(p => p.Id == id);
        }

        private Post dtoLogic(Post p)
        {
            var dtoPost = _converter.DtoPost(p);
            dtoPost.User = _converter.DtoUser(p.User);
            dtoPost.Comments = p.Comments.Select(c =>
            {
                var dtoComment = _converter.DtoComment(c);
                dtoComment.User = _converter.DtoUser(c.User);
                return dtoComment;
            }).ToArray();
            dtoPost.Likes = p.Likes.Select(l =>
            {
                var dtoLike = _converter.DtoLike(l);
                dtoLike.User = _converter.DtoUser(l.User);
                return dtoLike;
            }).ToArray();
            //dtoPost.Tags = p.Tags.Select(c =>
            //{
            //    var dtoTag = _converter.DtoTag(c);
            //    return dtoTag;
            //}).ToArray();
            dtoPost.UserTaggedPost = p.UserTaggedPost.Select(c =>
            {
                var dtoUsersTaggedPost = _converter.DtoUserTaggedPost(c);
                dtoUsersTaggedPost.User = _converter.DtoUser(c.User);
                return dtoUsersTaggedPost;
            }).ToArray();
            return dtoPost;
        }

      


        public ICollection<Post> GetByPredicate(Func<Post, bool> predicate)
        {
            return _context.Posts.Where(predicate).ToList();
        }
    }
}