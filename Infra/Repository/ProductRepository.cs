using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Models.Cathegories;
using Domain.Models.Descriptions;
using Domain.Models.Products;
using MongoDB.Driver;

namespace Infra.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoRepository<Product> _repository;
        private readonly IMongoSettings _settings;
        private readonly IMongoCollection<Product> _collection;

        public ProductRepository(IMongoRepository<Product> repository, IMongoSettings settings)
        {
            _repository = repository;
            _settings = settings;
            var database = new MongoClient("mongodb://root:AcheBaratoMongoDB2021!@localhost:27017").GetDatabase("AcheBarato");
            _collection = database.GetCollection<Product>("Products");
        }

        public void add(Product entity)
        {
            _repository.add(entity);
        }

        public void AddManyProductsAtOnce(List<Product> products)
        {
            _collection.InsertManyAsync(products);
        }

        public IEnumerable<Product> GetAllElements()
        {
            return _repository.GetAllElements();
        }

        public IEnumerable<Product> GetTrendProducts()
        {
            return _collection.Find(x => x.isTrending == true).ToList();
        }

        public List<Cathegory> GetCathegories()
        {
            return _collection.AsQueryable().Select(x => x.Cathegory).Distinct().ToList();
            
        }

        public (IQueryable<Product> products, bool isThereAnyProductsInBD) GetFilterProductsByName(ProductQueryParameters parameters)
        {
            var splitedSearch = parameters.Search.ToUpper().Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries).ToList();
            var builder =Builders<Product>.Filter;
            var filterTag = builder.All(x => x.Tag, splitedSearch) & builder.Gte(p => p.Price, parameters.MinPrice) & builder.Lte(p => p.Price, parameters.MaxPrice);
            var p =_collection.Find(filterTag).ToList().AsQueryable();
            return (p, p.Count() > 10);
        }

        public List<Product> GetProductsByCategories(string category)
        {
           return _collection.AsQueryable().Where(x => x.Cathegory.Name == category).ToList();
        }

        public List<Description> GetProductDescription(Guid idProduct)
        {
            var product = GetEntityById(x => x.id_product, idProduct);

            return product.Descriptions;
        }

        public Product GetEntityById(Expression<Func<Product, Guid>> function, Guid value)
        {
            return _repository.GetEntityById(function, value);
        }
    }
}