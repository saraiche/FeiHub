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

namespace FeiHub.UserControls
{
    /// <summary>
    /// Lógica de interacción para PostPreview.xaml
    /// </summary>
    public partial class PostPreview : UserControl
    {
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
    }
}
