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

namespace FeiHub.UserControls
{
    /// <summary>
    /// Lógica de interacción para Publicacion.xaml
    /// </summary>
    public partial class Publicacion : UserControl
    {
        public Publicacion()
        {
            InitializeComponent();
        }

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(Publicacion));

        public ImageSource FotoPerfil
        {
            get { return (ImageSource)GetValue(FotoPerfilProperty); }
            set { SetValue(FotoPerfilProperty, value); }
        }

        public static readonly DependencyProperty FotoPerfilProperty = DependencyProperty.Register("FotoPerfilProperty", typeof(ImageSource), typeof(Publicacion));

        public DateTime Fecha
        {
            get { return (DateTime)GetValue(FechaProperty); }
            set { SetValue(FechaProperty, value); }
        }
        public static readonly DependencyProperty FechaProperty = DependencyProperty.Register("FechaProperty", typeof(DateTime), typeof(Publicacion));

        public string Titulo
        {
            get { return (string)GetValue(TituloProperty); }
            set { SetValue(TituloProperty, value); }
        }
        public static readonly DependencyProperty TituloProperty =
            DependencyProperty.Register("TituloProperty", typeof(string), typeof(Publicacion));

        public string Cuerpo
        {
            get { return (string)GetValue(CuerpoProperty); }
            set { SetValue(CuerpoProperty, value); }
        }
        public static readonly DependencyProperty CuerpoProperty =
            DependencyProperty.Register("CuerpoProperty", typeof(string), typeof(Publicacion));
    }
}
