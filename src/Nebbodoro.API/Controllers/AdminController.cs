using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nebbodoro.API.Context;
using Nebbodoro.API.Models;

namespace Nebbodoro.API.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly PomodoroContext _pomodoroContext;

        public AdminController(PomodoroContext pomodoroContext)
        {
            _pomodoroContext = pomodoroContext;
        }

        [Route("migrations")]
        [HttpPost]
        public IActionResult ApplyMigrations()
        {
            _pomodoroContext.Database.Migrate();

            _pomodoroContext.AddPomodorosFor("Phil", "phil@nebbiatech.com", "Coding");
            _pomodoroContext.AddPomodorosFor("Facundo", "facundo@nebbiatech.com", "Research");
            _pomodoroContext.AddPomodorosFor("Jared", "jared@nebbiatech.com", "Blogging");
            _pomodoroContext.AddPomodorosFor("Esteban", "esteban@nebbiatech.com", "RoadShow");

            return Ok();
        }
    }
}