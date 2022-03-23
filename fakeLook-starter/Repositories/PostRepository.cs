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
        private readonly IRepository<Comment> _commentRepo;
        private readonly ILikeRepository _likeRepo;
        private readonly ITagsRepository _tagRepo;
        private readonly IUserTaggedPostRepository _userTaggedPostRepo;
        private readonly IUserTaggedCommentRepository _userTaggedCommentRepo;


        private IDtoConverter _converter;
        public PostRepository(DataContext context, IDtoConverter dtoConverter, IUserTaggedPostRepository userTaggedrepo,
          ITagsRepository tagsRepo, ILikeRepository likeRepo, IRepository<Comment> commentsRepo)
        {
            _context = context;
            _converter = dtoConverter;
            _commentRepo = commentsRepo;
            _likeRepo = likeRepo;
            _tagRepo = tagsRepo;
            _userTaggedPostRepo = userTaggedrepo;
        }


        public async Task<Post> Add(Post item)
        {
            ICollection<Tag> varTags = new List<Tag>();
            if(item.Tags!=null)
            for (var i = 0; i < item.Tags.Count; i++)
            {
                varTags.Add(await _tagRepo.Add(item.Tags.ElementAt(i)));
            }
            item.Tags = varTags;
            var res = _context.Posts.Add(item);
            var entries=_context.SaveChanges();
            if(entries==0)
            {
                //TODO
            }
            return res.Entity;                
        }

        public async Task<Post> Delete(int id)
        {
            var post = _context.Posts.SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return null;
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
                foreach (UserTaggedComment userTaggedComment in comment.UserTaggedComment)
                {
                    await _userTaggedCommentRepo.Delete(userTaggedComment.Id);
                }
                await _commentRepo.Delete(comment.Id);
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return _converter.DtoPost(post);
        }

        public string getUsernameById(int id) //for internal use only,hence not writing dto
        {
            return _context.Users.SingleOrDefault(u => u.Id == id).UserName;
        }



        public  async Task<Post> Edit(Post item) //TODO rewrite and split to cases!
        {
            var existingPost = _context.Posts
       .Where(p => p.Id == item.Id)
       .Include(p => p.Tags)
       .Include(p=>p.UserTaggedPost)
       .SingleOrDefault();
            if (existingPost != null)
            {
                // Update parent
                _context.Entry(existingPost).CurrentValues.SetValues(item);

            }
            // Delete children
            if(existingPost.Tags!=null)
            foreach (var existingTags in existingPost.Tags.ToList())
            {
                if(item.Tags!=null)
                if (!item.Tags.Any(c => c.Id == existingTags.Id))
                    _context.Tags.Remove(existingTags);
            }
            if(existingPost.UserTaggedPost!=null)
            foreach (var existingUserTag in existingPost.UserTaggedPost.ToList())
            {
                if (item.Tags != null)
                    if (!item.Tags.Any(c => c.Id == existingUserTag.Id))
                        _context.UserTaggedPosts.Remove(existingUserTag);
            }

            // Update and Insert children
            if (item.Tags!=null)
            foreach (var tag in item.Tags)
            { 
                var existingtag = existingPost.Tags
                    .Where(c => c.Id == tag.Id && c.Id != default(int))
                    .SingleOrDefault();

                if (existingtag != null)
                    // Update child
                    _context.Entry(existingtag).CurrentValues.SetValues(tag);
                else
                {
                    // Insert child
                    var newTag = new Tag
                    {
                        Content = tag.Content
                    };
                    existingPost.Tags.Add(newTag);
                }
            }
            //---------------------------------
            if (item.UserTaggedPost != null)
                foreach (var userTag in item.UserTaggedPost)
                {
                    
                    var existingtag = existingPost.UserTaggedPost
                        .Where(c => c.Id == userTag.Id && c.Id != default(int))
                        .SingleOrDefault();

                    if (existingtag != null)
                        // Update child
                        _context.Entry(existingtag).CurrentValues.SetValues(userTag);
                    else
                    {
                        // Insert child
                        var newUserTag = new UserTaggedPost
                        {
                            UserId = userTag.UserId,
                            PostId = userTag.PostId
                            

                        };
                        existingPost.UserTaggedPost.Add(newUserTag);
                    }
                }

            _context.SaveChanges();
            return existingPost;


        }

        public ICollection<Block> getAllBlockedByUser(int id)
        {
            return _context.Blocks.Where(p => p.BlockerUserId == id).ToList();
        }

        public ICollection<Post> GetAll()
        {
            return _context.Posts
                .Include(p => p.Likes).ThenInclude(p => p.User)
                .Include(p => p.User)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p=>p.Tags)
                .Include(p => p.UserTaggedPost).ThenInclude(t => t.User)
                .Select(dtoLogic).ToList();
        }

        public Post GetById(int id)
        {
            return _context.Posts
                .Include(p => p.Likes).ThenInclude(p => p.User)
                .Include(p => p.User)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Tags)
                .Include(p => p.UserTaggedPost).ThenInclude(t => t.User)
                .SingleOrDefault(p => p.Id == id);
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
            dtoPost.Tags = p.Tags.Select(c =>
            {
                var dtoTag = _converter.DtoTag(c);
                return dtoTag;
            }).ToArray();
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