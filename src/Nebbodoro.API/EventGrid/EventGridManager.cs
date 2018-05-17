using System;
using System.Net;
using System.Threading;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using Nebbodoro.API.Models;

namespace Nebbodoro.API.EventGrid
{
    public class EventGridManager
    {
        private readonly IOptions<EventGridOptions> _options;

        public EventGridManager(IOptions<EventGridOptions> options)
        {
            _options = options;
        }

        public void OnPomodoroDone(Pomodoro pomodoro)
        {
            var @event = new EventGridEvent()
            {
                Id = Guid.NewGuid().ToString(),
                Data = pomodoro,
                EventTime = DateTime.Now,
                EventType = "Nebbodoro.OnPomodoroDone",
                Subject = "OnPomodoroDone",
                DataVersion = "1.0"
            };

            if (_options.Value?.TopicEndpoint == null || _options.Value?.TopicKey == null)
                return;

            string topicEndpoint = _options.Value.TopicEndpoint;
            string topicKey = _options.Value.TopicKey;
            string topicHostname = new Uri(topicEndpoint).Host;

            TopicCredentials topicCredentials = new TopicCredentials(topicKey);
            EventGridClient client = new EventGridClient(topicCredentials);

            client.PublishEventsAsync(topicHostname, new[] {@event}, CancellationToken.None).Wait();
        }
    }
}