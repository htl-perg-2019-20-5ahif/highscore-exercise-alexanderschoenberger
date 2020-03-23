using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HighScoreAPI.Data
{
    public class HighScore
    {
        [Key]
        public int HighScoreId { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public int Score { get; set; }
    }
}
