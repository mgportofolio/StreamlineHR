using Microsoft.EntityFrameworkCore;
using StreamlineHR.Commons.Model;
using StreamlineHR.Data;
using StreamlineHR.Entities;
using System.Net;

namespace StreamlineHR.Services
{
    public class AppliedService
    {
        private readonly TrelloService _trelloMan;
        private readonly AppDbContext _dbMan;

        public AppliedService(AppDbContext db, TrelloService trelloService) {
            _trelloMan = trelloService;
            _dbMan = db;
        }

        /// <summary>
        /// Check existing job, 
        /// check existing candidate via phone and email, if not found create new candidate
        /// insert to applied
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<ResultStatus<Applied>> CandidateAppliedJob(Candidate candidate, int jobId)
        {
            var rs = new ResultStatus<Applied>();
            var existingJob = await _dbMan.Jobs
                .Where(Q => Q.Id == jobId)
                .FirstOrDefaultAsync();

            if(existingJob == null)
            {
                rs.Code = (int)HttpStatusCode.NotFound;
                rs.Status = false;
                rs.Message = "Job Doesn't exists";
                rs.Data = null;
                return rs;
            }

            var existingCandidate = await _dbMan.Candidates
                .Where(Q => Q.Email == candidate.Email || Q.Phone == candidate.Phone)
                .FirstOrDefaultAsync();
            var candidateId = existingCandidate?.Id;

            if (existingCandidate == null)
            {
                var newCandidate = await _dbMan.AddAsync<Candidate>(candidate);
                await _dbMan.SaveChangesAsync();
                candidateId = newCandidate.Entity.Id;
            }

            var existingApplied = await _dbMan.Applieds
                .Where(Q => Q.CandidateId == candidateId && Q.JobId == jobId)
                .FirstOrDefaultAsync();

            if (existingApplied != null)
            {
                rs.Code = (int) HttpStatusCode.BadRequest;
                rs.Status = false;
                rs.Message = "Candidate already applied for this job";
                rs.Data = existingApplied;
                return rs;
            }

            var newApplied = new Applied
            {
                CandidateId = candidateId ?? 0,
                JobId = existingJob.Id
            };

            var appliedResult = await _dbMan.AddAsync<Applied>(newApplied);
            await _dbMan.SaveChangesAsync();

            rs.Status = true;
            rs.Message = "Candidate applied job successfully";
            rs.Data = appliedResult.Entity;

            var title = $"[CANDIDATE] {candidate.Name} - {existingJob.Title}";
            var desc = $"Name : {candidate.Name} \n About : {candidate.About} \n Email : {candidate.Email} \n Phone : {candidate.Phone}";
            var trelloResult = await _trelloMan.PostCard(title, desc);

            if (!trelloResult.Status)
            {
                rs.Message = "Candidate applied job successfully! Failed to insert to trello";
            }

            return rs;
        }
    }
}
