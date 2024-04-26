using System;
using System.Collections.Generic;
using System.IO;
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
       public List<TextBox> _emailsList = new List<TextBox>();
       
       
        public EmailGroupBox(string nome)
        {
         
            InitializeComponent();
            if (nome != null)
            {
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
                Foreground = Brushes.White,
                TextAlignment = TextAlignment.Center,
                
           };
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

      
    }


    

}
