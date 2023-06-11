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
    /// Lógica de interacción para Comment.xaml
    /// </summary>
    public partial class Comment : UserControl
    {
        public Comment()
        {
            InitializeComponent();
            ThisVisibility = Visibility.Collapsed;
        }

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(Comment));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(Comment));

        public string Body
        {
            get { return (string)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        public static readonly DependencyProperty BodyProperty = DependencyProperty.Register("Body", typeof(string), typeof(Comment));

        public DateTime DateOfComment
        {
            get { return (DateTime)GetValue(DateOfCommentProperty); }
            set { SetValue(DateOfCommentProperty, value); }
        }
        public static readonly DependencyProperty DateOfCommentProperty = DependencyProperty.Register("DateOfComment", typeof(DateTime), typeof(Comment));
        public Visibility ThisVisibility
        {
            get { return (Visibility)GetValue(ThisVisibilityProperty); }
            set { SetValue(ThisVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ThisVisibilityProperty = DependencyProperty.Register("ThisVisibility", typeof(Visibility), typeof(Comment));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThisVisibility = Visibility.Collapsed;
            TextBox_Comment.IsEnabled = false;
        }

        private void Button_SaveChages_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Guardar cambios del comentario con id " + ((sender as Button).Tag as Models.Comment).commentId);
        }
    }
}
