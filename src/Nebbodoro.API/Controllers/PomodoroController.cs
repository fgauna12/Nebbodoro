using System;
using System.Linq;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Nebbodoro.API.Context;
using Nebbodoro.API.EventGrid;
using Nebbodoro.API.Models;

namespace Nebbodoro.API.Controllers
{   
    [Route("api/pomodoro")]
    public class PomodoroController : Controller
    {
        private readonly PomodoroContext _pomodoroContext;
        private readonly TelemetryClient _telemetryClient;
        private readonly EventGridManager _eventGridManager;

        public PomodoroController(PomodoroContext pomodoroContext, TelemetryClient telemetryClientClient, EventGridManager eventGridManager)
        {
            _pomodoroContext = pomodoroContext;
            _telemetryClient = telemetryClientClient;
            _eventGridManager = eventGridManager;
        }

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

            _telemetryClient.TrackEvent("Get Pomodoro Data....");

            return Ok(result);
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get([FromRoute] int id)
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
        public IActionResult Get([FromRoute] string email)
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
                _telemetryClient.TrackException(ex);
                throw ex;
            }

            _telemetryClient.TrackEvent("Get Pomodoro Data for " + email + " ....");
            return Ok(result);
        }

        [Route("")]
        [HttpPost]
        public IActionResult Post([FromBody]Pomodoro pomodoro)
        {
            var user = _pomodoroContext.Users.FirstOrDefault(u => u.Email == pomodoro.User.Email);

            if (user == null)
                return NotFound("User not found");

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

        [Route("{email}/done")]
        [HttpPost]
        public IActionResult Post([FromRoute] string email)
        {
            var user = _pomodoroContext.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return NotFound();

            var pomodoro = new Pomodoro
            {
                Task = "Some task",
                Start = DateTime.Now,
                End = DateTime.Now,
                Duration = 25,
                User = user
            };

            _pomodoroContext.Pomodoros.Add(pomodoro);

            _pomodoroContext.SaveChanges();

            _eventGridManager.OnPomodoroDone(pomodoro);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete([FromRoute] int id)
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
