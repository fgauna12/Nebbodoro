using System;
using System.Collections.Generic;
using System.Linq;
using Nebbodoro.API.Models;

namespace Nebbodoro.API.Context
{
    public static class DataSeeder
    {
        public static void AddPomodorosFor(this PomodoroContext context, string displayName, string email, string task)
        {
            if (context.Pomodoros.Any() || context.Users.Any())
                return;

            var user = new User { DisplayName = displayName, Email = email, Password = "P@ssw0rd" };
            var pomodoros = new List<Pomodoro>();

            var today = DateTime.Today;
            var startTime = new DateTime(today.Year, today.Month, today.Day, 9, 0, 0);
            var endTime = startTime.AddMinutes(25);
            var timeOffset = 0.0;

            pomodoros.Add(new Pomodoro
            {
                Task = task,
                Start = startTime.AddMinutes(timeOffset),
                End = endTime.AddMinutes(timeOffset),
                Duration = (startTime.AddMinutes(timeOffset).Subtract(endTime.AddMinutes(timeOffset))).TotalMinutes,
                User = user
            });

            timeOffset += 5.0;
            timeOffset += 25.0;
            pomodoros.Add(new Pomodoro
            {
                Task = task,
                Start = startTime.AddMinutes(timeOffset),
                End = endTime.AddMinutes(timeOffset),
                Duration = (startTime.AddMinutes(timeOffset).Subtract(endTime.AddMinutes(timeOffset))).TotalMinutes,
                User = user
            });

            timeOffset += 5.0;
            timeOffset += 25.0;
            pomodoros.Add(new Pomodoro
            {
                Task = task,
                Start = startTime.AddMinutes(timeOffset),
                End = endTime.AddMinutes(timeOffset),
                Duration = (startTime.AddMinutes(timeOffset).Subtract(endTime.AddMinutes(timeOffset))).TotalMinutes,
                User = user
            });

            context.Pomodoros.AddRange(pomodoros);
            context.SaveChanges();
        }
    }
}