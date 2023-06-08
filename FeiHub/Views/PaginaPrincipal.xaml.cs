using System;
using System.Collections.Generic;
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

namespace FeiHub.Views
{
    /// <summary>
    /// Lógica de interacción para PaginaPrincipal.xaml
    /// </summary>
    public partial class PaginaPrincipal : Page
    {
        public PaginaPrincipal()
        {
            InitializeComponent();
            AgregarSeguidos();
        }

        public void AgregarSeguidos()
        {
            UserControls.Seguidor seguidor = new UserControls.Seguidor();
            seguidor.seguidor.Username = "Carsiano";
            ImageSourceConverter converter = new ImageSourceConverter();
            seguidor.seguidor.Source = (ImageSource)converter.ConvertFromString("../../Resources/uv.png");

            seguidor.seguidor.TextBlock_Username.MouseDown += IrAPerfil;
            seguidor.seguidor.Button_EnviarMensaje.Click += EnviarMensaje;

            StackPanel_Seguidos.Children.Add(seguidor);
            UserControls.Seguidor seguidor2 = new UserControls.Seguidor();
            seguidor2.seguidor.Username = "Saraiche";
            seguidor2.seguidor.Source = (ImageSource)converter.ConvertFromString("../../Resources/pic.jpg");

            seguidor2.seguidor.TextBlock_Username.MouseDown += IrAPerfil;
            seguidor2.seguidor.Button_EnviarMensaje.Click += EnviarMensaje;
            
            StackPanel_Seguidos.Children.Add(seguidor2);

        }

        private void EnviarMensaje(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mando mensaje a " + (((((e.Source as Button).Parent as Grid).Parent as Border).Parent as UserControl) as UserControls.Seguidor).Username);
        }

        private void IrAPerfil(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Hola, me voy al perfil de " + (((((e.Source as TextBlock).Parent as Grid).Parent as Border).Parent as UserControl) as UserControls.Seguidor).Username);
        }


    }
}
