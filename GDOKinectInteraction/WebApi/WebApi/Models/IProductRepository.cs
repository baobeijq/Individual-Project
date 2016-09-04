﻿using System.Collections.Generic;

namespace WebApi.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product Get(int id);
        Product Add(Product item);
/*        void Remove(int id);
        bool Update(Product item);*/
    }
}