
   
    using fakeLook_models.Models;
    using fakeLook_starter.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace fakeLook_starter.Services
    {
        public class DtoConverter : IDtoConverter
        {
            public Comment DtoComment(Comment comment)
            {
                return new Comment() { Id = comment.Id, Content = comment.Content };
            }

            public Like DtoLike(Like like)
            {
                return new Like() { IsActive = like.IsActive };
            }

            public Post DtoPost(Post post)
            {
                return new Post() { Id = post.Id, Date = post.Date, Description = post.Description, ImageSorce = post.ImageSorce, X_Position = post.X_Position, Y_Position = post.Y_Position, Z_Position = post.Z_Position, };
            }

            public Tag DtoTag(Tag tag)
            {
                return new Tag() { Id = tag.Id, Content = tag.Content };
            }

            public User DtoUser(User user)
            {
                return new User() { Id = user.Id, UserName = user.UserName };
            }

            public UserTaggedComment DtoUserTaggedComment(UserTaggedComment userTaggedComment)
            {
                return new UserTaggedComment() { User = DtoUser(userTaggedComment.User) };
            }

            public UserTaggedPost DtoUserTaggedPost(UserTaggedPost userTaggedPost)
            {
                return new UserTaggedPost() { User = DtoUser(userTaggedPost.User) };
            }
        }
    }

