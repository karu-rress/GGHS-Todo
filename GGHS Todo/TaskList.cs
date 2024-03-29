﻿#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GGHS_Todo
{
    using Match = Predicate<TodoTask>;

    [DataContract(Name = "Tasks")]
    public class TaskList : IEnumerable<TodoTask>
    {
        [DataMember]
        public List<TodoTask> List { get; set; } = new();
        private static Stack<List<TodoTask>> TaskStack { get; } = new();

        public TaskList() { }

        public IEnumerator<TodoTask> GetEnumerator() => List.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsNullOrEmpty { get => List is null || !List.Any(); }
        public int Count { get => List.Count; }

        public int CountAll(Match? match) => match is null ? Count : List.FindAll(match).Count;

        /// <summary>
        /// Finds all that matches the Predicate of Task and return as List of Task
        /// </summary>
        /// <param name="match">Expression to find</param>
        /// <returns>List of Tasks. If match is null, then an empty List of Task</returns>
        public List<TodoTask> FindAll(Match? match) => match is null ? new() : List.FindAll(match);

        /// <summary>
        /// Remove all that matches the exprssion. If null is given, same to Clear()
        /// </summary>
        /// <param name="match">Expression to delete</param>
        public void RemoveAll(Match? match)
        {
            if (IsNullOrEmpty)
                return;

            if (match is null)
            {
                TaskStack.Push(new(List));
                List.Clear();
                return;
            }

            var list = FindAll(match);
            TaskStack.Push(list);
            List = List.Except(list).ToList();
        }

        public void Remove(in TodoTask task)
        {
            TaskStack.Push(new() { task });
            List.Remove(task);
        }

        public int Undo()
        {
            if (TaskStack.Count is 0)
                return 0;

            var list = TaskStack.Pop();
            if (list.Count is 0)
                return 0;

            List.AddRange(list);
            return list.Count;
        }

        public void Sort() => List.Sort((x, y) => x.DueDate.CompareTo(y.DueDate));

        public int FindIndex(Match? match) => List.FindIndex(match);

        public TodoTask this[int i] { get => List[i]; set => List[i] = value; }

        public void Add(TodoTask task) => List.Add(task);

    }
}
