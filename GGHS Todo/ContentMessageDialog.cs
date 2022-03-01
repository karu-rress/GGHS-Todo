using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GGHS_Todo
{
    public class ContentMessageDialog : ContentDialog
    {
        public ContentMessageDialog(string body, string title, string buttonText = "OK")
        {
            Content = body;
            Title = title;
            CloseButtonText = buttonText;
            CornerRadius = new(10);
        }
    }
}
