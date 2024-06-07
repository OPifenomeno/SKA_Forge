using Microsoft.Graph;
using Microsoft.Graph.Me.SendMail;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using skaf.Screen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace skaf
{
    /// <summary>
    /// Interação lógica para EmailGroupBox.xaml
    /// </summary>
    public partial class EmailGroupBox : UserControl
    {
     
       public string[] _emails = new string[99]; 
       public List<TextBox> _emailsList = new();
        protected string nomeA;
        
       
        public EmailGroupBox(string ?nome)
        {
         
            InitializeComponent();
            if (nome != null)
            {
                nomeA = nome;
                ModelName.Text = nome;
            }
            ModelName.KeyDown += (sender, e) => {
                if (e.Key == Key.Enter)
                {
                   
                    SendButton.Focus();
                }
            };

        }

        private void BtNovoEmail(object sender, RoutedEventArgs e)
        {
            TextBox novoEmail = new TextBox() {
                MaxLines = 1,
                Background = Brushes.Transparent,
                Foreground = Brushes.Black,
                TextAlignment = TextAlignment.Center,
                
           };
            ContextMenu menu = new ContextMenu();
            MenuItem limpar = new MenuItem() { Header = "Limpar" };
            limpar.Click += (s, e) => {
                DestBlock.Children.Clear();
            };
            menu.Items.Add(limpar);
            DestBlock.ContextMenu = menu;
            AddDest.ContextMenu = menu;

            novoEmail.ContextMenu = menu;


            novoEmail.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    switch (novoEmail.Text) {

                        case "/clear":
                            DestBlock.Children.Clear();
                            break;
                        case "/u":
                            DestBlock.Children.RemoveAt(DestBlock.Children.Count - 1);
                            break;
                        default:

                            if (novoEmail.Text == string.Empty || !novoEmail.Text.Contains("@"))
                            {
                                MessageBox.Show("Endereço de E-mail inválido!");
                            }
                            else {
                                TextBox? current = sender as TextBox;
                                int currentI = _emailsList.IndexOf(current);
                                if (currentI< _emailsList.Count - 1)
                                {
                                    TextBox nextTextBox = _emailsList[currentI + 1];

                                    nextTextBox.Focus();
                                }
                            }

                            break;
                    }
                
                }
          
            
            };
            _emailsList.Add(novoEmail);
            DockPanel.SetDock(novoEmail,Dock.Top);
            DestBlock.Children.Add(novoEmail);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
    
            foreach (TextBox i in DestBlock.Children)
            {
                EnviarEmail(i.Text);

            }
        }

        private string LerCaminhoAnexo(string texto) {
            string caminho = string.Empty;
            char[] chars = texto.ToCharArray();
            for (int i = 0; i < texto.Length; i++) {
                if (chars[i].ToString().Equals(";")) {

                    caminho += chars[i];
                    return caminho;
                }

                else {
                    caminho += chars[i];
                }
            }

            return caminho;
            
        }

        public List<byte[]> ConferirAttachs(string @conteudo) {
            

            List < byte[]> attachs = new List <byte[]>();
           
            while (conteudo.Contains("Attachment:")) {
                string caminho = LerCaminhoAnexo(@conteudo);
                conteudo = conteudo.Replace(@caminho, "");
                caminho = caminho.Replace("Attachment:", "").Replace(";","");
                
             
                try
                {
                    byte[] arq = File.ReadAllBytes(caminho.Trim());
                    attachs.Add(arq);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Não foi possível localizar o Anexo: " + caminho + "\n\n" + e.Message);
                    Clipboard.SetText(e.Message);
                }
               


            }




            return attachs;
            
        }
      
        
        
        
        
        private void EnviarEmail(string para){

        string conteudomsg(string texto) {
                texto = texto.Replace(LerCaminhoAnexo(texto) +";", "") ;

                if (texto.Contains("Attachment: ")){
                    return conteudomsg(texto);
                }
                else {
                    return texto;
                }
            }
           
            string[] nome = [];
            List<byte[]> attachments = new List<byte[]>();   
            List<object> attachFinal = new List<object>();
                DirectoryInfo inf = new DirectoryInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", ModelName.Text));
            if (!inf.Exists || inf.GetFiles().Length == 0) { MessageBox.Show("Confira o diretório."); }
            else
            {

                inf.GetFiles().ToList().ForEach(async f =>
                {
                string conteudo = File.ReadAllText(f.FullName) + "\n" + (LoginScreen.usuario.Assinatura != null ? LoginScreen.usuario.Assinatura : null);

                
                        attachments = ConferirAttachs(conteudo);
                                        
                    

                    foreach (byte[] att in attachments)
                    {
                        string caminho = LerCaminhoAnexo(conteudo);

                        attachFinal.Add(new Dictionary<string, object>
        {
            { "@odata.type", "#microsoft.graph.fileAttachment" },
            { "name", System.IO.Path.GetFileName(LerCaminhoAnexo(caminho)).Replace(";","") },
            { "contentBytes", Convert.ToBase64String(att) }
        });
                        conteudo = conteudo.Replace(caminho,"");
                        
                    }


                    var jsonBody = JsonConvert.SerializeObject(new
                    {
                        message = new
                        {
                            subject = f.Name.Replace(".txt", " "),
                            body = new
                            {
                                contentType = "html",
                                content = conteudomsg(conteudo),
                            },
                            toRecipients = new[] {

                                new {
                                    emailAddress = new {
                                        address = para
                                    }
                                }
                            },
                            attachments = attachFinal.ToArray()

                        }
                    });


                    
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.token);
                            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                            var response = await client.PostAsync("https://graph.microsoft.com/v1.0/me/sendMail", content);

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Operação concluída!");

                            }
                            else
                            {
                                var responseR = await response.Content.ReadAsStringAsync();
                                MessageBox.Show($"Erro ao enviar e-mail: {response.StatusCode},{responseR}");

                            }


                        }
                    
                   


                });

            }
           





        }
        private void Menuzito(object sender, MouseButtonEventArgs e)
        {
            ContextMenu menu = new ContextMenu();
            
            MenuItem limpar = new MenuItem() { Header = "Limpar" };
            limpar.Click += (s, e) => {
            DestBlock.Children.Clear();
            };
            menu.Items.Add(limpar);
            DestBlock.ContextMenu = menu;
            AddDest.ContextMenu = menu;
        }

        private void ConfirmaNome(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    string dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", nomeA);
                    Directory.Move(dir, dir.Replace(nomeA, ModelName.Text));
                    nomeA = ModelName.Text;
                }
                catch (Exception) { }
            }
        }

        private void ModelName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!ModelName.Text.Equals(nomeA)) {
                try
                {
                    string dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails", nomeA);
                    Directory.Move(dir, dir.Replace(nomeA, ModelName.Text));
                    nomeA = ModelName.Text;
                }
                catch (Exception) { }
            }
        }

        private void AbrirConfig(object sender, RoutedEventArgs e)
        {
           configModel md = new(this);
            md.ShowDialog();
            
        }
    }


    

}


