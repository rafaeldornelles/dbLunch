using DbLunch.Data;
using DbLunch.Models;
using DbLunch.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbLunch.Business
{
    public class VotingBusiness : IVotingBusiness
    {
        private readonly IVoteRepository voteRepository;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IWorkerRepository workerRepository;

        public VotingBusiness(IVoteRepository voteRepository, IRestaurantRepository restaurantRepository, IWorkerRepository workerRepository)
        {
            this.voteRepository = voteRepository;
            this.restaurantRepository = restaurantRepository;
            this.workerRepository = workerRepository;
        }

        private async Task<VotingViewModel> GetVotingViewModelWithoutWeekVerification(DateTime date)
        {
            var restaurants = await restaurantRepository.All();
            var votesInDate = await voteRepository.getVotesByDate(date);
            var voters = await workerRepository.All();
            var votersWithoutVote = voters.Where(v => !votesInDate.Any(vd => vd.voter_id == v.Id));
            var UnavaliableRestaurantIds = await GetWinningRestaurantsInWeek(date);
            var restaurantVotes = restaurants.Select(r => new RestaurantResult(
                Restaurant: r,
                Votes: votesInDate.Where(v => v.restaurant_id == r.Id).Count(),
                IsAvaliable: true
                ));

            return new VotingViewModel(date, restaurantVotes, votersWithoutVote);
        }

        public async Task<VotingViewModel> GetVotingViewModel(DateTime date)
        {
            var baseViewModel = await GetVotingViewModelWithoutWeekVerification(date);
            var unavaliableRestaurants = await GetWinningRestaurantsInWeek(date);
            baseViewModel.SetRestaurantIsAvaliable(unavaliableRestaurants);
            return baseViewModel;           
        }

        public async Task RegisterVote(Vote vote)
        {
            await ValidateVote(vote);
            await voteRepository.Insert(vote);
            return;
        }

        public async Task<List<int>> GetWinningRestaurantsInWeek(DateTime date)
        {
            var daysFromMonday = (int)date.DayOfWeek - (int) DayOfWeek.Monday;
            var startOfRequestWeek = date.AddDays(-daysFromMonday);

            List<int> winningRestaurantsIds = new List<int>();
            
            for(int i = 0; i<daysFromMonday; i++)
            {
                var checkedDate = startOfRequestWeek.AddDays(i);
                var votingVM = await GetVotingViewModel(checkedDate);

                if (!votingVM.AcceptsVotes && votingVM.TotalVotes > 0)
                {
                    winningRestaurantsIds.Add(votingVM.ChosenRestaurant.Restaurant.Id);
                }
            }
            return winningRestaurantsIds;
        }

        private async Task ValidateVote(Vote vote)
        {
            var votingVM = await GetVotingViewModel(vote.date);

            if (!votingVM.AcceptsVotes)
            {
                throw new ArgumentException("Voting on this day has ended.");
            }

            if (!votingVM.Voters.Select(v => v.Id).Contains(vote.voter_id))
            {
                throw new ArgumentException("Informed user cannot vote on this day.");
            }

            if(!votingVM.RestaurantResults.First(r=>r.Restaurant.Id == vote.restaurant_id).IsAvaliable)
            {
                throw new ArgumentException("Chosen restaurant is not avaliable to be voted.");
            }

            return;

        }
    }
}
