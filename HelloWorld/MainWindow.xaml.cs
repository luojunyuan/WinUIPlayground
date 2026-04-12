using Microsoft.UI.Xaml;

namespace HelloWorld
{
    public sealed partial class MainWindow : Window
    {
        private int _clickCount;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void HelloButton_Click(object sender, RoutedEventArgs e)
        {
            _clickCount++;
            ClickCountText.Text = $"Clicked {_clickCount} time{(_clickCount == 1 ? "" : "s")}!";
        }
    }
}
