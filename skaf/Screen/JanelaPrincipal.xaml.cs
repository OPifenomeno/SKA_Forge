
using NuGet;
using skaf.Screen;
using Squirrel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
        UpdateManager ?manager;
        public JanelaPrincipal()

        {
           
            InitializeComponent();
            novidades();
            carregarModelos();
           try { VerificarAtt();} catch {  }
        }

        private async void Atualizar()
        {
            await manager.UpdateApp();
            MessageBox.Show("Reinicie o app!");
            Application.Current.Shutdown();
            Process.Start(Assembly.GetExecutingAssembly().FullName);
        }

        private async void VerificarAtt()
        {
            try
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
                    else {
                        MessageBox.Show("Você já tem a última versão!");
                    }


                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }




        void novidades() {
            
            MessageBox.Show("Uma revisão foi feita no app! O que mudou?\n\n" +
                "-Correção de bugs;\n" +
                "-Adição de emails que faltavam;\n" +
                "ATENÇÃO:\n"+"1.Caso haja problema com os caminhos de e-mail, vá em configurações -> sair, e em seguida faça login novamente. \n"+
                "2. Se você perdeu as modificações/modelos que criou, por favor, informe em (emanuel.junior@ska.com.br)");



        }

        void carregarModelos() {
            
                

            conteiner.Children.Clear();
            DirectoryInfo past = new DirectoryInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Emails"));
            if (past.Exists)
            {
                if (Properties.Settings.Default.primeiroLogin)
                {
                    DirectoryInfo a = new DirectoryInfo(Path.Combine(past.FullName,"Anexos"));
                    DirectoryInfo p = new DirectoryInfo(Path.Combine(past.FullName, "Admissões - automação"));
                    DirectoryInfo p1 = new DirectoryInfo(Path.Combine(past.FullName, "Admissões - sistemas"));
                    
                    
                  
                   

                    if (!a.Exists || Properties.Settings.Default.primeiroLogin)
                    {
                        a.Create();
                        File.WriteAllBytes(Path.Combine(a.FullName, "Unimed Fesp Nacional - Apresentação.doc"), Properties.Resource1.Unimed_Fesp_Nacional___Apresentação);
                        File.WriteAllBytes(Path.Combine(a.FullName, "Termo de Opção - Dental SKA.pdf"), Properties.Resource1.Termo_de_Opção___Dental_SKA);
                        File.WriteAllBytes(Path.Combine(a.FullName, "planilha para inclusões- TIPAN.xls"), Properties.Resource1.planilha_para_inclusões__TIPAN);
                        File.WriteAllBytes(Path.Combine(a.FullName, "Lista de procedimentos - Odonto.pdf"), Properties.Resource1.Lista_de_procedimentos___Odonto);
                        File.WriteAllBytes(Path.Combine(a.FullName, "Formulário Designação de Beneficiários.pdf"), Properties.Resource1.Formulário_Designação_de_Beneficiários);
                    }
                    string[] paths = {
                        Path.Combine(p.FullName, "Seguro de Vida.txt"),
                        Path.Combine(p.FullName, "Plano Unimed.txt"),
                        Path.Combine(p.FullName, "Plano Odontologico.txt")
                    };
                    string[] att = {
                   $"Attachment:{Path.Combine(a.FullName, "Formulário Designação de Beneficiários.pdf")};",

                   $"Attachment:{Path.Combine(a.FullName, "planilha para inclusões- TIPAN.xls")};" +
                   $" \nAttachment:{Path.Combine(a.FullName, "Unimed Fesp Nacional - Apresentação.doc")};\n",

                   $"Attachment:{Path.Combine(a.FullName, "Termo de Opção - Dental SKA.pdf")};\r\n" +
                   $"Attachment:{Path.Combine(a.FullName, "Lista de procedimentos - Odonto.pdf")};\r\n" +
                   $"Attachment:{Path.Combine(a.FullName, "Unimed Fesp Nacional - Apresentação.doc")};\r\n"


                    };


                    if (!p.Exists || Properties.Settings.Default.primeiroLogin)
                    {
                        p.Create();
                    
                    //cria arquivo dos e-mails
                    File.WriteAllText(p + "/Gympass.txt", Properties.Resource1.Gympass);
                        File.WriteAllText(Path.Combine(p.FullName, "Vale Transporte.txt"), Properties.Resource1.VALE_TRANSPORTE);

                    //att 1
                        File.WriteAllText(Path.Combine(p.FullName, "Plano Unimed.txt"), Properties.Resource1.PlanoUnimed);
                    //2
                        File.WriteAllText(Path.Combine(p.FullName, "Seguro de Vida.txt"), Properties.Resource1.SeguroDeVidaAUT);
                       
                        File.WriteAllText(Path.Combine(p.FullName, "Uniformes.txt"), Properties.Resource1.Uniformes);
                    //3   
                        File.WriteAllText(Path.Combine(p.FullName, "Plano Odontologico.txt"), Properties.Resource1.PlanoOdonto);

                    for (int i = 0;i<paths.Length;i++) {
                        string contA= "";
                        using (StreamReader sr = new(paths[i])) {
                             contA = sr.ReadToEnd();

                        }

                        using (StreamWriter sw = new(paths[i]))
                        {
                            sw.WriteLine(att[i] + contA);

                        }

                    }
                    }


                    string[] paths1 = {
                        Path.Combine(p1.FullName, "Seguro de Vida.txt"),
                        Path.Combine(p1.FullName, "Plano Unimed.txt"),
                        Path.Combine(p1.FullName, "Plano Odontologico.txt")
                    };
                    string[] att1 = {
                   $"Attachment:{Path.Combine(a.FullName, "Formulário Designação de Beneficiários.pdf")};",

                   $"Attachment:{Path.Combine(a.FullName, "planilha para inclusões- TIPAN.xls")};" +
                   $" \nAttachment:{Path.Combine(a.FullName, "Unimed Fesp Nacional - Apresentação.doc")};\n",

                   $"Attachment:{Path.Combine(a.FullName, "Termo de Opção - Dental SKA.pdf")};\r\n" +
                   $"Attachment:{Path.Combine(a.FullName, "Lista de procedimentos - Odonto.pdf")};\r\n" +
                   $"Attachment:{Path.Combine(a.FullName, "Unimed Fesp Nacional - Apresentação.doc")};\r\n"


                    };

                    //cria arquivo dos e-mails
                    if (!p1.Exists || Properties.Settings.Default.primeiroLogin) { 
                        p1.Create();
                    File.WriteAllText(p1 + "/Gympass.txt", Properties.Resource1.Gympass);
                    File.WriteAllText(Path.Combine(p1.FullName, "Vale Transporte.txt"), Properties.Resource1.VALE_TRANSPORTE);

                    //att 1
                    File.WriteAllText(Path.Combine(p1.FullName, "Plano Unimed.txt"), Properties.Resource1.PlanoUnimed);
                    //2
                    File.WriteAllText(Path.Combine(p1.FullName, "Seguro de Vida.txt"), Properties.Resource1.SegudoDeVidaSis);

                    File.WriteAllText(Path.Combine(p1.FullName, "Uniformes.txt"), Properties.Resource1.Uniformes);
                    //3   
                    File.WriteAllText(Path.Combine(p1.FullName, "Plano Odontologico.txt"), Properties.Resource1.PlanoOdonto);

                    for (int i = 0; i < paths1.Length; i++)
                    {
                        string contA = "";
                        using (StreamReader sr = new(paths1[i]))
                        {
                            contA = sr.ReadToEnd();

                        }

                        using (StreamWriter sw = new(paths1[i]))
                        {
                            sw.WriteLine(att[i] + contA);

                        }

                    }

                    }




                    Properties.Settings.Default.primeiroLogin = false;
                    Properties.Settings.Default.Save();
                }
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
            userName.Content = LoginScreen.usuario.Name??"Perfil";
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

        private void Sair(object sender, RoutedEventArgs e)
        {
            
            Properties.Settings.Default.primeiroLogin = true;
            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Application.Current.Shutdown();
        }

        private void VerificarAtt(object sender, RoutedEventArgs e)
        {
            Atualizar();
            
        }
    }



}
