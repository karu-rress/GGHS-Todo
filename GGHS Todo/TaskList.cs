#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGHS_Todo
{
    using Match = Predicate<Task>;
    public class TaskList : IEnumerable<Task>
    {
        private List<Task> taskList = new();
        public List<Task> List { get => taskList; }
        private Stack<List<Task>> taskStack = new();

        public TaskList()
        {
        }

        public IEnumerator<Task> GetEnumerator() => taskList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsNullOrEmpty { get => taskList is null || !taskList.Any(); }
        public int Count { get => taskList.Count; }

        public int CountAll(Match? match) => match is null ? Count : taskList.FindAll(match).Count;

        /// <summary>
        /// Finds all that matches the Predicate of Task and return as List of Task
        /// </summary>
        /// <param name="match">Expression to find</param>
        /// <returns>List of Tasks. If match is null, then an empty List of Task</returns>
        public List<Task> FindAll(Match? match) => match is null ? new List<Task>() : taskList.FindAll(match);

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
                taskStack.Push(new(taskList));
                taskList.Clear();
                return;
            }

            Contract.Requires<ArgumentException>(FindAll(match) is not null);

            var list = FindAll(match);
            taskStack.Push(list);
            taskList = taskList.Except(list).ToList();
        }

        public void Remove(in Task task)
        {
            taskStack.Push(new() { task });
            taskList.Remove(task);
        }

        public int Undo()
        {
            if (taskStack.Count is 0)
                return 0;

            var list = taskStack.Pop();
            if (list.Count is 0)
                return 0;
            taskList.AddRange(list);
            Sort();
            return list.Count;
        }

        public void Sort() => taskList.Sort((x, y) => x.DueDate.Value.CompareTo(y.DueDate.Value));

        public int FindIndex(Match? match) => taskList.FindIndex(match);

        public Task this[int i] { get => taskList[i]; set => taskList[i] = value; }

        public void Add(Task task) => taskList.Add(task);

    }
}
