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
        private readonly ICommentRepository _commentRepo;
        private readonly ILikeRepository _likeRepo;
        private readonly ITagsRepository _tagRepo;
        private readonly IUserTaggedPostRepository _userTaggedPostRepo;
        private readonly IUserTaggedCommentRepository _userTaggedCommentRepo;


        private IDtoConverter _converter;
        public PostRepository(DataContext context, IDtoConverter dtoConverter, IUserTaggedPostRepository userTaggedrepo,
          ITagsRepository tagsRepo, ILikeRepository likeRepo, ICommentRepository commentsRepo)
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
                .Select(p => dtoLogic(p))

                .SingleOrDefault(p => p.Id == id);
        }

        private Post dtoLogic(Post post)
        {
            var dtoPost = _converter.DtoPost(post);
            // User
            dtoPost.User = _converter.DtoUser(post.User);
            // User ID
            dtoPost.UserId = post.UserId;
            // Comments
            dtoPost.Comments = post.Comments?.Select(c =>
            {
                var dtoComment = _converter.DtoComment(c);
                // User of the comment
                dtoComment.User = _converter.DtoUser(c.User);
                // User ID of the comment
                dtoComment.UserId = c.UserId;
                // Tags of the comment
                dtoComment.Tags = c.Tags?.Select(t =>
                {
                    var dtoCommentTag = _converter.DtoTag(t);
                    return dtoCommentTag;
                }).ToArray();
                // UserTags of the comment
                dtoComment.UserTaggedComment = c.UserTaggedComment?.Select(t =>
                {
                    var dtoUserTaggedComment = _converter.DtoUserTaggedComment(t);
                    dtoUserTaggedComment.User = _converter.DtoUser(t.User);
                    return dtoUserTaggedComment;
                }).ToArray();
                return dtoComment;
            }).ToArray();
            // Likes
            dtoPost.Likes = post.Likes?.Select(c =>
            {
                var dtoLike = _converter.DtoLike(c);
                // Like Id of like
                dtoLike.Id = c.Id;
                // User of the like
                //dtoLike.User = _dtoConverter.DtoUser(c.User);
                // IsActive of the like
                dtoLike.IsActive = c.IsActive;
                // UserId of like
                dtoLike.UserId = c.UserId;
                // PostId of like
                dtoLike.PostId = c.PostId;
                return dtoLike;
            }).ToArray();
            dtoPost.Tags = post.Tags?.Select(c =>
            {
                var dtoTag = _converter.DtoTag(c);
                return dtoTag;
            }).ToArray();
            // UserTaggedPost
            dtoPost.UserTaggedPost = post.UserTaggedPost?.Select(u =>
            {
                var dtoTaggedPost = _converter.DtoUserTaggedPost(u);
                dtoTaggedPost.User = _converter.DtoUser(u.User);
                return dtoTaggedPost;
            }).ToArray();

            return dtoPost;
        }




        public ICollection<Post> GetByPredicate(Func<Post, bool> predicate)
        {
            return _context.Posts.Include(p=>p.Tags).Include(p=>p.UserTaggedPost).Where(predicate).Select(p=>dtoLogic(p)).ToList();
        }
    }
}