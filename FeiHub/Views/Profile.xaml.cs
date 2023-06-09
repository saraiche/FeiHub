using FeiHub.Models;
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
    /// Lógica de interacción para Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public User User { get; set; }
        public Profile()
        {
            InitializeComponent();
            AddImagesToPost();
        }
        public Profile(User user)
        {
            InitializeComponent();
            this.User = user;
            if (String.IsNullOrEmpty(user.educationalProgram))
            {
                Label_EducationalProgram.Visibility = Visibility.Collapsed;
                Label_Mail.Visibility = Visibility.Collapsed;
                Label_UserType.Content = "Académico";
            }
            else
            {
                Label_EducationalProgram.Visibility = Visibility.Visible;
                Label_EducationalProgram.Content = User.educationalProgram;
                Label_Mail.Content = User.schoolId;
                Label_UserType.Content = "Estudiante";
            }
            FillHeader();
        }

        /// <summary>
        /// Write the data from the user in the header
        /// </summary>
        public void FillHeader()
        {
            Label_Name.Content = User.name + " " + User.paternalSurname + " " + User.maternalSurname;
            Label_Username.Content = User.username;
        }
        public void AddImagesToPost()
        {
            UserControls.PostPreview postPreview = new UserControls.PostPreview();
            ImageSourceConverter converter = new ImageSourceConverter();
            Image image = new Image();
            image.Source = (ImageSource)converter.ConvertFromString("../../Resources/uv.png");
            postPreview.WrapPanel_Images.Children.Add(image);
            StackPanel_Posts.Children.Add(postPreview);
            
        }

        private void ShowFollowing(object sender, RoutedEventArgs e)
        {
            Label_Content.Content = "Siguiendo";
            WrapPanel_Following.Visibility = Visibility.Visible;
            WrapPanel_Followers.Visibility = Visibility.Collapsed;
            StackPanel_Posts.Visibility = Visibility.Collapsed;
        }

        private void ShowFollowers(object sender, RoutedEventArgs e)
        {
            Label_Content.Content = "Seguidores";
            WrapPanel_Followers.Visibility = Visibility.Visible;
            WrapPanel_Following.Visibility = Visibility.Collapsed;
            StackPanel_Posts.Visibility = Visibility.Collapsed;
        }

        private void ShowPosts(object sender, RoutedEventArgs e)
        {
            Label_Content.Content = "Publicaciones";
            StackPanel_Posts.Visibility = Visibility.Visible;
            WrapPanel_Followers.Visibility = Visibility.Collapsed;
            WrapPanel_Following.Visibility = Visibility.Collapsed;
        }
    }
}
