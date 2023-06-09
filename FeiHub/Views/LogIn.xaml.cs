using EmailValidation;
using FeiHub.Models;
using FeiHub.Resources;
using FeiHub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace FeiHub.Views
{
    /// <summary>
    /// Lógica de interacción para LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        public string usernameAcademic = "";
        public string nameAcademic = "";
        public string paternalSurnameAcademic = "";
        public string maternalSurnameAcademic = "";
        public string emailAcademic = "";
        public string passwordAcademic = "";
        public string rolAcademic = "ACADEMIC";
        public string usernameStudent = "";
        public string nameStudent = "";
        public string paternalSurnameStudent = "";
        public string maternalSurnameStudent = "";
        public string emailStudent= "";
        public string passwordStudent = "";
        public string schoolId = "";
        public string educationalProgram = "";
        public string rolStudent = "STUDENT";
        public string usernameLogin = "";
        public string passwordLogin = "";
        UsersAPIServices usersAPIServices = new UsersAPIServices();
        SingletonUser user = SingletonUser.Instance;
        public LogIn()
        {
            InitializeComponent();
        }
        
        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            StackPanel_LogIn.Visibility = Visibility.Collapsed;
            StackPanel_SignIn.Visibility = Visibility.Visible;
        }

        private async void ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            usernameLogin = Username.Text;
            passwordLogin = Password.Password;
            bool withoutFieldsNull = ValidateNullFieldsLogin();
            if (withoutFieldsNull)
            {
                UserCredentials userCredentials = await usersAPIServices.GetUserCredentials(usernameLogin, passwordLogin);
                if (userCredentials.StatusCode == HttpStatusCode.OK)
                {
                    MessageBox.Show($"Bienvenido (a), {userCredentials.username}!", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                    user.Username = userCredentials.username;
                    user.Rol = userCredentials.rol;
                    user.Token = userCredentials.token;
                    this.NavigationService.Navigate(new MainPage());
                }
                else
                {
                    MessageBox.Show("Verifica tus credenciales", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("No puedes dejar campos vacíos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonGoToLogIn_Click(object sender, RoutedEventArgs e)
        {
            StackPanel_LogIn.Visibility = Visibility.Visible;
            StackPanel_SignIn.Visibility = Visibility.Collapsed;
        }

        

        private async void SingInStudent(object sender, RoutedEventArgs e)
        {
            usernameStudent = UsernameStudent.Text;
            nameStudent = NameStudent.Text;
            paternalSurnameStudent = PaternalSurnameStudent.Text;
            maternalSurnameStudent = MaternalStudentSurname.Text;
            passwordStudent = Encryptor.Encrypt(PasswordStudent.Password);
            schoolId = SchoolID.Text;
            educationalProgram = EducationalProgramStudent.Text;
            bool withoutNullFields = ValidateNullFieldsStudents();
            bool correctFields = ValidateFieldsStudent();
            if (withoutNullFields)
            {
                if (correctFields)
                {
                    emailStudent = "z" + SchoolID.Text.Substring(0, 1).ToLower() + SchoolID.Text.Substring(1, 8) + "@estudiantes.uv.mx";
                    Credentials credentialsStudent = new Credentials();
                    credentialsStudent.username = usernameStudent;
                    credentialsStudent.password = passwordStudent;
                    credentialsStudent.email = emailStudent;
                    credentialsStudent.rol = rolStudent;
                    User userStudent = new User();
                    userStudent.username = usernameStudent; 
                    userStudent.name = nameStudent;
                    userStudent.paternalSurname = paternalSurnameStudent;
                    userStudent.maternalSurname = maternalSurnameStudent;
                    userStudent.educationalProgram = educationalProgram;
                    userStudent.schoolId = schoolId;
                    string validateExistingUser = await usersAPIServices.GetExistingUser(emailStudent);
                    if(validateExistingUser != emailStudent)
                    {

                        User validateUsername = await usersAPIServices.GetUser(usernameStudent);
                        if (validateUsername == null)
                        {

                            HttpResponseMessage responseCredentials = await usersAPIServices.CreateCredencials(credentialsStudent);
                            if (responseCredentials.IsSuccessStatusCode)
                            {
                                HttpResponseMessage responseUser = await usersAPIServices.CreateUser(userStudent, rolStudent);
                                if (responseUser.IsSuccessStatusCode)
                                {
                                    MessageBox.Show("Usuario creado exitosamente, inicia sesión para ingresar", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo crear tu usuario, inténtalo más tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No se pudieron crear tus credenciales, inténtalo más tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Nombre de usuario en uso, ingresa otro nombre de usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ya tienes una cuenta en este sistema, ve al inicio de sesión e ingresa tus credenciales de acceso", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Verifica los datos proporcionados", "Notificación", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No puedes dejar campos vacíos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        

        private async void SingInAcademic(object sender, RoutedEventArgs e)
        {
            usernameAcademic = UsernameAcademic.Text;
            nameAcademic = NameAcademic.Text;
            paternalSurnameAcademic = PaternalSurnameAcademic.Text;
            maternalSurnameAcademic = MaternalSurnameAcademic.Text;
            emailAcademic = EmailAcademic.Text;
            passwordAcademic = Encryptor.Encrypt(PasswordAcademic.Password);
            bool withoutNullFields = ValidateNullFieldsAcademic();
            bool correctFields = ValidateFieldsAcademic();
            if (withoutNullFields)
            {
                if (correctFields)
                {
                    Credentials credentialsAcademic = new Credentials();
                    credentialsAcademic.username = usernameAcademic;
                    credentialsAcademic.password = passwordAcademic;
                    credentialsAcademic.email = emailAcademic;
                    credentialsAcademic.rol = rolAcademic;
                    User userAcademic = new User();
                    userAcademic.username = usernameAcademic;
                    userAcademic.name = nameAcademic;
                    userAcademic.paternalSurname = paternalSurnameAcademic;
                    userAcademic.maternalSurname = maternalSurnameAcademic;
                    string validateExistingUser = await usersAPIServices.GetExistingUser(emailAcademic);
                    if (validateExistingUser != emailAcademic)
                    {

                        User validateUsername = await usersAPIServices.GetUser(usernameAcademic);
                        if (validateUsername == null) 
                        {
                        
                            HttpResponseMessage responseCredentials = await usersAPIServices.CreateCredencials(credentialsAcademic);
                            if (responseCredentials.IsSuccessStatusCode)
                            {
                                HttpResponseMessage responseUser = await usersAPIServices.CreateUser(userAcademic, rolAcademic);
                                if (responseUser.IsSuccessStatusCode)
                                {
                                    MessageBox.Show("Usuario creado exitosamente, inicia sesión para ingresar", "Notificación", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo crear tu usuario, inténtalo más tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No se pudieron crear tus credenciales, inténtalo más tarde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Nombre de usuario en uso, ingresa otro nombre de usuario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ya tienes una cuenta en este sistema, ve al inicio de sesión e ingresa tus credenciales de acceso", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Verifica los datos proporcionados", "Notificación", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No puedes dejar campos vacíos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool ValidateSpecialCharacter(string verify)
        {
            bool withoutSpecialCharacter = true;
            string specialCharacteres = "*#+-_;.@%&/()=!?¿¡{}[]^<>";
            foreach (char character in verify)
            {
                if (character >= '0' && character <= '9')
                {
                    withoutSpecialCharacter = false;
                }
            }
            foreach (char character in verify)
            {
                foreach (char specialCharacter in specialCharacteres)
                {
                    if (character == specialCharacter)
                    {
                        withoutSpecialCharacter = true;
                    }

                }

                if (withoutSpecialCharacter)
                {
                    break;
                }
            }
            return withoutSpecialCharacter;
        }
        public bool ValidateFieldsStudent()
        {
            bool correctField = false;
            correctField = schoolId.Length == 9;
            correctField = correctField && schoolId.Substring(0, 1) == "S";
            correctField = correctField && ValidateSpecialCharacter(nameStudent);
            correctField = correctField && ValidateSpecialCharacter(paternalSurnameStudent);
            correctField = correctField && ValidateSpecialCharacter(maternalSurnameStudent);
            return correctField;
        }
        public bool ValidateFieldsAcademic()
        {
            bool correctField = false;
            correctField = EmailValidator.Validate(emailAcademic);
            correctField = correctField && ValidateSpecialCharacter(nameAcademic);
            correctField = correctField && ValidateSpecialCharacter(paternalSurnameAcademic);
            correctField = correctField && ValidateSpecialCharacter(maternalSurnameAcademic);
            correctField = correctField && emailAcademic.EndsWith("@uv.mx", StringComparison.OrdinalIgnoreCase);
            return correctField;
        }
        public bool ValidateNullFieldsAcademic()
        {
            bool fullFields = false;
            if (!String.IsNullOrWhiteSpace(nameAcademic) && !String.IsNullOrWhiteSpace(paternalSurnameAcademic) && !String.IsNullOrWhiteSpace(maternalSurnameAcademic) && !String.IsNullOrWhiteSpace(emailAcademic) && !String.IsNullOrWhiteSpace(passwordAcademic) && !String.IsNullOrWhiteSpace(usernameAcademic))
            {
                fullFields = true;
            }
            return  fullFields;
        }
        public bool ValidateNullFieldsStudents()
        {
            bool fullFields = false;
            if (!String.IsNullOrWhiteSpace(nameStudent) && !String.IsNullOrWhiteSpace(paternalSurnameStudent) && !String.IsNullOrWhiteSpace(maternalSurnameStudent) && !String.IsNullOrWhiteSpace(schoolId) && !String.IsNullOrWhiteSpace(passwordStudent) && !String.IsNullOrWhiteSpace(educationalProgram))
            {
                fullFields = true;
            }
            return fullFields;
        }
        public bool ValidateNullFieldsLogin()
        {
            bool fullFields = false;
            if (!String.IsNullOrWhiteSpace(passwordLogin) && !String.IsNullOrWhiteSpace(usernameLogin))
            {
                fullFields = true;
            }
            return fullFields;
        }
    }
}
