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

namespace FeiHub.Vistas
{
    /// <summary>
    /// Lógica de interacción para InicioDeSesion.xaml
    /// </summary>
    public partial class InicioDeSesion : Page
    {
        public InicioDeSesion()
        {
            InitializeComponent();
        }

        private void ButtonRegistrate_Click(object sender, RoutedEventArgs e)
        {
            IniciarSesion.Visibility = Visibility.Collapsed;
            Registrarse.Visibility = Visibility.Visible;
        }

        private void ButtonIniciarSesion_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonIrAIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            IniciarSesion.Visibility = Visibility.Visible;
            Registrarse.Visibility = Visibility.Collapsed;
            
            /* 
            Button button = new Button();
            button.Content = "Hola";
            IniciarSesion.Children.Add(button);
            */
        }
    }
}
