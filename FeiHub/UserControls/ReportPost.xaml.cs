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
    /// Lógica de interacción para ReportPost.xaml
    /// </summary>
    public partial class ReportPost : UserControl
    {
        public ReportPost()
        {
            InitializeComponent();
            AddSituations();
        }
        private void AddSituations()
        {
            List<String> situations = new List<string>();
            situations.Add("Es spam");
            situations.Add("Lenguaje o símbolos que incitan al odio");
            situations.Add("Bullying o acoso");
            situations.Add("Información falsa");
            situations.Add("Expone información sensible");


            DataGrid_Situations.ItemsSource = situations;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Aquí va el método de reportar, el idPost se recupera en this.tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportThisPost(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Funcion de reportar, ID Recibido: " + this.Tag);
            this.Visibility = Visibility.Collapsed;
        }
    }
}
