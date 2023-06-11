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
    /// Lógica de interacción para PostPreview.xaml
    /// </summary>
    public partial class PostPreview : UserControl
    {
        PostsAPIServices postsAPIServices = new PostsAPIServices();
        public PostPreview()
        {
            InitializeComponent();
        }

        public PostPreview(PostPreview post)
        {
            InitializeComponent();
            Id = post.postPreview.Id;
            Username = post.postPreview.Username;
            ProfilePhoto = post.postPreview.ProfilePhoto;
            PostDate = post.postPreview.PostDate;
            Title = post.postPreview.Title;
            Body = post.postPreview.Body;
            Likes = post.postPreview.Likes;
            Dislikes = post.postPreview.Dislikes;
            Target = post.postPreview.Target;
            LikeStatus = false;
            DislikeStatus = false;
        }

        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(string), typeof(PostPreview));

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(PostPreview));

        public ImageSource ProfilePhoto
        {
            get { return (ImageSource)GetValue(ProfilePhotoProperty); }
            set { SetValue(ProfilePhotoProperty, value); }
        }

        public static readonly DependencyProperty ProfilePhotoProperty = DependencyProperty.Register("ProfilePhoto", typeof(ImageSource), typeof(PostPreview));

        public DateTime PostDate
        {
            get { return (DateTime)GetValue(PostDateProperty); }
            set { SetValue(PostDateProperty, value); }
        }
        public static readonly DependencyProperty PostDateProperty = DependencyProperty.Register("PostDate", typeof(DateTime), typeof(PostPreview));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PostPreview));

        public string Body
        {
            get { return (string)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.Register("Body", typeof(string), typeof(PostPreview));
        public int Likes
        {
            get { return (int)GetValue(LikesProperty); }
            set { SetValue(LikesProperty, value); }
        }

        public static readonly DependencyProperty LikesProperty =
            DependencyProperty.Register("Likes", typeof(int), typeof(PostPreview));
        public int Dislikes
        {
            get { return (int)GetValue(DislikesProperty); }
            set { SetValue(DislikesProperty, value); }
        }

        public static readonly DependencyProperty DislikesProperty =
            DependencyProperty.Register("Dislikes", typeof(int), typeof(PostPreview));
        public string Target
        {
            get { return (string)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(string), typeof(PostPreview));

        public ImageSource PostMainPhoto
        {
            get { return (ImageSource)GetValue(PostMainPhotoProperty); }
            set { SetValue(PostMainPhotoProperty, value); }
        }

        public static readonly DependencyProperty PostMainPhotoProperty = DependencyProperty.Register("PostMainPhoto", typeof(ImageSource), typeof(PostPreview));
        public bool LikeStatus
        {
            get { return (bool)GetValue(LikesSatusProperty); }
            set { SetValue(LikesSatusProperty, value); }
        }

        public static readonly DependencyProperty LikesSatusProperty = DependencyProperty.Register("LikeStatus", typeof(bool), typeof(PostPreview));
        public bool DislikeStatus
        {
            get { return (bool)GetValue(DislikesSatusProperty); }
            set { SetValue(DislikesSatusProperty, value); }
        }

        public static readonly DependencyProperty DislikesSatusProperty = DependencyProperty.Register("DislikesSatus", typeof(bool), typeof(PostPreview));
        public Visibility ThisVisibility
        {
            get { return (Visibility)GetValue(ThisVisibilityProperty); }
            set { SetValue(ThisVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ThisVisibilityProperty = DependencyProperty.Register("ThisVisibility", typeof(Visibility), typeof(PostPreview));

        public void ReportThisPost(Object sender, RoutedEventArgs args)
        {
            ReportPost_report.Tag = Id;
            ReportPost_report.Visibility = Visibility.Visible;
        }
        public async void LikePost(Object sender, RoutedEventArgs args)
        {
            if (this.LikeStatus == false)
            {
                this.Likes++;
                HttpResponseMessage response = await postsAPIServices.AddLike(this.Id);
                if (response.IsSuccessStatusCode)
                {
                    string hexColor = "#093660";
                    Color color = (Color)ColorConverter.ConvertFromString(hexColor);
                    Brush brush = new SolidColorBrush(color);
                    Button_Like.Background = brush;
                    this.LikeStatus = true;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.ThisVisibility = Visibility.Collapsed;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("No se pudo agregar el me gusta publicación inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                this.Likes--;
                HttpResponseMessage response = await postsAPIServices.RemoveLike(this.Id);
                if (response.IsSuccessStatusCode)
                {
                    string hexColor = "#094e60";
                    Color color = (Color)ColorConverter.ConvertFromString(hexColor);
                    Brush brush = new SolidColorBrush(color);
                    Button_Like.Background = brush;
                    this.LikeStatus = false;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.ThisVisibility = Visibility.Collapsed;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("No se pudo eliminar el me gusta de esta publicación inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public async void DislikePost(Object sender, RoutedEventArgs args)
        {
            if (this.DislikeStatus == false)
            {
                this.Dislikes++;
                HttpResponseMessage response = await postsAPIServices.AddDislike(this.Id);
                if (response.IsSuccessStatusCode)
                {
                    string hexColor = "#093660";
                    Color color = (Color)ColorConverter.ConvertFromString(hexColor);
                    Brush brush = new SolidColorBrush(color);
                    Button_Dislike.Background = brush;
                    this.DislikeStatus = true;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.ThisVisibility = Visibility.Collapsed;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("No se pudo agregar el no me gusta a esta publicación inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                this.Dislikes--;
                HttpResponseMessage response = await postsAPIServices.RemoveDislike(this.Id);
                if (response.IsSuccessStatusCode)
                {
                    string hexColor = "#094e60";
                    Color color = (Color)ColorConverter.ConvertFromString(hexColor);
                    Brush brush = new SolidColorBrush(color);
                    Button_Dislike.Background = brush;
                    this.DislikeStatus = false;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Su sesión expiró, vuelve a iniciar sesión", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    SingletonUser.Instance.BorrarSinglenton();
                    this.ThisVisibility = Visibility.Collapsed;
                }
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("No se pudo eliminar el no me gusta publicación inténtalo más tarde", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
