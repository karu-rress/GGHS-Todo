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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
// Enables using record types as tuple-like types.
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class IsExternalInit { }
}
namespace GGHS_Todo
{
    public class TaskButton : Button
    {
        const int buttonWidth = 2560;
        const int buttonHeight = 93;

        public Task Task { get; private set; }


        public TaskButton(in Task task, RoutedEventHandler TaskButton_Click, int buttons)
        {
            Task = task;
            Click += TaskButton_Click;
            Height = buttonHeight;
            Margin = new(0, 98 * buttons, 0, 0);
            VerticalAlignment = VerticalAlignment.Top;

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
                Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xA4, 0xA4, 0xA4))
            };
        }

        private void CreateDdayTextBlock(out TextBlock tb1, out TextBlock tb2)
        {
            tb1 = new()
            {
                FontSize = 19,
                Text = Task.DueDate?.ToString("MM/DD"),
                Margin = new(0, 10, 0, 46),
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = new("Segoe"),
                FontWeight = FontWeights.Bold
            };
            tb2 = new()
            {
                FontSize = 15,
                Text = "D" + (DateTime.Now < Task.DueDate.Value ? "" : "+") + (DateTime.Now - Task.DueDate.Value).Days,
                Margin = new(0, 44, 0, 12),
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = new("Consolas"),
                FontWeight = FontWeights.Bold
            };
        }

        void CreateGrid(out Grid inner, out Grid dday)
        {
            inner = new()
            {
                Height = 80,
                Width = 2560,
                Margin = new(-12, 0, 0, 0)
            };
            dday = new()
            {
                Width = 65,
                Margin = new(10, 0, 0, 0),
            };
        }

    }


    public record Task(DateTime? DueDate, string Subject, string Title, string? Body);
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static List<Task> TaskList { get; set; } = new()
        {
            new Task(new DateTime(2021, 08, 16), "독서", "와 재밌당", "허허"),
            new Task(new DateTime(2021, 08, 17), "심화영어", "와 싸발적", "호호"),
        };
        public const int ButtonWidth = 2560;
        public const int ButtonHeight = 93;

        public MainPage()
        {
            InitializeComponent();
            LoadTasks();
        }

        /*
         private void Button_Click(object sender, RoutedEventArgs e)
        {

            //Create the button
            Button b = new Button();
            b.Height = 30;
            b.Width = 100;
            b.VerticalAlignment = VerticalAlignment.Top;
            b.HorizontalAlignment = HorizontalAlignment.Left;
            b.Margin = new Thickness(20, 20, 0, 0);
            b.Content = "Button " + buttonCounter;
            b.Click += Button_Click;

            //Calculate the place of the button
            int column = (int)(buttonCounter / 4);
            int row = buttonCounter % 4;

            //Check if you need to add a columns
            if(row == 0)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(column, GridUnitType.Auto);
                myGrid.ColumnDefinitions.Add(col);
            }

            //Add the button
            myGrid.Children.Add(b);
            Grid.SetColumn(b, column);
            Grid.SetRow(b, row);
            buttonCounter++;
        }
         
         */

        /*
<Button Click="TaskButton_Click">
    <Grid Height="80" Width="2560" Margin="-12,0,0,0" >
        <Grid>
            <TextBlock>
            <TextBlock>
        </Grid>
        <TextBlock FontSize="17" Text="독서" Margin="80,12,0,44" Width="2560"/>
        <TextBlock FontSize="15" Text="매 시간이 수행평가" Margin="80,43,0,13" HorizontalAlignment="Left" Width="2560" Foreground="#ffa4a4a4"/>
    </Grid>
</Button>
         
         */
        private void LoadTasks()
        {
            if (TaskList is null || TaskList.Count is 0)
            {
                TaskGrid.Children.Clear();
                return;
            }

            int buttons = 0;
            foreach (var task in TaskList)
            {

                /*
                Button b = new() { Height = ButtonHeight, Margin = new(0, 98 * buttons, 0, 0) };
                b.Click += TaskButton_Click;

                    Grid inner = new()
                    {
                        Height = 80,
                        Width = 2560,
                        Margin = new(-12, 0, 0, 0)
                    };
                    Grid dday = new()
                    { 
                        Width = 65,
                        Margin = new(10, 0, 0, 0),
                    };
                    TextBlock tb1 = new()
                    {
                        FontSize = 19,
                        Text = task.DueDate?.ToString("MM/dd"),
                        Margin = new(0, 10, 0, 46),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontFamily = new("Segoe"),
                        FontWeight = FontWeights.Bold
                    };
                    TextBlock tb2 = new()
                    { 
                        FontSize = 15,
                        Text = "D" + (DateTime.Now < task.DueDate.Value ? "" : "+") + (DateTime.Now - task.DueDate.Value).Days,
                        Margin = new(0, 44, 0, 12),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontFamily = new("Consolas"),
                        FontWeight = FontWeights.Bold
                    };
                    dday.Children.Add(tb1);
                    dday.Children.Add(tb2);

                    TextBlock tb3 = new()
                    { 
                        FontSize = 17,
                        Text = task.Subject,
                        Margin = new(80, 12, 0, 44),
                        Width = ButtonWidth
                    };
                    TextBlock tb4 = new()
                    {
                        FontSize = 15,
                        Text = task.Title,
                        Margin = new(80, 43, 0, 13),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Width = ButtonWidth,
                        Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xA4, 0xA4, 0xA4))
                    };
                    inner.Children.Add(dday);
                    inner.Children.Add(tb3);
                    inner.Children.Add(tb4);
                b.Content = inner;
                TaskGrid.Children.Add(b);

                buttons++;*/


                TaskGrid.Children.Add(new TaskButton(task, TaskButton_Click, buttons++));
            }
        }

        private void AddButton_Click(object _, RoutedEventArgs e) => Frame.Navigate(typeof(AddPage));

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TaskButton tb)
                Frame.Navigate(typeof(AddPage), tb.Task);
        }
    }
}

// lrbyskojsnuuitgr