using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HighScoreAPI.Data
{
    public class HighScore
    {
        [JsonProperty(PropertyName = "highscoreid")]
        [Key]
        public Guid HighScoreId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }

        [Required]
        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }
    }
}
