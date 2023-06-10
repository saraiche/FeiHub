using FeiHub.Models;
using FeiHub.UserControls;
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
    /// Lógica de interacción para Chat.xaml
    /// </summary>
    public partial class Chat : Page
    {
        public Chat()
        {
            InitializeComponent();
            AddFollowing();
        }

        public void AddFollowing()
        {
            
            PreviewUser following = new PreviewUser();

            ImageSourceConverter converter = new ImageSourceConverter();
            following.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
            following.previewUser.Username = "Saraiche";
            following.previewUser.Border_Seguidor.MouseDown += Border_Seguidor_MouseDown;
            StackPanel_Following.Children.Add(following);

            PreviewUser following2 = new PreviewUser();
            following2.ThisVisibility = Visibility.Collapsed;
            following2.Username = "Carsiano";
            following2.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/pic.jpg");
            following2.previewUser.Border_Seguidor.MouseDown += Border_Seguidor_MouseDown;
            StackPanel_Following.Children.Add(following2);

            PreviewUser following3 = new PreviewUser();
            following3.ThisVisibility = Visibility.Collapsed;
            following3.Username = "Carsiano";
            following3.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/pic.jpg");
            StackPanel_Following.Children.Add(following3);

            PreviewUser following4 = new PreviewUser();
            following4.ThisVisibility = Visibility.Collapsed;
            following4.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
            following4.previewUser.Username = "Saraiche";

            StackPanel_Following.Children.Add(following4);

            PreviewUser following5 = new PreviewUser();
            following5.ThisVisibility = Visibility.Collapsed;
            following5.Username = "Carsiano";
            following5.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/pic.jpg");
            StackPanel_Following.Children.Add(following5);

            PreviewUser following6 = new PreviewUser();
            following6.ThisVisibility = Visibility.Collapsed;
            following6.Username = "Carsiano";
            following6.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/pic.jpg");
            StackPanel_Following.Children.Add(following6);

            PreviewUser following7 = new PreviewUser();
            following7.ThisVisibility = Visibility.Collapsed;
            following7.Username = "Carsiano";
            following7.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/pic.jpg");
            StackPanel_Following.Children.Add(following7);

            PreviewUser following8 = new PreviewUser();
            following8.ThisVisibility = Visibility.Collapsed;
            following8.Username = "Carsiano";
            following8.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/pic.jpg");
            StackPanel_Following.Children.Add(following8);

            PreviewUser following9 = new PreviewUser();
            following9.ThisVisibility = Visibility.Collapsed;
            following9.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
            following9.previewUser.Username = "Saraiche";

            StackPanel_Following.Children.Add(following9);

            PreviewUser following10 = new PreviewUser();
            following10.Username = "Carsiano";
            StackPanel_Following.Children.Add(following10);

            PreviewUser following11 = new PreviewUser();
            following11.ThisVisibility = Visibility.Collapsed;
            following11.Username = "Carsiano";
            following11.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/pic.jpg");
            StackPanel_Following.Children.Add(following11);
        }

        private void Border_Seguidor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Se debe mantener la parte del código, solo cambiar la manera de presentar los mensajes
            string username = (((sender as Border).Parent as UserControl) as PreviewUser).Username;
            Label_NoChatSelected.Visibility = Visibility.Collapsed;
            Label_Username.Content = username;
            Label_Username.Visibility = Visibility.Visible;
            ScrollViewer_ListMessages.Visibility = Visibility.Visible;
            ListView_Chat.Items.Clear();
            StackPanel_MessageToSend.Visibility = Visibility.Visible;

            //Esta es la línea que se debe modificar 
            ListView_Chat.Items.Add(username + " : Hola");
            ListView_Chat.Items.Add(username + " : Hola");
            ListView_Chat.Items.Add(username + " : Hola");

        }
    }
}
