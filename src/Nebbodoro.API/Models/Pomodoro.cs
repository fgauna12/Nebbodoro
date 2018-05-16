using System;
using System.ComponentModel.DataAnnotations;

namespace Nebbodoro.API.Models
{
    public class Pomodoro
    {
        [Key]
        public int Id { get; set; }
        public string Task { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Duration { get; set; }
        public User User { get; set; }
    }
}