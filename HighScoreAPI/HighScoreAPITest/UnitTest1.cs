using RestSharp;
using System;
using Xunit;
using HighScoreAPI.Controllers;
using HighScoreAPI.Data;

namespace HighScoreAPITest
{
    public class UnitTest1
    {
       
        [Fact]
        public void AddHighScore()
        {
            DataContext dataContext = new DataContext();
            HighScoresController controller = new HighScoresController(dataContext);
            //controller.GetHighScores().
        }
        [Fact]
        public void AddAnotherHighScore()
        {

        }
        [Fact]
        public void AddMoreThenTenHighscores()
        {

        }

        [Fact]
        public void SendFalseRequest()
        {

        }
    }
}
