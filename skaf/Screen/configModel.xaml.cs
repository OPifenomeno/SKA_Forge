using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
                    
                    ultimoBt = file.Name;
                    titleBox.Text = file.Name.Replace(".txt","");
                    TextMail.Document.Blocks.Clear();

                    

                    try {
                        Encoding encoding = Encoding.UTF8;
                        

                        string fileContent = File.ReadAllText(file.FullName,encoding);
                    

                        TextMail.AppendText(fileContent);
                    
                    
                    
                    
                    } catch(Exception i){ MessageBox.Show(i.Message); }
                    
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

                File.Create(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text, $"Email_{dir.GetFiles().Length}.txt"));

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            LoadModel();

        }

        private void SalvarModel(object sender, RoutedEventArgs e)
        {
          
            try {
                string caminhoAntigo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text, $"{ultimoBt}");
                string caminhoNovo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text, $"{titleBox.Text}.txt");
                if (!File.Exists(caminhoNovo)) { File.Move(caminhoAntigo, caminhoNovo); }
                
                LoadModel();
                using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text, $"{titleBox.Text}.txt")))
                {
                    TextRange tr = new TextRange(TextMail.Document.ContentStart, TextMail.Document.ContentEnd);
                    
                    sw.Write(tr.Text);

                }
            }
            catch (Exception ez){ MessageBox.Show($"{ez.Message}\n {ez.StackTrace}"); }
            
        }

        private void excluirEmail(object sender, RoutedEventArgs e)
        {
            SalvarModel(sender, e);
            try {
                File.Delete(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", gpM.ModelName.Text, $"{titleBox.Text}.txt"));
            }
            catch {
                MessageBox.Show("Não foi possível deletar o e-mail.");
            }
            LoadModel();
            Button? btFind = buttonPanel.Children.OfType<Button>().FirstOrDefault();
            if (btFind != null) {
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




    }
}
