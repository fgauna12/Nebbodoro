using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Nebbodoro.API.Models;

namespace Nebbodoro.API.Context
{
    public class PomodoroContext : DbContext
    {
        public PomodoroContext(DbContextOptions<PomodoroContext> options) : base(options)
        {}

        public DbSet<Pomodoro> Pomodoros { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
        }
    }
}