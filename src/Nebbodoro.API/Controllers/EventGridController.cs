using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nebbodoro.API.Context;
using Nebbodoro.API.EventGrid;

namespace Nebbodoro.API.Controllers
{
    [Route("api/event-grid")]
    public class EventGridController : Controller
    {
        private readonly EventGridManager _eventGridManager;
        private readonly PomodoroContext _pomodoroContext;

        public EventGridController(EventGridManager eventGridManager, PomodoroContext pomodoroContext)
        {
            _eventGridManager = eventGridManager;
            _pomodoroContext = pomodoroContext;
        }

        [Route("test")]
        [HttpPost]
        public IActionResult PostTest()
        {
            var pomodoro = _pomodoroContext.Pomodoros.First();
            _eventGridManager.OnPomodoroDone(pomodoro);
            return Ok();
        }
    }
}