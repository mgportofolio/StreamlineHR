using Microsoft.EntityFrameworkCore;
using StreamlineHR.Commons.Model;
using StreamlineHR.Data;
using StreamlineHR.Entities;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StreamlineHR.Services
{
    public class JobService
    {
        private readonly AppDbContext _dbMan;

        public JobService(AppDbContext db) {
            _dbMan = db;
        }

        public async Task<ResultStatus<List<Job>>> GetListOfJob()
        {
            var rs = new ResultStatus<List<Job>>();

            try
            {
                List<Job> jobs = await _dbMan.Jobs.Where(Q => Q.Status >= 0).ToListAsync();
                if(jobs.Count > 0)
                {
                    rs.Data = jobs;
                }
                else
                {
                    rs.Code = (int)HttpStatusCode.NotFound;
                    rs.Status = false;
                    rs.Data = null;
                }
            }
            catch (Exception ex)
            {
                rs.Code = (int) HttpStatusCode.InternalServerError;
                rs.Status = false;
                rs.Message = $"Error: Failed get list of jobs. {ex.Message}";
            }

            return rs;
        }

        public async Task<ResultStatus<Job>> InsertJob(Job job)
        {
            var rs = new ResultStatus<Job>();
            try
            {
                var existingJob = await _dbMan.Jobs
                   .Where(Q => Q.Title == job.Title)
                   .FirstOrDefaultAsync();

                if (existingJob != null)
                {
                    rs.Code = (int) HttpStatusCode.BadRequest;
                    rs.Status = false;
                    rs.Message = "Job already exists";
                    return rs;
                }

                var res = _dbMan.Add<Job>(job);
                await _dbMan.SaveChangesAsync();
                rs.Code = (int) HttpStatusCode.Created;
                rs.Data = res.Entity;
            }
            catch (Exception ex)
            {
                rs.Code = (int) HttpStatusCode.InternalServerError;
                rs.Status = false;
                rs.Message = $"Error: Failed insert to job. {ex.Message}";
                return rs;
            }

            return rs;
        }
    }
}
