using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GGHS_Todo
{
    public sealed partial class SubjectSelectDialog : ContentDialog
    {
        public string SelectedSubject { get; set; }
        public List<string> Subject { get; }
        public SubjectSelectDialog()
        {
            InitializeComponent();
            Subject = AddPage.Grade1.Concat(AddPage.Grade2).Concat(AddPage.Grade3).Distinct().ToList();
            Subject.Add("기타");
            SubjectComboBox.ItemsSource = Subject;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (SubjectComboBox.SelectedIndex is -1)
            {
                args.Cancel = true;
                TextBlock.Visibility = Visibility.Visible;
            }
            if (SubjectComboBox.SelectedItem is string s)
            {
                SelectedSubject = s;
            }
        }

    }
}
