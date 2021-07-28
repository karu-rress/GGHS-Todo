#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;

namespace GGHS_Todo
{
    public class TaskButton : Button
    {
        private const int buttonWidth = 2560;
        private const int buttonHeight = 93;

        public Task Task { get; private set; }

        public TaskButton(in Task task, RoutedEventHandler TaskButton_Click, int buttons)
        {
            Task = task;
            Click += TaskButton_Click;
            Height = buttonHeight;
            Margin = new(0, 98 * buttons, 0, 0);
            VerticalAlignment = VerticalAlignment.Top;
            CornerRadius = new(10);

            CreateGrid(out Grid inner, out Grid dday);
            CreateDdayTextBlock(out TextBlock tb1, out TextBlock tb2);
            dday.Children.Add(tb1);
            dday.Children.Add(tb2);
            inner.Children.Add(dday);

            CreateTaskTextBlock(out TextBlock tb3, out TextBlock tb4);
            inner.Children.Add(tb3);
            inner.Children.Add(tb4);

            Content = inner;
        }

        private void CreateGrid(out Grid inner, out Grid dday)
        {
            inner = new()
            {
                Height = 80,
                Width = 2560,
                Margin = new(-12, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            dday = new()
            {
                Width = 65,
                Margin = new(10, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
        }

        private void CreateDdayTextBlock(out TextBlock tb1, out TextBlock tb2)
        {
            bool hasNoDueDay = !Task.DueDate.HasValue;
            tb1 = new()
            {
                FontSize = 19,
                Text = hasNoDueDay ? null : Task.DueDate?.ToString("MM/dd"),
                Margin = new(0, 10, 0, 46),
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = new("Segoe"),
                FontWeight = FontWeights.Bold
            };

            string? text = null;
            if (Task.DueDate is not null)
            {
                text = "D" + (DateTime.Now < Task.DueDate.Value ? "" : "+") + (DateTime.Now - Task.DueDate.Value).Days;
            }

            tb2 = new()
            {
                FontSize = 15,
                Text = text,
                Margin = new(0, 44, 0, 12),
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = new("Consolas"),
                FontWeight = FontWeights.Bold
            };
        }

        private void CreateTaskTextBlock(out TextBlock tb3, out TextBlock tb4)
        {
            tb3 = new()
            {
                FontSize = 17,
                Text = Task.Subject,
                Margin = new(80, 12, 0, 44),
                Width = buttonWidth
            };
            tb4 = new()
            {
                FontSize = 15,
                Text = Task.Title,
                Margin = new(80, 43, 0, 13),
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = buttonWidth,
                Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xA0, 0xA0, 0xA0))
            };
        }
    }
}
