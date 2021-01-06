using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TestesInterface.Fixtures;
using TestesInterface.PageObjects;
using Xunit;

namespace TestesInterface.Tests
{
    [Collection("Chrome Driver")]
    public class VotingTests
    {
        private IWebDriver driver;

        public VotingTests(TestFixture fixture)
        {
            this.driver = fixture.Driver;
        }

        [Fact]
        public void DeveCadastrarUmVoto()
        {
            //Arrange
            var votingPO = new VotingPO(driver);

            //Act
            votingPO.Visit();
            votingPO.NavigateToEmptyVoting();
            votingPO.FillForm();
            votingPO.Vote();

            //Assert
            Assert.Equal(1, votingPO.TotalVotes);
        }

        [Fact]
        public void DeveMostrarResultadoVotação()
        {
            //Arrange
            var votingPo = new VotingPO(driver);

            //Act
            votingPo.Visit();
            votingPo.NavigateToEmptyMonday();
            votingPo.AllVoteInRestaurant("1001");

            //Assert
            Assert.Contains("Restaurante Escolhido:", votingPo.ChosenRestaurantBanner.Text);
        }

        [Fact]
        public void MostrarVotaçãoEncerrada()
        {
            //Arrange
            var votingPo = new VotingPO(driver);

            //Act
            votingPo.Visit();
            votingPo.GoToPrevDay();

            //Assert
            Assert.Contains("Votação Encerrada.", votingPo.votingEndedBanner.Text);
        }

        [Fact]
        public void DeveImpedirQueUmMesmoRestauranteSejaEscolhidoNaMesmaSemana()
        {
            //Arrange
            var votingPO = new VotingPO(driver);

            //Act
            votingPO.Visit();
            votingPO.NavigateToEmptyMonday();
            votingPO.AllVoteInRestaurant("1001");

            votingPO.GoToNextDay();

            votingPO.AllVoteInRestaurant("1002");

            votingPO.GoToNextDay();

            //Assert
            Assert.True(votingPO.RestaurantIsDisabled("1001"));
            Assert.True(votingPO.RestaurantIsDisabled("1002"));
            Assert.False(votingPO.RestaurantIsDisabled("1003"));
            Assert.False(votingPO.RestaurantIsDisabled("1004"));
            Assert.False(votingPO.RestaurantIsDisabled("1005"));

        }

        [Fact]
        public void DeveImpedirQueVoteDuasVezes()
        {
            //Arrange
            var votingPO = new VotingPO(driver);

            //Act
            votingPO.Visit();
            votingPO.NavigateToEmptyMonday();

            votingPO.FillForm(workerId: "1001");
            votingPO.Vote();

            Assert.True(votingPO.GetWorkerOptionById("1001") == null);
        }
    }
}
