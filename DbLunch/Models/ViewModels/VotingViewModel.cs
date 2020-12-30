using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbLunch.Models.ViewModels
{
    public class VotingViewModel
    {
        public IEnumerable<Worker> Voters { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<RestaurantResult> RestaurantResults { get; set; }
        public IEnumerable<RestaurantResult> OrderedRestaurantResults => RestaurantResults.OrderByDescending(r => r.IsAvaliable).ThenByDescending(r => r.Votes).ThenBy(r => r.Restaurant.Name);
        public int TotalVotes => RestaurantResults.ToList().Aggregate(0, (total, rest) => rest.Votes + total);
        public bool AcceptsVotes => this.Date.Date.AddHours(12).CompareTo(DateTime.Now) > 0 && Voters.Count() > 0;
        public RestaurantResult ChosenRestaurant => this.TotalVotes > 0? this.OrderedRestaurantResults.First() : null;
        public VotingViewModel(DateTime date, IEnumerable<RestaurantResult> restaurantResults, IEnumerable<Worker> voters)
        {
            Date = date;
            this.RestaurantResults = restaurantResults;
            Voters = voters;
        }
        public void SetRestaurantIsAvaliable(IEnumerable<int> UnavaliableRestaurantIds) {
            this.RestaurantResults = this.RestaurantResults.Select(r =>
            {
                if (UnavaliableRestaurantIds.Contains(r.Restaurant.Id))
                {
                    r.IsAvaliable = false;
                }
                return r;
            });
        }
    }

    public class RestaurantResult
    {
        public Restaurant Restaurant { get; set; }
        public int Votes { get; set; }
        public bool IsAvaliable { get; set; }

        public RestaurantResult(Restaurant Restaurant, int Votes, bool IsAvaliable)
        {
            this.Restaurant = Restaurant;
            this.Votes = Votes;
            this.IsAvaliable = IsAvaliable;
        }
    }
}
