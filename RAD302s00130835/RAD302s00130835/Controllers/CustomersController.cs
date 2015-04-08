using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RAD302s00130835.Models;

namespace RAD302s00130835.Controllers
{
    [RoutePrefix("api/Cust")]
    public class CustomersController : ApiController
    {
        private nwEntities db = new nwEntities();

        // Display single customer
        [Route("GetCustomers")]
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers;
        }

        // Display all customers
        [Route("GetCustomer/{id}")]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(string id)
        {
            Customer cust = db.Customers.Find(id);
            if (cust == null)
            {
                return NotFound();
            }

            return Ok(cust);
        }

        // Display single customer
        [Route("GetOrders/{id}")]
        public dynamic GetOrders(string id)
        {
            Customer cust = db.Customers.Find(id);
            if (cust == null)
            {
                return NotFound();
            }

            //First attempt at assigning order details to customers

            foreach (Order ord in cust.Orders)
            {
                Ok(new
                {
                    id = ord.OrderID,
                    name = ord.ShipName,
                    street = ord.ShipAddress,
                    city = ord.ShipCity,
                    country = ord.ShipCountry
                });
            }

            return Ok();
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(string id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerID)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerID }, customer);
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(string id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(string id)
        {
            return db.Customers.Count(e => e.CustomerID == id) > 0;
        }
    }
}