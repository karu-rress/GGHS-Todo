#nullable enable

using System;
using Windows.UI.Xaml;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Controls.Primitives;

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
        private int buttonWidth => 2560;
        private int buttonHeight => 93;

        public Task Task { get; private set; }

        public TaskButton(in Task task, RoutedEventHandler TaskButton_Click, int buttons)
        {
            Task = task;
            Click += TaskButton_Click;
            RightTapped += TaskButton_RightTapped;
            Height = buttonHeight;
            Margin = new(0, 98 * buttons, 0, 0);
            CornerRadius = new(10);
            VerticalAlignment = VerticalAlignment.Top;
            
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

        private void TaskButton_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (sender is UIElement uiElem)
            {
                MenuFlyout btnFlyOut = new();
                MenuFlyoutItem edit = new() { Text = "Edit", Icon = new SymbolIcon(Symbol.Edit) };
                MenuFlyoutItem delete = new() { Text = "Delete", Icon = new SymbolIcon(Symbol.Delete) };

                edit.Click += (_, e) => {
                    AddPage.Task = Task;
                    if (Window.Current.Content is Frame rootFrame)
                        rootFrame.Navigate(typeof(AddPage), null, new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
                };
                delete.Click += async (_, e) =>
                {
                    await AddPage.DeleteTask(Task.Title, Task);
                    await System.Threading.Tasks.Task.Delay(100);
                    if (Window.Current.Content is Frame rootFrame)
                    {
                        // TODO: 이걸 그냥 MainPage의 Reload Task..?
                        rootFrame.Navigate(typeof(MainPage), null, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
                    }
                };
                
                btnFlyOut.Items.Add(edit);
                btnFlyOut.Items.Add(delete);
                btnFlyOut.Placement = FlyoutPlacementMode.Bottom;
                btnFlyOut.ShowAt(uiElem, e.GetPosition(uiElem));
            }
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
            tb1 = new()
            {
                FontSize = 19,
                Text = Task.DueDate.ToString("MM/dd"),
                Margin = new(0, 10, 0, 46),
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = new("Segoe"),
                FontWeight = FontWeights.Bold
            };

            var now = DateTime.Now;
            int days = (new DateTime(now.Year, now.Month, now.Day) - Task.DueDate).Days;
            string text = "D" + days switch
            {
                0 => "-Day",
                _ => days.ToString("+0;-0"),
            };

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
