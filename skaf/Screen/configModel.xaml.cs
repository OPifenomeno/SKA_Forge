using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
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

namespace skaf.Screen
{
    /// <summary>
    /// Lógica interna para configModel.xaml
    /// </summary>
    public partial class configModel : Window
    {
        EmailGroupBox gpM;
        public DirectoryInfo dir;
        string ?ultimoBt = null;
        public configModel(EmailGroupBox gp)
        {
            this.gpM = gp;
            dir =new DirectoryInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text));
            InitializeComponent();
            LoadModel();
        }

        public void LoadModel() {
            DirectoryInfo past = new (System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails",gpM.ModelName.Text));
            buttonPanel.Children.Clear();

            //adicionar os botoes
            foreach (var file in past.GetFiles())
            {
                Button btn = new Button()
                {
                    Content = file.Name.Replace(".txt", ""),
                    Height = 60,
                    Background = new SolidColorBrush(Colors.White),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    
                };
                DockPanel.SetDock(btn, Dock.Top);
                btn.Click += (s, e) => {

                    btn.IsEnabled = false;  
                        ultimoBt = file.Name;
                        titleBox.Text = file.Name.Replace(".txt", "").Replace("_","");
                        TextMail.Document.Blocks.Clear();
                        try
                        {
                            TextMail.AppendText(File.ReadAllText(System.IO.Path.GetFullPath(file.FullName)));
                            
                        }
                        catch (Exception mes){ MessageBox.Show(mes.Message); }
                   
                    btn.IsEnabled = true;

                };
                buttonPanel.Children.Add(btn);
            }
        }

        private void ExcluirModelo(object sender, MouseButtonEventArgs e)
        {
            dir.Delete();
            
            this.Close();
        }

        private void criarEmail(object sender, RoutedEventArgs e)
        {
            
            try
            {

                ultimoBt = $"Email{dir.GetFiles().Length}.txt";

                File.Create(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text, $"Email{dir.GetFiles().Length}.txt"));
    
                titleBox.Text = ultimoBt.Replace(".txt","");
                TextMail.Document.Blocks.Clear();
                
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message,ex.StackTrace); }
            
            LoadModel();

        }

        public  void SalvarModel(object sender, RoutedEventArgs e)
        {
            
            
            try {
                string caminhoAntigo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text, $"{ultimoBt}");
                string caminhoNovo =System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text, $"{titleBox.Text}.txt");
                if (!File.Exists(caminhoNovo) && caminhoNovo != caminhoAntigo) { File.Move(caminhoAntigo, caminhoNovo); }
                LoadModel();


                using (StreamWriter sw = new StreamWriter(caminhoNovo))
                {
                    TextRange tr = new TextRange(TextMail.Document.ContentStart, TextMail.Document.ContentEnd);
                    
                     sw.Write(tr.Text);
                     sw.Flush();
                }
            }
            catch (Exception ez){ MessageBox.Show($"{ez.Message} \n {ez.StackTrace}"); }
            
        }

        private void excluirEmail(object sender, RoutedEventArgs e)
        {
            string caminhoArquivo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text, $"{titleBox.Text}.txt");

            if (File.Exists(caminhoArquivo))
            {
                try
                {
                    // Tente excluir o arquivo
                    File.Delete(caminhoArquivo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Não foi possível deletar o e-mail. " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Arquivo não encontrado.");
            }

            

                LoadModel();

            Button? btFind = buttonPanel.Children.OfType<Button>().FirstOrDefault();
            if (btFind != null)
            {
                btFind.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void addAnexo(object sender, RoutedEventArgs e)
        {
            SalvarModel(sender,e);
            if(!Directory.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", "Anexos")))
            {
                Directory.CreateDirectory(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", "Anexos"));
            }
            DirectoryInfo diretorioPasta = new DirectoryInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", "Anexos"));

            var fd = new Microsoft.Win32.OpenFileDialog();
            fd.Multiselect = true;
            bool? result = fd.ShowDialog();

            

                if (result == true)
                {
                    foreach (var item in fd.FileNames)
                    {
                        //copiar arquivos para pasta Anexos
                        FileInfo fl = new FileInfo(item);
                    if (!diretorioPasta.GetFiles().Any(file => file.Name == fl.Name))
                    {
                        File.Copy(fl.FullName, System.IO.Path.Combine(diretorioPasta.FullName, fl.Name));
                    }

                    //adicionar no e-mail.
                    TextRange tr = new TextRange(TextMail.Document.ContentStart, TextMail.Document.ContentEnd);
                    string conteudoA = tr.Text;
                    string newC = $"Attachment: {System.IO.Path.Combine(diretorioPasta.FullName, fl.Name)};\n{conteudoA}";
                    try
                    {
                        File.WriteAllText(System.IO.Path.Combine(diretorioPasta.FullName, fl.Name),string.Empty);
                        File.WriteAllText(System.IO.Path.Combine(diretorioPasta.FullName, fl.Name), newC);
                        TextMail.Document.Blocks.Clear();
                        TextMail.AppendText(newC);
                        
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                        //using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text, $"{titleBox.Text}")))
                        //{
                        //    TextRange tr = new TextRange(TextMail.Document.ContentStart, TextMail.Document.ContentEnd);
                        //    conteudoA = tr.Text;
                        //    sw.Write($"Attachment: {System.IO.Path.Combine(diretorioPasta.FullName, fl.Name)};\n{conteudoA}");

                    //}
                    }
                }
            

            SalvarModel(sender ,e);
            Button? btFind = buttonPanel.Children.OfType<Button>().FirstOrDefault(btn => btn.Content.ToString() == $"{titleBox.Text}");
            if (btFind != null)
            {
                btFind.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else { MessageBox.Show("a"); }
        }


        private void TextMail_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Deseja abrir um editor de texto dinâmico (Necessário conexão com a internet)?","Editar texto",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No);
            if (result == MessageBoxResult.Yes) {
                try
                {
                    this.Topmost = true;
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo {FileName= "https://editorhtmlonline.com/pt/",UseShellExecute=true });
                    Clipboard.SetText(Properties.Resource1.Como_usar_editor);
                    MessageBox.Show("Aperte ctrl+v na caixa da DIREITA do editor, ok?", "Como usar");
                    this.Topmost = false;
                }
                catch(Exception exp){ MessageBox.Show(exp.Message); }
            }
        }
    }
}
