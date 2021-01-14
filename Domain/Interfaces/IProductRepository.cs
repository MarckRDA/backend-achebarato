using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models.Cathegories;
using Domain.Models.Products;
using Domain.Products;

namespace Domain.Interfaces
{
    public interface IProductRepository : IMongoRepository<Product>
    {
        (IQueryable<Product> products, bool isThereAnyProductsInBD) GetFilterProductsByName(ProductQueryParameters search);

        Product GetProductById(Guid idProduct);
        
        void AddManyProductsAtOnce(List<Product> products);

        List<Cathegory> GetCathegories();

        List<Product> GetProductsByCategories(string category);

    }
}