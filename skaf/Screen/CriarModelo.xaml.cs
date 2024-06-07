using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;
namespace skaf
{
    /// <summary>
    /// Lógica interna para CriarModelo.xaml
    /// </summary>
    public partial class CriarModelo : Window
    {
        string[] filePaths = new string[10];
        JanelaPrincipal principal;
        public CriarModelo(JanelaPrincipal janelaP)
        {
            this.principal = janelaP;
            InitializeComponent();
            
        }

        private void DialogoArquivo(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;
            openFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|html files(*.html)|*.html";
            openFile.FileOk += (sender, e)=>{

                for(int i=0; i<filePaths.Length; i++) {
                    filePaths[i] = openFile.FileNames[i];
                    ArquivosPane.Children.Add(new Label() {Content = $"{i + 1}.{filePaths[i]}"});
                }
            };


            openFile.ShowDialog();
        }

     
        private async Task CriarPasta(string[] path, string nomePasta) {
            string caminhoModelo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Emails",nomePasta.Normalize());
            Directory.CreateDirectory(caminhoModelo).Attributes &= ~FileAttributes.ReadOnly;


            Process.Start("explorer.exe", caminhoModelo);
                
                for(int i = 0;i < path.Length;i++)
            { 
                if (path[i] == string.Empty)
                {
                    break;
                }
                else {
                    using (StreamReader reader = new(path[i]))
                    {
                        
                        string conteudo = reader.ReadToEnd();
                        using (StreamWriter writer = new StreamWriter(System.IO.Path.Combine(caminhoModelo, $"Email_{i + 1}.txt")))
                        {
                            
                            writer.WriteLine(conteudo);
                            writer.Flush();


                        }
                    }
                }

             
            }

           
        }

        private async void ProntoBt(object sender, RoutedEventArgs e)
        {
            if (TextBoxFile.Text.Equals(string.Empty))
            {
                MessageBox.Show("Por favor, atribua um nome ao modelo");


            }
            else
            {
                try { await CriarPasta(filePaths, TextBoxFile.Text); }
                catch (Exception) {
                   
                }
              

                EmailGroupBox gp = new EmailGroupBox(TextBoxFile.Text);
                principal.conteiner.Children.Add(gp);
                gp.HorizontalAlignment = HorizontalAlignment.Stretch;
                DockPanel.SetDock(gp, Dock.Top);

            }
           
          
        }
    }
}
