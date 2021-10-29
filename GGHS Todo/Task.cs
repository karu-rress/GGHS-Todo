#nullable enable

using System;

namespace GGHS_Todo
{
    public class Task
    {
        public Task() : this(DateTime.Now, "", "", default) { }
        public Task(in DateTime date, string subject, string title, string? body)
        {
            DueDate = date;
            Subject = subject;
            Title = title;
            Body = body;
        }

        public DateTime DueDate { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string? Body { get; set; }
    }

    /*
public record Task(DateTime DueDate, string Subject, string Title, string? Body)
{
    public Task() : this(DateTime.Now, "", "", null) { }
}*/
}