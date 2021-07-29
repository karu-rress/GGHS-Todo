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
    /*
     <Button Click="TaskButton_Click" CornerRadius="10">
                    <Grid>
                        <Grid Height="80" Width="2560" Margin="-12,0,0,0" >
                            <Grid Width="65" Margin="10,0,0,0" HorizontalAlignment="Left">
                                <TextBlock FontSize="19" Text="07/29" Margin="0,10,0,46" HorizontalAlignment="Center" FontFamily="Segoe" FontWeight="Bold"/>
                                <TextBlock FontSize="15" Text="D-29" Margin="0,44,0,12" HorizontalAlignment="Center" FontFamily="Consolas" FontWeight="Bold"/>
                            </Grid>
                            <TextBlock FontSize="17" Text="독서" Margin="80,12,0,44" Width="2560"/>
                            <TextBlock FontSize="15" Text="매 시간이 수행평가" Margin="80,43,0,13" HorizontalAlignment="Left" Width="2560" Foreground="#ffa4a4a4"/>
                        </Grid>
                        <TextBlock Text="&#xE76C;" FontFamily="Segoe MDL2 Assets" Width="17" VerticalAlignment="Center"  FontSize="17" Foreground="#ff727272" Margin="0,0,10,0" HorizontalAlignment="Right"/>
                    </Grid>
                </Button>
     
     */
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

            CreateGrid(out Grid inner, out Grid dday, out Grid outter);
            CreateDdayTextBlock(out TextBlock tb1, out TextBlock tb2);
            dday.Children.Add(tb1);
            dday.Children.Add(tb2);
            inner.Children.Add(dday);

            CreateTaskTextBlock(out TextBlock tb3, out TextBlock tb4);
            inner.Children.Add(tb3);
            inner.Children.Add(tb4);

            CreateArrowTextBlock(out TextBlock arrow);
            outter.Children.Add(inner);
            outter.Children.Add(arrow);

            Content = outter;
        }

        private void CreateArrowTextBlock(out TextBlock arrow)
        {
            arrow = new()
            {
                Text = "\xE76C",
                FontFamily = new("Segoe MDL2 Assets"),
                FontSize = 17,
                Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x72, 0x72, 0x72)),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new(0, 0, 10, 0)
            };
        }

        private void CreateGrid(out Grid inner, out Grid dday, out Grid outter)
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
            outter = new();
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
                // D0 & D-Day 문제점 수정
                //
                // TimeSpan.Day에서 +1
                // if문 하나 추가해서 D is 0일때 D-Day로
                // 아니면 Three-way comparsion switch expr로
                // -1, 0, +1일때 각각 표시
                // 
                int days = (DateTime.Now - Task.DueDate.Value).Days;
                days = days switch
                {
                    < 0 => days - 1,
                    _ => days,
                };


                text = "D" + days switch
                {
                    0 => "-Day",
                    _ => days.ToString("+0;-0"),
                };
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
