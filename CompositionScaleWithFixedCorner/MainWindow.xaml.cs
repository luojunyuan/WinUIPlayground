using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using System;
using System.Numerics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CompositionScaleWithFixedCorner
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private SpriteVisual _spriteVisual;
        private Compositor _compositor;
        private CompositionRoundedRectangleGeometry _clipGeometry;
        private const float CornerRadius = 12f; // 固定圆角半径

        public MainWindow()
        {
            InitializeComponent();
            this.AppWindow.Resize(new(642, 672));

            _compositor = ElementCompositionPreview.GetElementVisual(MyRect).Compositor;

            _spriteVisual = _compositor.CreateSpriteVisual();
            _spriteVisual.Size = new Vector2(100f, 100f);
            _spriteVisual.Offset = new Vector3(0, 0, 0);

            // 关键：给 SpriteVisual 设置画刷（颜色）
            var brush = _compositor.CreateColorBrush(Microsoft.UI.Colors.Blue);
            _spriteVisual.Brush = brush;

            // --------------------------------------------------- clip
            // 创建圆角矩形几何（用于裁剪）
            _clipGeometry = _compositor.CreateRoundedRectangleGeometry();
            _clipGeometry.Size = new Vector2(100f, 100f); // 初始大小与 SpriteVisual 一致
            _clipGeometry.CornerRadius = new Vector2(CornerRadius); // 设置固定圆角半径

            // 创建几何裁剪并应用到 SpriteVisual
            var clip = _compositor.CreateGeometricClip(_clipGeometry);
            _spriteVisual.Clip = clip;

            // --------------------------------------------------- clip

            // 将 SpriteVisual 附加到 MyRect 上
            ElementCompositionPreview.SetElementChildVisual(MyRect, _spriteVisual);

            // 可选：隐藏原来的 Rectangle 填充，只显示 Composition 内容
            MyRect.Fill = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }

        private void StartOffsetAnimation(Vector2 from, Vector2 to)
        {
            var placementVisualOffsetAnimation = _compositor.CreateVector2KeyFrameAnimation();
            placementVisualOffsetAnimation.InsertKeyFrame(0.0f, from);
            placementVisualOffsetAnimation.InsertKeyFrame(1.0f, to);
            placementVisualOffsetAnimation.Duration = TimeSpan.FromSeconds(2);
            _spriteVisual.StartAnimation("Offset.XY", placementVisualOffsetAnimation);
        }
        private void StartSizeAnimation(Vector2 from, Vector2 to)
        {
            // 视觉元素尺寸动画
            var placementVisualSizeAnimation = _compositor.CreateVector2KeyFrameAnimation();
            placementVisualSizeAnimation.InsertKeyFrame(0.0f, from);
            placementVisualSizeAnimation.InsertKeyFrame(1.0f, to);
            placementVisualSizeAnimation.Duration = TimeSpan.FromSeconds(2);
            _spriteVisual.StartAnimation("Size", placementVisualSizeAnimation);

            // Clip 尺寸动画（对应 C++ 中的 Right 和 Bottom 动画）
            // 注意：圆角半径保持不变，我们只动画 Size
            var clipSizeAnimation = _compositor.CreateVector2KeyFrameAnimation();
            clipSizeAnimation.InsertKeyFrame(0.0f, from);
            clipSizeAnimation.InsertKeyFrame(1.0f, to);
            clipSizeAnimation.Duration = TimeSpan.FromSeconds(2);
            _clipGeometry.StartAnimation("Size", clipSizeAnimation);

            // 不需要对 CornerRadius 做动画，它会自动保持固定值
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartSizeAnimation(new Vector2(100f, 100f), new Vector2(200f, 200f));

            StartOffsetAnimation(new Vector2(0f, 0f), new Vector2(150f, 150f));
        }
    }
}
