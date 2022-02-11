using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Media.Animation;

namespace WPFWindow
{
    /// <summary>
    /// Interaction logic for MovingMsgBox.xaml
    /// </summary>
    public partial class MovingMsgBox : Window
    {
        public MovingMsgBox(string oriPath, string destFolderPath, ImageSource oriSource, ImageSource destSource)
        {
            InitializeComponent();
            OriImage.Source = oriSource;
            DestImage.Source = destSource;
            var oriName = Path.GetFileName(oriPath);
            var destName = Path.GetFileName(destFolderPath);
            DestLabel.Content = destName.Length > 9 ? destName[..9] + "..." : destName;
            OriLabel.Content = oriName.Length > 10 ? oriName[..9] + "..." : oriName;
        }

        public void ShowMovingAnim()
        {
            var tnLeft = ArrowImage.Margin;
            var tnMiddle = ArrowImage.Margin;
            var tnRight = ArrowImage.Margin;
            
            tnLeft.Left -= 32;
            tnRight.Left += 32;

            var anim = new ThicknessAnimationUsingKeyFrames();
            anim.KeyFrames.Add(new DiscreteThicknessKeyFrame(tnLeft, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
            anim.KeyFrames.Add(new DiscreteThicknessKeyFrame(tnMiddle, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5))));
            anim.KeyFrames.Add(new DiscreteThicknessKeyFrame(tnRight, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1))));

            var storyboard = new Storyboard
            {
                Duration = TimeSpan.FromSeconds(1.5), RepeatBehavior = RepeatBehavior.Forever,
            };
            Storyboard.SetTarget(anim, Rect);
            Storyboard.SetTargetProperty(anim, new PropertyPath(MarginProperty));

            storyboard.Children.Add(anim);
            storyboard.Begin();
        }

        private void Window_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Close();
        }
    }
}