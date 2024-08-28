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
using System.Windows.Shapes;
using NuGet;




namespace skaf.Screen
{
    /// <summary>
    /// Lógica interna para Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        public Config()
        {
          
            InitializeComponent();
            LoadBoxes();
            
        }
   


        private void LoadBoxes() {
            carregarFoto();
            if (Properties.Settings.Default.imagem != string.Empty) {
                byte[] byt = Convert.FromBase64String(Properties.Settings.Default.imagem);
                using (var ms = new MemoryStream(byt))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    ImageBox.Source = image;
                }

            }
            NomeBox.Text = Properties.Settings.Default.Nome ?? "@nome";
            FoneBox.Text = " (51) 3591-2900";
            LinkedinBox.Text = Properties.Settings.Default.linkedin ?? "";


        }

        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }




        private void submit(object sender, RoutedEventArgs e)
        {
            try
            {
                var v = Properties.Settings.Default;

               
                LoginScreen.usuario.Name = NomeBox.Text;
                LoginScreen.usuario.Linkedin = LinkedinBox.Text; 
                v.Nome = NomeBox.Text;
                v.linkedin = LinkedinBox.Text;
                v.Save(); } catch(Exception es){ MessageBox.Show(es.Message + es.StackTrace); }
            this.Close();
           
        }

        private void MudarFotoClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            
            dialog.Filter = "Png images (.png)|*.png|jpg images (.jpeg)|*.jpg";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                byte[] arq = File.ReadAllBytes(dialog.FileName);
                string base64 = Convert.ToBase64String(arq);
                Properties.Settings.Default.imagem = base64;
                Properties.Settings.Default.Save();
              
            }
            carregarFoto();
        }

        private void carregarFoto()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.imagem))
            {
                byte[] img = Convert.FromBase64String(Properties.Settings.Default.imagem);

                ImageBox.Source = byteArrayToImage(img) ;

            }

            
        }
        public BitmapImage byteArrayToImage(byte[] img)
        {
            using (var ms = new MemoryStream(img))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }

       
    }
}
