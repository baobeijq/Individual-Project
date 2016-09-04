using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApi.Models;

namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    public class ProductsController : ApiController
    {
        static readonly IProductRepository repository = new ProductRepository();

        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };
        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAll();
        }

        public Product GetProductById(int id)
        {
            Product item = repository.Get(id);
            //var product = products.FirstOrDefault((p) => p.Id == id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

/*        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));

            /*return products.Where(
               (p) => string.Equals(p.Category, category,
                   StringComparison.OrdinalIgnoreCase));#2#
        }*/

        public HttpResponseMessage PostProduct(Product item)
        {
            item = repository.Add(item);
            //响应码（Response code）：默认地，Web API框架把响应状态码设置为200（OK）。
            //但据HTTP/1.1协议，在POST请求形成资源创建时，服务器应当用状态201（已创建）进行回答。

            //这个CreateResponse方法创建一个HttpResponseMessage，
            //并自动地把一个序列化的Product对象表达式写入响应消息体
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);
            string uri = Url.Link("DefaultApi", new { id = item.Id });
            //当服务器创建一个资源时，它应当在响应的Location报头中包含新资源的URI。
            response.Headers.Location = new Uri(uri);
            return response;
        }

/*        //用PUT更新一个产品是很直观的：此方法采用两个参数，产品ID和被更新产品。
        //Id参数取自URI路径，而product参数通过请求解序列化。
        //默认地，ASP.NET Web API框架通过路由获取简单参数，而通过请求体获取复合类型。
        public void PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }*/

    }

}