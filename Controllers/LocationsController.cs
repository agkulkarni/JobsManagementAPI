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
    public class LocationsController : ApiController
    {
        private JobsEntities db = new JobsEntities();

        // GET: api/Locations
        /// <summary>
        /// Get all locations
        /// </summary>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        public IQueryable<Location> GetLocations()
        {
            return db.Locations;
        }

        // GET: api/Locations/5
        /// <summary>
        /// Get location by id
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> GetLocation(long id)
        {
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        // PUT: api/Locations/5
        /// <summary>
        /// Update location
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <param name="location">Updated location JSON request object</param>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLocation(long id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.LocId)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        // POST: api/Locations
        /// <summary>
        /// Create location
        /// </summary>
        /// <param name="location">Request JSON object</param>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> PostLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = location.LocId }, location);
        }

        // DELETE: api/Locations/5
        /// <summary>
        /// Delete location
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> DeleteLocation(long id)
        {
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            await db.SaveChangesAsync();

            return Ok(location);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(long id)
        {
            return db.Locations.Count(e => e.LocId == id) > 0;
        }
    }
}