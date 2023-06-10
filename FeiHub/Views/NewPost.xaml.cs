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
    /// Lógica de interacción para NewPost.xaml
    /// </summary>
    public partial class NewPost : Page
    {
        public NewPost()
        {
            InitializeComponent();
        }

        public NewPost(int idPublicacion)
        {
            InitializeComponent();
            Label_PageTitle.Content = "Editar Publicación";
            TextBox_Title.Text = "Aquí va el titulo recuperado";
            TextBox_Body.Text = "Aquí va toda la descrípción";
            Button_Post_SaveChanges.Content = "Guardar cambios";
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
