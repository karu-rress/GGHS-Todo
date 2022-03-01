#nullable enable

using System;

namespace GGHS_Todo
{
    public class TodoTask
    {
        public TodoTask() : this(DateTime.Now, "", "", null) { }
        public TodoTask(DateTime dueDate, string subject, string title, string? body)
        {
            DueDate = dueDate;
            Subject = subject;
            Title = title;
            Body = body;
        }

        public DateTime DueDate { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string? Body { get; set; }
    }
}