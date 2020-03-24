using RestSharp;
using System;
using Xunit;
using HighScoreAPI.Controllers;
using HighScoreAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace HighScoreAPITest
{
    public class UnitTest1
    {
        DataContext dataContext;
        HighScoresController controller;
        public UnitTest1()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
.UseSqlServer("Server=localhost;Database=ShootingHighscore;User Id=sa;Password=Mssql!Server02;")
.Options;
            dataContext = new DataContext(options);
            controller = new HighScoresController(dataContext);
        }
        [Fact]
        public void GetEmptyHighScore()
        {
            Assert.Empty(controller.GetHighScores());
        }

        [Fact]
        public void AddHighScore()
        {
            HighScore highScore = new HighScore();
            highScore.Score = 150;
            highScore.User = "SCH";
            var ret = controller.PostHighScore(highScore).Result.Value;
            Assert.Single(controller.GetHighScores());
            Assert.Equal("SCH", ret.User);
            Assert.Equal(150, ret.Score);
        }
        [Fact]
        public async void GetOrderdList()
        {
            HighScore highScore = new HighScore();
            highScore.Score = 150;
            highScore.User = "SCH";
            await controller.PostHighScore(highScore);
            highScore.Score = 155;
            highScore.User = "RIE";
            await controller.PostHighScore(highScore);
            highScore.Score = 200;
            highScore.User = "HOF";
            await controller.PostHighScore(highScore);
            var list = controller.GetHighScores().ToArray();
            Assert.Equal(200, list[0].Score);
            Assert.Equal(155, list[1].Score);
            Assert.Equal(150, list[2].Score);
        }
        [Fact]
        public async void AddMoreThenTenHighscores()
        {
            HighScore highScore = new HighScore();
            for (var i = 1; i < 12; i++)
            {
                highScore.Score = 100 * i;
                highScore.User = "" + i;
                await controller.PostHighScore(highScore);
            }
            Assert.Equal(10, controller.GetHighScores().Count());
        }

        [Fact]
        public async void SendFalseRequest()
        {
            HighScore highScore = new HighScore();
            highScore.Score = 15000;
            await controller.PostHighScore(highScore);
            //TODO: Add Assert
        }
    }
}
