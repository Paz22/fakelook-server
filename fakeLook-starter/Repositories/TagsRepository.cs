using fakeLook_dal.Data;
using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fakeLook_starter.Repositories
{
    public class TagsRepository : ITagsRepository
    {

        readonly private DataContext _context;
        private IDtoConverter _converter;
        public TagsRepository(DataContext context, IDtoConverter dtoConverter)
        {
            _context = context;
            _converter = dtoConverter;
        }

        private bool TagExist(string content)
        {
            var res = _context.Tags.Where(item => item.Content == content).SingleOrDefault();
            return res != null;
        }


        public async Task<Tag> Add(Tag item)
        {
            if (!TagExist(item.Content))
            {
                var res = _context.Tags.Add(item);
                await _context.SaveChangesAsync();
                return res.Entity;
            }
            return item;
        }

        public async Task<Tag> Delete(int id)
        {
            var tag = GetById(id);
            if (tag == null)
            {
                return null;
            }
            var removed = _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return removed.Entity;
        }

        public ICollection<Tag> GetAll()
        {
            return _context.Tags.ToList();
        }

        public Tag GetById(int id)
        {
            return _context.Tags.Where((item) => item.Id == id).FirstOrDefault();
        }


    }
}
