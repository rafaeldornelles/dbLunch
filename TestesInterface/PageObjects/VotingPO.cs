using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestesInterface.PageObjects
{
    class VotingPO
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private By byDateInput;
        private By byPrevButton;
        private By byNextButton;

        private By byRestaurantOptions;
        private By byWorkerSelect;

        private By bySendButton;

        private By byVotingEndedAlert;
        private By byTotalVotes;
        private By byTodayDate;
        private By byChosenRestaurantBanner;
        private By byInvalidVoterFeedback;

        public VotingPO(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            byDateInput = By.Id("week");
            byPrevButton = By.Id("prev");
            byNextButton = By.Id("next");
            byRestaurantOptions = By.CssSelector("input[type=radio][name=restaurant_id]");
            byWorkerSelect = By.CssSelector("select[name=voter_id] option:not([disabled])");
            bySendButton = By.CssSelector("button[type=submit]");
            byVotingEndedAlert = By.CssSelector(".alert.alert-danger");
            byTotalVotes = By.CssSelector("#total-votes");
            byTodayDate = By.Id("voteDate");
            byChosenRestaurantBanner = By.CssSelector(".alert-success");
            byInvalidVoterFeedback = By.Id("invalid-voter_id");

        }
        public IWebElement InvalidVoterFeedback => driver.FindElement(byInvalidVoterFeedback);

        public IWebElement ChosenRestaurantBanner
        {
            get
            {
                try
                {
                    return driver.FindElement(byChosenRestaurantBanner);
                }
                catch
                {
                    return null;
                }
            }
        }

        public bool VotingHasEnded
        {
            get
            {
                try
                {
                    return driver.FindElement(byVotingEndedAlert).Displayed;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void NavigateToEmptyMonday()
        {
            while(VotingHasEnded || TotalVotes!=0 || currentDay.DayOfWeek != DayOfWeek.Monday)
            {
                GoToNextDay();
            }
        }

        private IWebElement GetRestaurantRadioById(string id)
        {
            return RestaurantOptions.First(r => r.GetAttribute("value") == id);
        }

        public IWebElement GetWorkerOptionById(string id)
        {
            try
            {
            return WorkersOptions.First(r => r.GetAttribute("value") == id);

            }
            catch
            {
                return null;
            }
        }

        public void GoToNextDay()
        {
            NextButton.Click();
        }
        public bool RestaurantIsDisabled(string id)
        {
            try
            {
                return GetRestaurantRadioById(id).GetAttribute("disabled") != null;
            }
            catch
            {
                return false;
            }
        }
        public int TotalVotes => Convert.ToInt32(driver.FindElement(byTotalVotes).Text);
        private IWebElement NextButton => driver.FindElement(byNextButton);
        private DateTime currentDay => Convert.ToDateTime(driver.FindElement(byTodayDate).GetAttribute("value"));
        private IEnumerable<IWebElement> RestaurantOptions => driver.FindElements(byRestaurantOptions);
        private IEnumerable<IWebElement> WorkersOptions => driver.FindElements(byWorkerSelect);
        public void Visit()
        {
            driver.Navigate().GoToUrl("https://localhost:5001/");
        }

        public void NavigateToEmptyVoting()
        {
            while (VotingHasEnded || TotalVotes != 0)
            {
                GoToNextDay();
            }
        }

        public void FillForm(string? restaurantId = null, string? workerId = null)
        {
            var restaurant = restaurantId == null ?
                RestaurantOptions.First() :
                GetRestaurantRadioById(restaurantId);


            var workerSelect = workerId == null ?
                WorkersOptions.First() :
                GetWorkerOptionById(workerId);

            restaurant.Click();
            workerSelect.Click();
        }

        public void Vote()
        {
            var submitButton = driver.FindElement(bySendButton);
            submitButton.Click();
            wait.Until(d =>
            {
                try
                {
                    return RestaurantOptions.All(r => !r.Selected);
                }
                catch
                {
                    return true;
                }
            });
        }
        private IWebElement PrevButton => driver.FindElement(byPrevButton);
        public void GoToPrevDay()
        {
            PrevButton.Click();
        }

        public string WinningRestaurantId => RestaurantOptions.First().GetAttribute("value");

        public void SelectDate(DateTime date)
        {
            driver.FindElement(byDateInput).SendKeys("");
        }

        public void AllVoteInRestaurant(string restaurantId)
        {
            FillForm(restaurantId: restaurantId, "1001");
            Vote();
            FillForm(restaurantId: restaurantId, "1002");
            Vote();
            FillForm(restaurantId: restaurantId, "1003");
            Vote();
            FillForm(restaurantId: restaurantId, "1004");
            Vote();
            FillForm(restaurantId: restaurantId, "1005");
            Vote();

        }
        public IWebElement votingEndedBanner
        {
            get
            {
                try
                {
                    return driver.FindElement(byVotingEndedAlert);
                }
                catch
                {
                    return null;
                }
            }
        }
    }

}
