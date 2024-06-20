
using skaf.Screen;
using Squirrel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;


namespace skaf
{
    /// <summary>
    /// Lógica interna para JanelaPrincipal.xaml
    /// </summary>
    public partial class JanelaPrincipal : Window
    {
        UpdateManager manager;
        public JanelaPrincipal()

        {
           
            InitializeComponent();
           
            carregarModelos();
          // try { VerificarAtt(); } catch (Exception es){ MessageBox.Show(es.Message); }
        }

        private async void Atualizar()
        {
            
            await manager.UpdateApp();
            MessageBox.Show("Reinicie o app!");
            this.Close();
        }

        private async void VerificarAtt()
        {
            manager = await UpdateManager.GitHubUpdateManager(@"https://github.com/OPifenomeno/SKA_Forge");


            var updInfo = await manager.CheckForUpdate();
            if (updInfo.ReleasesToApply.Count > 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Há atualizações disponíveis. Deseja aplicá-las?", "Atualização Disponível", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {

                    Atualizar();

                }


            }
        }






        void carregarModelos() {
            conteiner.Children.Clear();
            DirectoryInfo past = new DirectoryInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Emails"));
            if (past.Exists)
            {
                past.GetDirectories().ToList().ForEach(dir =>
                {
                    if (dir.Name != "Anexos") { 
                    EmailGroupBox gp = new EmailGroupBox(dir.Name, this);
                    Canvas.SetZIndex(gp, 4);
                    DockPanel.SetDock(gp, Dock.Top);
                    conteiner.Children.Add(gp);
                    }
                });

            }
            else {
                past.Create();
                carregarModelos();
            }
                }


        private void drag(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }



        private void Adicionar_Modelo(object sender, RoutedEventArgs e)
        {
            CriarModelo novoM = new CriarModelo(this);
            novoM.Owner = this;
            novoM.Show();

        }
        public void MostrarModelo(EmailGroupBox element){
            element.VerticalAlignment = VerticalAlignment.Top;
        conteiner.Children.Add(element);
        }

        

        

        private void MostrarPerfil(object sender, RoutedEventArgs e)
        {
            Screen.Config conf = new();
            conf.ShowDialog();
            userName.Content = LoginScreen.usuario.Name??"User";
        }

        private void AbrirMenu(object sender, RoutedEventArgs e)
        {
            if (LoginScreen.usuario != null)
            {
                userName.Content = LoginScreen.usuario.Name;
            }
            if (MenuBar.Width != MenuBar.MaxWidth)
            {
                MenuBar.BeginAnimation(WidthProperty, new DoubleAnimation
                {
                    From = 0,
                    To = MenuBar.MaxWidth,
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }

                });

            }
            else
            {
                MenuBar.BeginAnimation(WidthProperty, new DoubleAnimation
                {
                    From = MenuBar.MaxWidth,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
                });
            }
        }
    }
}
