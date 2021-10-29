using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GGHS_Todo
{
    public sealed partial class DateSelectDialog : ContentDialog
    {
        public DateTime SelectedDate { get; set; }

        public DateSelectDialog()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (datePicker.SelectedDate is null)
            {
                args.Cancel = true;
                textBlock.Visibility = Visibility.Visible;
            }
            else
            {
                var date = datePicker.SelectedDate.Value.DateTime;
                SelectedDate = new(date.Year, date.Month, date.Day);
            }
        }
    }
}
