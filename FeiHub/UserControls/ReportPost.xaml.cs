using FeiHub.Models;
using FeiHub.Services;
using FeiHub.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        int checkedCount = 0;
        PostsAPIServices postsAPIServices = new PostsAPIServices();
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
        private async void ReportThisPost(object sender, RoutedEventArgs e)
        {
            foreach (string item in DataGrid_Situations.Items)
            {
                var row = DataGrid_Situations.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    var checkBox = DataGrid_Situations.Columns[1].GetCellContent(row) as CheckBox;
                    if (checkBox.IsChecked == true)
                    {
                        checkedCount++;
                    }

                }
            }
            HttpResponseMessage response =  await postsAPIServices.AddReport(this.Tag.ToString(), checkedCount);
            if (response.IsSuccessStatusCode)
            {
                MessageBoxResult result = MessageBox.Show($"Reporte enviado", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                SingletonUser.Instance.BorrarSinglenton();
                this.Visibility = Visibility.Collapsed;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                MessageBox.Show($"Tuvimos un error al enviar el reporte, inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Visibility = Visibility.Collapsed;
        }
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T found)
                {
                    return found;
                }

                var foundChild = FindVisualChild<T>(child);
                if (foundChild != null)
                {
                    return foundChild;
                }
            }

            return null;
        }
    }
}
