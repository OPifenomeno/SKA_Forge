﻿using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace skaf
{
    /// <summary>
    /// Lógica interna para JanelaPrincipal.xaml
    /// </summary>
    public partial class JanelaPrincipal : Window
    {
        public JanelaPrincipal()
        {
            InitializeComponent();
            carregarModelos();
        }
         void carregarModelos() {
                    DirectoryInfo past = new DirectoryInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Emails"));
                    past.GetDirectories().ToList().ForEach(dir =>
                    {
                        EmailGroupBox gp = new EmailGroupBox(dir.Name);
                        Canvas.SetZIndex(gp, 4);
                        DockPanel.SetDock(gp, Dock.Top) ;
                        conteiner.Children.Add(gp);

                    });
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

        

        private void AbrirMenu(object sender, MouseButtonEventArgs e)
        {
            if (MenuBar.Width != MenuBar.MaxWidth)
            {
                MenuBar.BeginAnimation(WidthProperty, new DoubleAnimation
                {
                    From = 0,
                    To = MenuBar.MaxWidth,
                    Duration = TimeSpan.FromSeconds(1),
                    EasingFunction = new QuadraticEase() {EasingMode=EasingMode.EaseInOut}
                });
            }
            else {
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
