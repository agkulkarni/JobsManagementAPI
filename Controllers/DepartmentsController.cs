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
using System.Web.Http.Description;
using JobsManagementAPI.Models;

namespace JobsManagementAPI.Controllers
{
    public class DepartmentsController : ApiController
    {
        private JobsEntities db = new JobsEntities();

        // GET: api/Departments
        /// <summary>
        /// Get all departments
        /// </summary>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        public IQueryable<Department> GetDepartments()
        {
            return db.Departments;
        }

        // GET: api/Departments/5
        /// <summary>
        /// Get department by id
        /// </summary>
        /// <param name="id">Department ID</param>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        [ResponseType(typeof(Department))]
        public async Task<IHttpActionResult> GetDepartment(long id)
        {
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        // PUT: api/Departments/5
        /// <summary>
        /// Update existing department
        /// </summary>
        /// <param name="id">Department ID</param>
        /// <param name="department">Update department JSON request object</param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDepartment(long id, Department department)
        {
            try
            {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DeptId)
            {
                return BadRequest();
            }

            if (db.Departments.Where(a => a.DeptId == id).Any())
            {
                Department dept = db.Departments.Where(b => b.DeptId == id).FirstOrDefault();
                if (!string.IsNullOrEmpty(department.Title) && dept.Title != department.Title)
                    dept.Title = department.Title;

                await db.SaveChangesAsync();
            }
            else
                return NotFound();

            return StatusCode(HttpStatusCode.NoContent);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }            
        }

        // POST: api/Departments
        /// <summary>
        /// Create department
        /// </summary>
        /// <param name="department">Request JSON object</param>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        [ResponseType(typeof(Department))]
        public async Task<IHttpActionResult> PostDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = department.DeptId }, department);
        }

        // DELETE: api/Departments/5
        /// <summary>
        /// Delete department
        /// </summary>
        /// <param name="id">Department ID</param>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        [ResponseType(typeof(Department))]
        public async Task<IHttpActionResult> DeleteDepartment(long id)
        {
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            await db.SaveChangesAsync();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(long id)
        {
            return db.Departments.Count(e => e.DeptId == id) > 0;
        }
    }
}