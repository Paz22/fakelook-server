﻿using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class LikesRepository : ILikeRepository
    {

        readonly private DataContext _context;
        private IDtoConverter _converter;
        public LikesRepository(DataContext context, IDtoConverter dtoConverter)
        {
            _context = context;
            _converter = dtoConverter;
        }

        public async Task<Like> Add(Like item)
        {
            if(LikeExist(item))
            {
                return await Delete(item.Id);
            }
            else
            {
                var addedlike= _context.Add(item);
                await _context.SaveChangesAsync();
                return addedlike.Entity;
            }
        }

        private bool LikeExist(Like item)
        {
            var like = _context.Likes.Where(l => l.UserId == item.UserId && l.PostId == item.PostId);
            return like != null;
        }

        public async Task<Like> Delete(int id)
        {
           var like = GetById(id);
           like.IsActive = false;
           await _context.SaveChangesAsync();
           return like;
        }

        public ICollection<Like> GetAll()
        {
            return _context.Likes.ToList();
        }

        public Like GetById(int id)
        {
            return _context.Likes.Where(_like => _like.Id == id).FirstOrDefault();
        }

      

       

    }
}
