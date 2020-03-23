using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HighScoreAPI.Data;

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
        public async Task<ActionResult<IEnumerable<HighScore>>> GetHighScores()
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
            await _context.HighScores.AddAsync(highScore);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHighScore", new { id = highScore.HighScoreId }, highScore);
        }

    }
}
