using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace FeiHub.UserControls
{
    /// <summary>
    /// Lógica de interacción para PreviewUser.xaml
    /// </summary>
    public partial class PreviewUser : UserControl
    {
        public PreviewUser()
        {
            InitializeComponent();
        }

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(PreviewUser));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(PreviewUser));

        public Visibility ThisVisibility
        {
            get { return (Visibility)GetValue(ThisVisibilityProperty); }
            set { SetValue(ThisVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ThisVisibilityProperty = DependencyProperty.Register("ThisVisibility", typeof(Visibility), typeof(PreviewUser));
    }
}
