﻿using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class PostRepository : IPostRepository
    {
        readonly private DataContext _context;
        public PostRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Post> Add(Post item)
        {
            var res = _context.Posts.Add(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Post> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                //TODO
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post> Edit(Post item)
        {
            item.IsEdited = true;
            var res = _context.Posts.Update(item);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public ICollection<Post> GetAll()
        {
            return _context.Posts.ToList();
        }

        public Post GetById(Guid id)
        {
            return _context.Posts.SingleOrDefault(p => p.Id == id);
        }


        public ICollection<Post> GetByPredicate(Func<Post, bool> predicate)
        {
            return _context.Posts.Where(predicate).ToList();
        }
    }
}