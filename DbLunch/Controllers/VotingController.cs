using DbLunch.Business;
using DbLunch.Data;
using DbLunch.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbLunch.Controllers
{
    public class VotingController: Controller
    {
        private readonly IVotingBusiness votingBusiness;
        private readonly IVoteRepository voteRepository;

        public VotingController(IVotingBusiness votingBusiness, IVoteRepository voteRepository)
        {
            this.votingBusiness = votingBusiness;
            this.voteRepository = voteRepository;
        }

        public async Task<IActionResult> Index([FromQuery] DateTime? date)
        {
            DateTime queryDate = date ?? DateTime.Now;
            if (queryDate.DayOfWeek == DayOfWeek.Saturday || queryDate.DayOfWeek==DayOfWeek.Sunday)
            {
                return Redirect("Voting");
            }
            var votingViewModel = await votingBusiness.GetVotingViewModel(queryDate);
            return View(votingViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterVote([FromBody] Vote vote)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await votingBusiness.RegisterVote(vote);
                    return Ok();
                }
                return BadRequest();
            }
            catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }

}
