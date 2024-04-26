using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Security.Permissions;

namespace SKA_Forge
{
    /// <summary>
    /// Lógica interna para LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void IntroEnd(object sender, RoutedEventArgs e)
        {
            Intro.Visibility = Visibility.Collapsed;

            DoubleAnimation animation_width = new DoubleAnimation();
            DoubleAnimation animation_height = new DoubleAnimation();
            label.Visibility = Visibility.Visible;
            animation_width.From = bola.Width;
            animation_width.To = 866;
            animation_height.Duration = TimeSpan.FromSeconds(2);
            animation_height.From = bola.Height;
            animation_height.To = 615;
            animation_width.Duration = TimeSpan.FromSeconds(2);
            bola.BeginAnimation(WidthProperty,animation_width);
            bola.BeginAnimation(HeightProperty,animation_height);

           
            DoubleAnimation labelPos = new DoubleAnimation();
                QuadraticEase ease = new QuadraticEase();
                ease.EasingMode = EasingMode.EaseInOut;
                labelPos.EasingFunction = ease;
                labelPos.Duration = TimeSpan.FromSeconds(3);
                labelPos.From = 111;
                labelPos.To = -10;
            labelPos.Completed += exibirBt;

            label.BeginAnimation(Canvas.TopProperty,labelPos);
            

        }

        private void exibirBt(object? sender, EventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = 1;
            anim.Duration = TimeSpan.FromSeconds(1);

            anim.EasingFunction = new QuadraticEase() {EasingMode = EasingMode.EaseOut };
            
          

            btLogin.BeginAnimation(OpacityProperty,anim);
        }

      
        
    }
}
