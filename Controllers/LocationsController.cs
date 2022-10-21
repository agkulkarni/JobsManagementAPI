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
        public IHttpActionResult GetLocations()
        {
            return Ok(db.Locations.ToList<Location>());
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

            if (db.Locations.Where(a => a.LocId == id).Any())
            {
                Location loc = db.Locations.Where(b => b.LocId == id).FirstOrDefault();
                if (!string.IsNullOrEmpty(location.Name) && loc.Name != location.Name)
                    loc.Name = location.Name;

                if (!string.IsNullOrEmpty(location.City) && loc.City != location.City)
                    loc.City = location.City;

                if (!string.IsNullOrEmpty(location.State) && loc.State != location.State)
                    loc.State = location.State;

                if (!string.IsNullOrEmpty(location.Country) && loc.Country != location.Country)
                    loc.Country = location.Country;

                if (!string.IsNullOrEmpty(location.Zip) && loc.Zip != location.Zip)
                    loc.Zip = location.Zip;

                db.SaveChanges();
            }
            else
                return NotFound();

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