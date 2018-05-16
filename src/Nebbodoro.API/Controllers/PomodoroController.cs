using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nebbodoro.API.Context;
using Nebbodoro.API.Models;

namespace Nebbodoro.API.Controllers
{
    [Route("api/pomodoros")]
    public class PomodoroController : Controller
    {
        private readonly PomodoroContext _pomodoroContext;

        public PomodoroController(PomodoroContext pomodoroContext)
        {
            _pomodoroContext = pomodoroContext;
        }
        private readonly Microsoft.ApplicationInsights.TelemetryClient _telemetry = new Microsoft.ApplicationInsights.TelemetryClient();

        [Route("")]
        [HttpGet]
        public IActionResult Get()
        {
            var result = _pomodoroContext.Pomodoros
                .Select(p => new {
                    p.Task,
                    p.User.Id,
                    p.User.DisplayName,
                    p.User.Email,
                    p.Start,
                    p.End,
                    p.Duration
                })
                .ToList();

            _telemetry.TrackEvent("Get Pomodoro Data....");

            return Ok(result);
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var result = _pomodoroContext.Pomodoros
                .Where(p => p.User.Id == id)
                .Select(p => new {
                    ID = p.Id,
                    p.Task,
                    UserID = p.User.Id,
                    p.User.DisplayName,
                    p.User.Email,
                    p.Start,
                    p.End,
                    p.Duration
                })
                .ToList();

            return Ok(result);
        }

        [Route("{email}")]
        [HttpGet]
        public IActionResult Get(string email)
        {
            var result = _pomodoroContext.Pomodoros
                .Where(p => p.User.Email == email)
                .Select(p => new {
                    p.Task,
                    p.User.Id,
                    p.User.DisplayName,
                    p.User.Email,
                    p.Start,
                    p.End,
                    p.Duration
                })
                .ToList();

            if (email == "user@nebbiatech.com")
            {
                var ex = new Exception("Bad user data...");
                _telemetry.TrackException(ex);
                throw ex;
            }

            _telemetry.TrackEvent("Get Pomodoro Data for " + email + " ....");
            return Ok(result);
        }

        [Route("")]
        [HttpPost]
        public IActionResult Post([FromBody]Pomodoro pomodoro)
        {
            var user = _pomodoroContext.Users.FirstOrDefault(u => u.Email == pomodoro.User.Email);
            _pomodoroContext.Pomodoros.Add(new Pomodoro
            {
                Task = pomodoro.Task,
                Start = pomodoro.Start,
                End = pomodoro.End,
                Duration = pomodoro.Duration,
                User = user
            });

            _pomodoroContext.SaveChanges();

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var pomodoro = _pomodoroContext.Pomodoros.Find(id);

            if (pomodoro != null)
            {
                _pomodoroContext.Pomodoros.Remove(pomodoro);
                _pomodoroContext.SaveChanges();
            }

            return Ok();
        }
    }
}
