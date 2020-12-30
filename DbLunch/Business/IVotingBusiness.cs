using DbLunch.Models;
using DbLunch.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace DbLunch.Business
{
    public interface IVotingBusiness
    {
        public Task<VotingViewModel> GetVotingViewModel(DateTime date);
        public Task RegisterVote(Vote vote);
    }
}