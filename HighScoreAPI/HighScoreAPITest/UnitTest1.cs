using RestSharp;
using System;
using Xunit;
using HighScoreAPI.Controllers;
using HighScoreAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using RestSharp.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HighScoreAPITest
{
    public class UnitTest1
    {
        DataContext dataContext;
        HighScoresController controller;
        public UnitTest1()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
.UseCosmos("https://csm.documents.azure.com:443",
            "o9t7nHTTr1ch22TvmKIXtv6MNS1bxG2Ukmk48J4EdXg68WR3nSg2EAItJSPIeAnkRKaDPem9KFdFsTFbSLV3mw==", "ShootingHighscore")
.Options;
            dataContext = new DataContext(options);
            controller = new HighScoresController(dataContext);
        }
        [Fact]
        public async void AddMoreThenHighScoresAndNewHighScore()
        {
            dataContext.HighScores.RemoveRange(controller.GetHighScores());
            await dataContext.SaveChangesAsync();
            HighScore highScore;
            for (var i = 1; i < 12; i++)
            {
                highScore = new HighScore();
                highScore.Score = 100 * i;
                highScore.User = "" + i;
                await controller.AddHighScore(highScore);
            }
            highScore = new HighScore();
            highScore.Score = 999999;
            highScore.User = "TES";
            await controller.AddHighScore(highScore);
            Assert.Equal(highScore.Score, controller.GetHighScores().ElementAt(0).Score);
            dataContext.HighScores.RemoveRange(controller.GetHighScores());
            await dataContext.SaveChangesAsync();
        }
        [Fact]
        public async void AddHighScore()
        {
            dataContext.HighScores.RemoveRange(controller.GetHighScores());
            await dataContext.SaveChangesAsync();
            HighScore highScore = new HighScore();
            highScore.Score = 1150;
            highScore.User = "SCH";
            await controller.AddHighScore(highScore);
            Assert.Single(controller.GetHighScores());
            dataContext.HighScores.RemoveRange(controller.GetHighScores());
            await dataContext.SaveChangesAsync();
        }

        [Fact]
        public async void GetOrderdList()
        {
            HighScore highScore = new HighScore();
            highScore.Score = 150;
            highScore.User = "SCH";
            await controller.AddHighScore(highScore);
            highScore = new HighScore();
            highScore.Score = 155;
            highScore.User = "RIE";
            await controller.AddHighScore(highScore);
            highScore = new HighScore();
            highScore.Score = 200;
            highScore.User = "HOF";
            await controller.AddHighScore(highScore);
            var list = controller.GetHighScores().ToArray();
            Assert.True(list[0].Score >= list[1].Score);
            Assert.True(list[1].Score >= list[2].Score);
            Assert.True(list[0].Score >= 200);
            dataContext.HighScores.RemoveRange(controller.GetHighScores());
            await dataContext.SaveChangesAsync();
        }
        [Fact]
        public async void AddMoreThenTenHighscores()
        {

            for (var i = 1; i < 12; i++)
            {
                HighScore highScore = new HighScore();
                highScore.Score = 100 * i;
                highScore.User = "" + i;
                await controller.AddHighScore(highScore);
            }
            Assert.Equal(10, controller.GetHighScores().Count());
            dataContext.HighScores.RemoveRange(controller.GetHighScores());
            await dataContext.SaveChangesAsync();
        }

        [Fact]
        public async void SendFalseRequest()
        {
            dataContext.HighScores.RemoveRange(controller.GetHighScores());
            await dataContext.SaveChangesAsync();
            HighScore highScore = new HighScore();
            highScore.Score = -150;
            await controller.AddHighScore(highScore);
            Assert.Empty(controller.GetHighScores());
        }
    }
}
