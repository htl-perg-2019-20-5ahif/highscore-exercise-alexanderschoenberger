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
            Assert.Equal(controller.GetHighScores().Result.Result, OK);
            //controller.GetHighScores().
        }
        [Fact]
        public async void AddAnotherHighScoreAsync()
        {
            DataContext dataContext = new DataContext();
            HighScoresController controller = new HighScoresController(dataContext);
            HighScore highScore = new HighScore();
            highScore.Score = 150;
            await controller.PostHighScore(highScore);
        }
        [Fact]
        public async void AddMoreThenTenHighscores()
        {
            DataContext dataContext = new DataContext();
            HighScoresController controller = new HighScoresController(dataContext);
            HighScore highScore = new HighScore();
            for (var i = 1; i < 12; i++)
            {
                highScore.Score = 100 * i;
                await controller.PostHighScore(highScore);
            }
        }

        [Fact]
        public void SendFalseRequest()
        {
            DataContext dataContext = new DataContext();
            HighScoresController controller = new HighScoresController(dataContext);
            HighScore highScore = new HighScore();
            // false Request?
            controller.PostHighScore(highScore);
        }
    }
}
