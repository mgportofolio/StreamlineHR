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
    public class AppliedController : ControllerBase
    {
        private readonly AppliedService _candidateMan;

        public AppliedController(AppliedService candidateService) {

            _candidateMan = candidateService;
        }
        
        /// <summary>
        /// Candidate applies job, will create candidate automatically (unique key: email and phone)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultStatus<Applied>> Post([FromBody] CandidateAppliesJob data)
        {
            var rs = new ResultStatus<Applied>();
            var candidate = new Candidate
            {
                Phone = data.Phone,
                Email = data.Email,
                Name = data.Name,
                About = data.About
            };
            try
            {
                rs = await _candidateMan.CandidateAppliedJob(candidate, data.JobId);
                if(!rs.Status)
                {
                    Response.StatusCode = rs.Code;
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                rs.Data = null;
                rs.Message = ex.Message;
                rs.Status = false;
            }

            return rs;
        }
    }       
}
