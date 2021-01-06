namespace DbLunch.Models.ViewModels
{
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
