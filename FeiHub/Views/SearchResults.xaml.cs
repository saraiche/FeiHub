﻿using FeiHub.Models;
using FeiHub.Services;
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
    /// Lógica de interacción para SearchResults.xaml
    /// </summary>
    public partial class SearchResults : Page
    {
        List<User> usersObtained = new List<User>();
        UsersAPIServices usersAPIServices = new UsersAPIServices();
        string usernameToFind = "";
        public SearchResults(string username)
        {
            InitializeComponent();
            this.MainBar.Button_GoBack.Click += GoBack;
            this.MainBar.Button_Search.Click += FindUsers;
            this.MainBar.Button_Profile.Click += GoToMyProfile;
            this.MainBar.Button_LogOut.Click += LogOut;
            usernameToFind = username;
            AddUsersFind(usernameToFind);
        }

        private void FindUsers(object sender, RoutedEventArgs e)
        {
            WrapPanel_Users.Children.Clear();
            usernameToFind = MainBar.TextBox_Search.Text;
            if (usernameToFind != "")
            {
                AddUsersFind(usernameToFind);
            }
            
        }
        public async void AddUsersFind(string usernameFind)
        {
            usersObtained = await usersAPIServices.FindUsers(usernameFind);
            if (usersObtained.Count > 0)
            {
                if (usersObtained[0].StatusCode == System.Net.HttpStatusCode.OK)
                {

                    foreach (User user in usersObtained)
                    {
                        UserControls.PreviewUser users = new UserControls.PreviewUser();
                        users.previewUser.Username = user.username;
                        if (user.profilePhoto == null)
                        {
                            ImageSourceConverter converter = new ImageSourceConverter();
                            users.previewUser.Source = (ImageSource)converter.ConvertFromString("../../Resources/usuario.png");
                        }
                        else
                        {
                            users.previewUser.Source = new BitmapImage(new Uri(user.profilePhoto));
                        }
                        users.Margin = new Thickness(10);
                        WrapPanel_Users.Children.Add(users);
                        users.previewUser.TextBlock_Username.Tag = user;
                        users.previewUser.TextBlock_Username.MouseDown += GoToProfile;
                        users.previewUser.Button_SendMessage.Tag = user;
                        users.previewUser.Button_SendMessage.Click += GoToChat;
                    }
                }
                if (usersObtained[0].StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {

                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.NavigationService.Navigate(new LogIn());
                }
                if (usersObtained[0].StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {

                    MessageBox.Show("Tuvimos un error al obtener a quiénes sigues, inténtalo más tarde", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Label labelWithoutFollowings = new Label();
                labelWithoutFollowings.Content = "Sigue a tus amigos para verlos aquí";
                StackPanel_Users.Children.Add(labelWithoutFollowings);
            }


        }
        private void GoToProfile(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                var user = textBlock.Tag as User;
                if (user != null)
                {
                    this.NavigationService.Navigate(new Profile(user));
                }
            }
        }
        private void GoToMyProfile(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Profile(SingletonUser.Instance.Username));
        }
        private void GoToChat(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var user = button.Tag as User;
                if (user != null)
                {
                    this.NavigationService.Navigate(new Chat(user));
                }
            }
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage());
        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
            SingletonUser.Instance.BorrarSinglenton();
            this.NavigationService.Navigate(new LogIn());
        }
    }
}
