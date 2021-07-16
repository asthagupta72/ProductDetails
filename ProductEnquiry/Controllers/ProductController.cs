using ProductEnquiry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace ProductEnquiry.Controllers
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {


        //Retrieve all the products from DB
        [HttpGet]
        [Route("ProductDetails")]
        public HttpResponseMessage Get()
        {
            try
            {
                using (ProductDataContext DbContext = new ProductDataContext())
                {
                    var getProducts = DbContext.products.ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, getProducts);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }

        [HttpPost]
        [Route("addNewProduct")]
        public HttpResponseMessage addNewProduct([FromBody] product newProduct)
        {
            try
            {
                using (ProductDataContext DbContext = new ProductDataContext())
                {
                    DbContext.products.Add(newProduct);
                    DbContext.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, newProduct);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + newProduct.product_id.ToString());

                    return message;

                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPut]
        [Route("modifyProduct/{id}")]
        public HttpResponseMessage modifyProduct(int id, [FromBody] product UpdateProduct)
        {
            try
            {
                using (ProductDataContext DbContext = new ProductDataContext())
                {
                    var productEntity = DbContext.products.FirstOrDefault(e => e.product_id == id);
                    if(productEntity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        productEntity.brand_name = UpdateProduct.brand_name;
                        productEntity.list_price = UpdateProduct.list_price;
                        productEntity.model_year = UpdateProduct.model_year;
                        productEntity.product_description = UpdateProduct.product_description;
                        productEntity.product_name = UpdateProduct.product_name;

                        DbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, productEntity);
                    }
                }

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        [Route("deleteProduct/{id}")]
        public HttpResponseMessage removeProduct(int id)
        {
            try
            {
                using (ProductDataContext DbContext = new ProductDataContext())
                {
                    var productEntity = DbContext.products.FirstOrDefault(e => e.product_id == id);
                    if(productEntity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Desired product with id =" + id.ToString() + " not to be found");
                    }
                    else
                    {
                        DbContext.products.Remove(productEntity);
                        DbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Employee with id " + id.ToString()+ " deleted from records");
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
