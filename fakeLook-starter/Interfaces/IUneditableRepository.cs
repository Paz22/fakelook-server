using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fakeLook_starter.Interfaces
{
    //interface to be implemented by all repositories handing immutable models
    public interface IUneditableRepository<T>
    {
        
            public Task<T> Add(T item);
            public ICollection<T> GetAll();
            public Task<T> Delete(int id);

            public T GetById(int id);
        
    }
}
