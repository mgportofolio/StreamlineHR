using Microsoft.AspNetCore.Mvc;
using StreamlineHR.Commons.Model;
using StreamlineHR.Entities;
using StreamlineHR.Models;
using StreamlineHR.Services;
using System.ComponentModel.DataAnnotations;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StreamlineHR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly JobService _jobMan;

        public JobController(JobService jobService)
        {
            _jobMan = jobService;
        }

        /// <summary>
        /// Get list of jobs from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResultStatus<Job>), 200)]
        [ProducesResponseType(typeof(ResultStatus<Job>), 400)]
        [ProducesResponseType(typeof(ResultStatus<Job>), 500)]
        public async Task<ActionResult<ResultStatus<List<Job>>>> GetAsync()
        {
            return await this._jobMan.GetListOfJob();
        }

        /// <summary>
        /// Insert job to database
        /// </summary>
        /// <param name="jobInsert"></param>
        /// <returns>A <see cref="Task{ResultStatus{Job}}"/> representing the asynchronous operation. The <see cref="ResultStatus{T}"/> contains the list of jobs if successful.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResultStatus<Job>), 201)]
        [ProducesResponseType(typeof(ResultStatus<Job>), 404)]
        [ProducesResponseType(typeof(ResultStatus<Job>), 500)]
        public async Task<ResultStatus<Job>> Post([FromBody] JobInsertModel jobInsert)
        {
            var job = new Job
            {
                Title = jobInsert.Title,
                Content = jobInsert.Content,
            };

            var rs = await _jobMan.InsertJob(job);
            if(!rs.Status || rs.Code != 201)
            {
                rs.Data = null;
            }
            return rs;
        }
    }
}
