﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Azure.Identity;
using Squirrel;
using System.Net.Mail;
using System.Net;


namespace skaf
{
    /// <summary>
    /// Lógica interna para LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
       

        public  static skaf.res.User ?usuario;
        public static GraphServiceClient graphClient;
        public LoginScreen()
        {
            InitializeComponent();
            try { VerificarAtt(); } catch { }
        }

        private async void Atualizar()
        {
            new skaf.Screen.Update().Show();
            this.Visibility=0;
            await manager.UpdateApp();
            MessageBox.Show("Reinicie o App");
            System.Diagnostics.Process.Start("skaf");
            skaf.App.Current.Shutdown();
           
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
            DoubleAnimation joi = new()
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
            await SilentLogin();

        }
        public async Task SilentLogin() {
        var cca = PublicClientApplicationBuilder.Create("1a6e7e51-b9a3-4036-ba0a-12560ce5fd47")
                .WithDefaultRedirectUri()
                .Build();
            
            var scopes = new[] { "https://graph.microsoft.com/User.Read" };
            var acc = await cca.GetAccountsAsync();
            
            try {
               
                var result = await cca.AcquireTokenSilent(scopes,acc.FirstOrDefault()).ExecuteAsync();
                Properties.Settings.Default.Nome = Properties.Settings.Default.Nome ?? result.Account.Username;
                Properties.Settings.Default.token = result.AccessToken;
                Properties.Settings.Default.Save();
                usuario = new skaf.res.User(result.Account.Username, result.AccessToken);


            } catch (Exception log) {
              
                await Login(); }
        }

        public async Task Login() {

            var cca = PublicClientApplicationBuilder.Create("1a6e7e51-b9a3-4036-ba0a-12560ce5fd47")
                .WithDefaultRedirectUri().Build();


            var ss = new[] { "User.Read" };
            var options = new DeviceCodeCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                ClientId = "1a6e7e51-b9a3-4036-ba0a-12560ce5fd47",
                TenantId = "5f2bb378-929d-431f-8874-fe83e34bec89",
               
                DeviceCodeCallback = (code, cancellation) =>
                {
                    Console.WriteLine(code.Message);
                    return Task.FromResult(0);
                },
            };
            var deviceCode = new DeviceCodeCredential(options);
            graphClient  = new GraphServiceClient(deviceCode,ss);

            var scopes = new[] { "https://graph.microsoft.com/User.Read" };
            try {
                var result = await cca.AcquireTokenInteractive(scopes).ExecuteAsync();
                

                Properties.Settings.Default.Nome = Properties.Settings.Default.Nome??result.Account.Username;
                Properties.Settings.Default.Email = result.Account.Username;
                Properties.Settings.Default.token = result.AccessToken;
                Properties.Settings.Default.Save();
                usuario = new skaf.res.User(result.Account.Username,result.AccessToken);

                
                await relatarUsuario((result.Account.Username + Properties.Settings.Default.Nome));
                new JanelaPrincipal().Show();

                this.Close();
                relatarUsuario(result.Account.Username);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("User canceled")) {

                }
                else
                {
                    System.Windows.MessageBox.Show("Falha ao executar Login!: \n" + ex.Message + "\n\n" + ex.StackTrace);
                }
                
            }
           
            

           
        }

            async Task relatarUsuario(string user) {

            MailMessage message = new MailMessage();
            message.From = new MailAddress("pijrjava@gmail.com");
            message.Subject = "Novo acesso em SKAForge";
            message.To.Add("emanuel.junior@ska.com.br");
            message.Body = $"Olá senhor todo poderoso\nUm usuário logou em SKAMail.\n" +
                $"Quem logou: {user}\n" +
                $"\n Att,\n\nPi Java\n Contribuindo para um melhor monitoramento do SKAf.";
                


            using (SmtpClient sm = new SmtpClient("smtp.gmail.com",587)) {
                sm.UseDefaultCredentials = false;
                sm.Credentials = new NetworkCredential("pijrjava@gmail.com", "rfab sngs egrg eidt");
                sm.EnableSsl = true;
                sm.Send(message);
            }

        }
    }
   
}
