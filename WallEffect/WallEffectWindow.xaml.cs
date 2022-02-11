using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WPFWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly double _screenWidthDiv2 = SystemParameters.PrimaryScreenWidth / 2 + 4;
        private readonly DoubleAnimation _expandAnimation;
        private readonly DoubleAnimation _shrinkAnimation;
        private readonly ColorAnimation _colorAnimation;
        
        public void InAnimation(Action callback)
        {
            var expandAnimationA = _expandAnimation.Clone();
            Storyboard.SetTarget(expandAnimationA, leftRec);
            Storyboard.SetTargetProperty(expandAnimationA, new PropertyPath(WidthProperty));

            Storyboard.SetTarget(_expandAnimation, rightRec);
            Storyboard.SetTargetProperty(_expandAnimation, new PropertyPath(WidthProperty));

            var brush = new SolidColorBrush();
            leftRec.Fill = brush;
            rightRec.Fill = brush;
            RegisterName("brush", brush);
            Storyboard.SetTargetName(_colorAnimation, "brush");
            Storyboard.SetTargetProperty(_colorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            var storyboard = new Storyboard();
            storyboard.Children.Add(expandAnimationA);
            storyboard.Children.Add(_expandAnimation);
            storyboard.Children.Add(_colorAnimation);
            storyboard.Completed += (s, e) => { callback(); };
            storyboard.Begin(this);
        }

        public void OutAnimation()
        {
            var shrinkAnimationA = _shrinkAnimation.Clone();
            Storyboard.SetTarget(shrinkAnimationA, leftRec);
            Storyboard.SetTargetProperty(shrinkAnimationA, new PropertyPath(WidthProperty));

            var shrinkAnimationB = _shrinkAnimation;
            Storyboard.SetTarget(shrinkAnimationB, rightRec);
            Storyboard.SetTargetProperty(shrinkAnimationB, new PropertyPath(WidthProperty));

            var storyboard = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromSeconds(6))
            };
            storyboard.Children.Add(shrinkAnimationA);
            storyboard.Children.Add(shrinkAnimationB);
            storyboard.Completed += (s, e) => { Dispatcher.InvokeShutdown(); };
            storyboard.Begin();
        }

        public void BlackOut()
        {
            leftRec.Width = _screenWidthDiv2 * 2;
            leftRec.Fill = new SolidColorBrush(Colors.Black);
        }

        public void OutAnim(Action callback)
        {
            var anim = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(3)),
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };

            DoubleAnimation animA = anim.Clone();
            Storyboard.SetTarget(animA, leftRec);
            Storyboard.SetTargetProperty(animA, new PropertyPath(OpacityProperty));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animA);
            storyboard.Completed += (s, e) => { callback(); };
            storyboard.Begin(this);

        }

        public MainWindow()
        {
            ResizeMode = ResizeMode.NoResize;
            WindowState = WindowState.Maximized;
            InitializeComponent();
            _expandAnimation = new DoubleAnimation
            {
                From = 0,
                To = _screenWidthDiv2,
                Duration = new Duration(TimeSpan.FromSeconds(3)),
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut },
            };

            _shrinkAnimation = new DoubleAnimation
            {
                From = _screenWidthDiv2 * 1.5,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };

            _colorAnimation = new ColorAnimation
            {
                From = Colors.DimGray,
                To = Colors.Black,
                Duration = new Duration(TimeSpan.FromSeconds(3)),
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };
        }


    }
}
