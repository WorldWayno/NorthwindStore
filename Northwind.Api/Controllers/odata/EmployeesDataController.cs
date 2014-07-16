using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Northwind.Data;
using Northwind.Model;

namespace Northwind.Api.Controllers.odata
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using Northwind.Model;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Employee>("Employees");
    builder.EntitySet<Order>("Orders"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
   
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("employeesdata")]
    public class EmployeesDataController : ODataController
    {
        private readonly IRepository<Employee> _repository;

        public EmployeesDataController(IRepository<Employee> repository)
        {
            
        }
        // GET: odata/Employees
        [Queryable]
        public IQueryable<Employee> GetEmployees()
        {
            return _repository.Queryable();
        }

        // GET: odata/Employees(5)
        [EnableQuery()]
        public SingleResult<Employee> GetEmployee([FromODataUri] int key)
        {
            return SingleResult.Create(_repository.Queryable().Where(employee => employee.EmployeeID == key));
        }

        // PUT: odata/Employees(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != employee.EmployeeID)
            {
                return BadRequest();
            }

            //_repository.Entry(employee).State = EntityState.Modified;

            try
            {
                _repository.Update(employee);
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(employee);
        }

        // POST: odata/Employees
        public async Task<IHttpActionResult> Post(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(employee);
            await _repository.SaveChangesAsync();

            return Created(employee);
        }

        // PATCH: odata/Employees(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Employee> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Employee employee =  _repository.Find(key);
            if (employee == null)
            {
                return NotFound();
            }

            patch.Patch(employee);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(employee);
        }

        // DELETE: odata/Employees(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Employee employee = _repository.Find(key);
            if (employee == null)
            {
                return NotFound();
            }

            _repository.Remove(employee);
            await _repository.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Employees(5)/Orders
        [Queryable]
        public IQueryable<Order> GetOrders([FromODataUri] int key)
        {
            return _repository.Queryable().Where(m => m.EmployeeID == key).SelectMany(m => m.Orders);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool EmployeeExists(int key)
        {
            return _repository.Queryable().Count(e => e.EmployeeID == key) > 0;
        }
    }
}
