using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HighScoreAPI.Data;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

namespace HighScoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HighScoresController : ControllerBase
    {
        private readonly DataContext _context;

        public HighScoresController(DataContext context)
        {
            _context = context;
        }

        // GET: api/HighScores
        [HttpGet]
        public IEnumerable<HighScore> GetHighScores()
        {
            return _context.HighScores.OrderByDescending(h => h.Score).ToList();
        }

        // GET: api/HighScores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HighScore>> GetHighScore(int id)
        {
            var highScore = await _context.HighScores.FindAsync(id);

            if (highScore == null)
            {
                return NotFound();
            }

            return highScore;
        }

        // POST: api/HighScores
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<HighScore>> PostHighScore(HighScore highScore)
        {
            if (_context.HighScores.Count() >= 10)
            {
                var lastHighscore = _context.HighScores.OrderByDescending(h => h.Score).Last();
                if (lastHighscore.Score < highScore.Score)
                {
                    _context.HighScores.Remove(lastHighscore);
                }
                else
                {
                    return BadRequest("Not a highScore");
                }
            }
            _context.HighScores.Add(highScore);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHighScore", new { id = highScore.HighScoreId }, highScore);
        }
        public static bool ReCaptchaPassed(string gRecaptchaResponse)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?6LfetuMUAAAAALher4htFllDm9IaROp6FYqfT9GW").Result;
            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
                return false;

            return true;
        }
    }
}
