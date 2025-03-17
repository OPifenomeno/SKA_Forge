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
using System.Drawing;
using System.Drawing.Drawing2D;



namespace skaf.Screen
{
    /// <summary>
    /// Lógica interna para Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        string selec;


        public Config()
        {
          
            InitializeComponent();
            LoadBoxes();
            
        }
   


        private void LoadBoxes() {
            tratarImgBox.IsChecked = Properties.Settings.Default.tratarImagem;
            
            
            if (tratarImgBox.IsChecked.Value && !string.IsNullOrEmpty(Properties.Settings.Default.imagem))
            {
                selec = Properties.Settings.Default.imagemTratada;
            }
            else { selec = Properties.Settings.Default.imagem; }

            carregarFoto(selec);

            if (selec != string.Empty) {
                byte[] byt = Convert.FromBase64String(selec);
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
                selec = Properties.Settings.Default.imagem;
                tratarImgBox.IsChecked = false;
            }
            carregarFoto(selec);
        }

        private void carregarFoto(string selec)
        {
            if (!string.IsNullOrEmpty(selec))
            {
                byte[] img = Convert.FromBase64String(selec);

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

        public Bitmap tratarImg(string base64IMG) {
            this.Cursor = Cursors.Wait;
            byte[] bts = Convert.FromBase64String(base64IMG);
            BitmapImage bitmapImage = byteArrayToImage(bts);
            Bitmap original;

            using (MemoryStream outStream = new MemoryStream()) {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(outStream);

                original = new Bitmap(outStream);
            }

            Bitmap grays = new(original.Width,original.Height);
            for(int y = 0; y < original.Height; y++) {
            for (int x = 0; x < original.Width; x++){
                System.Drawing.Color corPixel = original.GetPixel(x, y);

                    int grayscale = (int)(corPixel.R*0.3 + corPixel.G * 0.59+corPixel.B*0.11);
                    System.Drawing.Color gray = System.Drawing.Color.FromArgb(grayscale, grayscale, grayscale);
                    grays.SetPixel(x, y, gray);

                }
            
            }
             selec = Properties.Settings.Default.imagemTratada; using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(0, 0, grays.Width, grays.Height);
                using (Graphics gr = Graphics.FromImage(grays))
                {
                    gr.SetClip(gp);
                    gr.DrawImage(grays, System.Drawing.Point.Empty);
                }
            }
            this.Cursor = Cursors.Arrow;
            return grays;
        }

        private void tratarImgBox_Click(object sender, RoutedEventArgs e)
        {
            skaf.Properties.Settings.Default.tratarImagem = tratarImgBox.IsChecked.Value;
            if (Properties.Settings.Default.tratarImagem && Properties.Settings.Default.imagem != null)
            {
               
                try
                {
                        Properties.Settings.Default.imagemTratada = Convert.ToBase64String(ImageToByte(tratarImg(Properties.Settings.Default.imagem)));

                }
                catch (Exception ex) { MessageBox.Show(ex.StackTrace); }
                selec = Properties.Settings.Default.imagemTratada;
            }
            else { selec = Properties.Settings.Default.imagem; }
            
            skaf.Properties.Settings.Default.Save();
            carregarFoto(selec);
        }
    }
}
