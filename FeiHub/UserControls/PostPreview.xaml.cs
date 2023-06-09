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

        public static readonly DependencyProperty ProfilePhotoProperty = DependencyProperty.Register("ProfilePhotoProperty", typeof(ImageSource), typeof(PostPreview));

        public DateTime PostDate
        {
            get { return (DateTime)GetValue(PostDateProperty); }
            set { SetValue(PostDateProperty, value); }
        }
        public static readonly DependencyProperty PostDateProperty = DependencyProperty.Register("PostDateProperty", typeof(DateTime), typeof(PostPreview));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("TitleProperty", typeof(string), typeof(PostPreview));

        public string Body
        {
            get { return (string)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }
        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.Register("BodyProperty", typeof(string), typeof(PostPreview));

        public ImageSource PostMainPhoto
        {
            get { return (ImageSource)GetValue(PostMainPhotoProperty); }
            set { SetValue(PostMainPhotoProperty, value); }
        }

        public static readonly DependencyProperty PostMainPhotoProperty = DependencyProperty.Register("PostMainPhotoProperty", typeof(ImageSource), typeof(PostPreview));
    }
}
