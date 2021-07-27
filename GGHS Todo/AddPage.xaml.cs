using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GGHS_Todo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddPage : Page
    {
        public AddPage()
        {
            this.InitializeComponent();
        }

        public AddPage(in Task task)
        {
            InitializeComponent();
            mainText.Text = "Modify Task";
            DueDatePicker.Date = task.DueDate.Value;
            SubjectPicker.SelectedItem = task.Subject;
            TitleTextBox.Text = task.Title;
            BodyTextBox.Text = task.Body;
        }

        private void mainText_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = new(DueDatePicker.Date.DateTime, SubjectPicker.SelectedItem as string, TitleTextBox.Text, BodyTextBox.Text);
            MainPage.TaskList.Add(task);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();
        

        private void Close() => Frame.Navigate(typeof(MainPage));
    }
}
