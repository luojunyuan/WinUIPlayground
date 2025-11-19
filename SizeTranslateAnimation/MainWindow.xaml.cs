using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SizeTranslateAnimation
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.AppWindow.Resize(new(642, 672));

            Storyboard.SetTarget(_xAnimation, RectTransform);
            Storyboard.SetTargetProperty(_xAnimation, nameof(TranslateTransform.X));
            sb.Children.Add(_xAnimation);

            Storyboard.SetTarget(_yAnimation, RectTransform);
            Storyboard.SetTargetProperty(_yAnimation, nameof(TranslateTransform.Y));
            sb.Children.Add(_yAnimation);

            Storyboard.SetTarget(scaleXAnimation, ScaleAni);
            Storyboard.SetTarget(scaleYAnimation, ScaleAni);
            Storyboard.SetTargetProperty(scaleXAnimation, nameof(ScaleTransform.ScaleX));
            Storyboard.SetTargetProperty(scaleYAnimation, nameof(ScaleTransform.ScaleY));
            sb.Children.Add(scaleXAnimation);
            sb.Children.Add(scaleYAnimation);
        }

        Storyboard sb = new Storyboard();
        static Duration RectDuration = new Duration(TimeSpan.FromMilliseconds(500));
        static int GoTo = 150;

        private DoubleAnimation _xAnimation = new DoubleAnimation()
        {
            From = 0,
            To = GoTo,
            Duration = RectDuration,
        };
        private DoubleAnimation _yAnimation = new DoubleAnimation()
        {
            From = 0,
            To = GoTo,
            Duration = RectDuration,
        };

        DoubleAnimation scaleXAnimation = new DoubleAnimation
        {
            From = 1.0,
            To = 2.0,
            Duration = RectDuration,
        };
        DoubleAnimation scaleYAnimation = new DoubleAnimation
        {
            From = 1.0,
            To = 2.0,
            Duration = RectDuration,
        };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sb.Begin();
        }
    }
}
