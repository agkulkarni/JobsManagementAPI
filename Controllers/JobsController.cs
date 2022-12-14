using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using JobsManagementAPI.Models;

namespace JobsManagementAPI.Controllers
{
    [Authorize]
    public class JobsController : ApiController
    {
        private JobsEntities db = new JobsEntities();
        /// <summary>
        /// Create new job
        /// </summary>
        /// <param name="job">JSON request object</param>
        /// <returns>Valid HTTP response with generated job id</returns>
        [HttpPost]
        [Route("api/v1/jobs")]
        public async Task<IHttpActionResult> PostJob(Job job)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                db.Jobs.Add(job);
                await db.SaveChangesAsync();
                return Ok(job.JobId);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }            
        }
        /// <summary>
        /// Update exisiting job
        /// </summary>
        /// <param name="id">Job id to be updated</param>
        /// <param name="job">Updated job JOSN request object</param>
        /// <returns>Valid HTTP response with generated job id</returns>
        [HttpPut]
        [Route("api/v1/jobs/{id}")]
        public async Task<IHttpActionResult> PutJob(long id, Job job)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != job.JobId)
                    return BadRequest();

                if (db.Jobs.Where(a => a.JobId == id).Any())
                {
                    Job job1 = db.Jobs.Where(b => b.JobId == id).FirstOrDefault();
                    if (!string.IsNullOrEmpty(job.Title) && job.Title != job1.Title)
                        job1.Title = job.Title;

                    if (!string.IsNullOrEmpty(job.Description) && job.Title != job1.Description)
                        job1.Description = job.Description;

                    if (job.LocId > 0 && job.LocId != job1.LocId)
                        job1.LocId = job.LocId;

                    if (job.DeptId > 0 && job.DeptId != job1.DeptId)
                        job1.DeptId = job.DeptId;

                    if (job.PostDate.Date != job1.PostDate.Date)
                        job1.PostDate = job.PostDate;

                    if (job.ClosingDate.Date != job1.ClosingDate.Date)
                        job1.ClosingDate = job.ClosingDate;

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

        /// <summary>
        /// Get existing job details
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        [HttpGet]
        [Route("api/v1/jobs/{id}")]
        public async Task<IHttpActionResult> GetJob(long id)
        {
            try
            {
                Job job = null;
                JobDetails details = null;
                if(db.Jobs.Where(a => a.JobId == id).Any())
                {
                    job = db.Jobs.FirstOrDefault(a => a.JobId == id);
                    details = new JobDetails();
                    details.id = job.LocId;
                    details.code = job.JobCode;
                    details.title = job.Title;
                    details.description = job.Description;
                    details.location = new Location() { LocId=job.Location.LocId, Name=job.Location.Name, City=job.Location.City, Country=job.Location.Country, State=job.Location.State, Zip=job.Location.Zip };
                    details.department = new Department() { DeptId = job.Location.LocId, Title = job.Department.Title };
                    details.postedtDate = job.PostDate;
                    details.closingDate = job.ClosingDate;

                    return Ok(details);
                }                    
                else
                    return NotFound();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get jobs list based on input parameters
        /// </summary>
        /// <param name="jobsListRequest">JSON request object</param>
        /// <returns>Valid HTTP response with generated JSON response</returns>
        [HttpPost]
        [Route("api/jobs/list")]
        [Authorize]
        public IHttpActionResult GetJobsList(JobsListRequest jobsListRequest)
        {
            try
            {
                if (jobsListRequest == null)
                    return BadRequest("Request JSON mandatory");

                using(SqlConnection con = new SqlConnection("Data Source=beast;Initial Catalog=Jobs;Integrated Security=True;MultipleActiveResultSets=True;"))
                {
                    SqlCommand cmd = new SqlCommand("GetJobDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(!string.IsNullOrEmpty(jobsListRequest.q))
                        cmd.Parameters.Add(new SqlParameter("@SearchStr", SqlDbType.NVarChar, jobsListRequest.q.Length)).Value = jobsListRequest.q;
                    if(jobsListRequest.pageNo > 0 && jobsListRequest.pageSize > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.Int)).Value = jobsListRequest.pageNo;
                        cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int)).Value = jobsListRequest.pageSize;
                    }                        
                    if(jobsListRequest.locationId > 0)
                        cmd.Parameters.Add(new SqlParameter("@LocationId", SqlDbType.Int)).Value = jobsListRequest.locationId;
                    if(jobsListRequest.departmentId > 0)
                        cmd.Parameters.Add(new SqlParameter("@DepartmentID", SqlDbType.Int)).Value = jobsListRequest.departmentId;
                    con.Open();
                    DataTable dtJobsList = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dtJobsList);

                    if (dtJobsList.Rows.Count > 0)
                    {
                        JobsList jobsList = new JobsList();
                        jobsList.total = dtJobsList.Rows.Count;
                        List<JobDet> jobs = new List<JobDet>();
                        JobDet job = null;

                        foreach(DataRow dr in dtJobsList.Rows)
                        {
                            job = new JobDet();
                            job.id = Convert.ToInt64(dr["JobId"]);
                            job.code = Convert.ToString(dr["JobCode"]);
                            job.title = Convert.ToString(dr["Title"]);
                            job.location = Convert.ToString(dr["location"]);
                            job.department = Convert.ToString(dr["department"]);
                            job.closingDate = Convert.ToDateTime(dr["ClosingDate"]);
                            job.postedDate = Convert.ToDateTime(dr["PostDate"]);
                            jobs.Add(job);
                        }
                        jobsList.data = jobs;
                        return Ok(jobsList);
                    }
                    else
                        return NotFound();
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Jobs
        //public IQueryable<Job> GetJobs()
        //{
        //    return db.Jobs;
        //}

        //// GET: api/Jobs/5
        //[ResponseType(typeof(Job))]
        //public async Task<IHttpActionResult> GetJob(long id)
        //{
        //    Job job = await db.Jobs.FindAsync(id);
        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(job);
        //}

        //// PUT: api/Jobs/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutJob(long id, Job job)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != job.JobId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(job).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!JobExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Jobs
        //[ResponseType(typeof(Job))]
        //public async Task<IHttpActionResult> PostJob(Job job)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Jobs.Add(job);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = job.JobId }, job);
        //}

        //// DELETE: api/Jobs/5
        //[ResponseType(typeof(Job))]
        //public async Task<IHttpActionResult> DeleteJob(long id)
        //{
        //    Job job = await db.Jobs.FindAsync(id);
        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Jobs.Remove(job);
        //    await db.SaveChangesAsync();

        //    return Ok(job);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool JobExists(long id)
        //{
        //    return db.Jobs.Count(e => e.JobId == id) > 0;
        //}
    }
}