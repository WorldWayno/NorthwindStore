using Northwind.Data;
using Northwind.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Northwind.Api.Controllers
{
    [RoutePrefix("employees")]
    public class EmployeesController : ApiController
    {
        private readonly NorthwindContext db = new NorthwindContext();

       
        /// <summary>
        ///   Get all the Employees in an org
        /// </summary>
        /// <remarks>I like this remark</remarks>
        /// <returns><see cref="Employee"/></returns>
        
        [HttpGet]
        [Route("")]
        public IQueryable<Employee> GetEmployees()
        {
            return db.Employees;
        }

        
        /// <summary>
        ///  Gets an employee by name
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Employee"/></returns>
        
        [Route("{id:int}")]
        [ResponseType(typeof (Employee))]
        public async Task<IHttpActionResult> GetEmployee(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [HttpPut]
        [Route("")]
        [ResponseType(typeof (void))]
        public async Task<IHttpActionResult> PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employees
        [HttpPost]
        [Route("")]
        [ResponseType(typeof (Employee))]
        public async Task<IHttpActionResult> PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Employees.Add(employee);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new {id = employee.EmployeeID}, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete]
        [Route("")]
        [ResponseType(typeof (Employee))]
        public async Task<IHttpActionResult> DeleteEmployee(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            await db.SaveChangesAsync();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeID == id) > 0;
        }
    }
}