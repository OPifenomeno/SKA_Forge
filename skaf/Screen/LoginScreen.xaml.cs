using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using Microsoft.Identity.Client;
namespace skaf
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

        private void Load(object sender, RoutedEventArgs e)
        {
            
            ska_logo.BeginAnimation(OpacityProperty, new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1)
            });


            bolaBack.BeginAnimation(WidthProperty, new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(2),
                From = 0,
                To = 848,
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut }
            });



          
                bolaBack.BeginAnimation(HeightProperty, new DoubleAnimation()
                {
                    Duration = TimeSpan.FromSeconds(3),
                    From = 0,
                    To = 521,
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut }

                });
            DoubleAnimation joi = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(3),
                From = 126,
                To = 20,
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
                
            };

            joi.Completed += (sender, e) => {

                bt_login.BeginAnimation(OpacityProperty, new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseIn }
                });
            };

            ska_logo.BeginAnimation(TopProperty, joi);
          
        }

        private void Animar(object sebder, RoutedEventArgs e) {
            Ellipse[] ell = graphs.Children.OfType<Ellipse>().ToArray();


            int i = 0;
            foreach(Ellipse ellipse in ell)
            {
                i++;
                ellipse.BeginAnimation(TopProperty, new DoubleAnimation()
                {
                    
                    From = Canvas.GetTop(ellipse),
                    To = Canvas.GetTop(ellipse)-120 - i,
                    Duration = TimeSpan.FromSeconds(2+i),
                    EasingFunction = new QuadraticEase() {EasingMode = EasingMode.EaseOut}
                });

            }
        
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private async void Logar(object sender, RoutedEventArgs e)
        {
            await Login();
        }

        public async Task Login() {

            var cca = PublicClientApplicationBuilder.Create("1a6e7e51-b9a3-4036-ba0a-12560ce5fd47")
                .WithDefaultRedirectUri().Build();
            var scopes = new[] { "https://graph.microsoft.com/User.Read" };
            try {
                var result = await cca.AcquireTokenInteractive(scopes).ExecuteAsync();
                var accessToken = result.AccessToken;
                new JanelaPrincipal().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                Clipboard.SetText(ex.Message);
                MessageBox.Show("Falha ao executar Login!: \n"+ex.Message);
            }
           
            

           
        }
    }
   
}
