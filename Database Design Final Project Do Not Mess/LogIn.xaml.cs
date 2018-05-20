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
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Driver;
using System.Security;

namespace Database_Design_Final_Project_Do_Not_Mess
{
    /// <summary>
    /// The Login page is the first thing the user sees. It allows the user to either login or create a user account. It also warns the user
    /// if the database is not found running.
    /// </summary>
    public partial class LogIn : Window
    {
        //private fields:
        private userNameProcessor userProcessor = new userNameProcessor();
        public LogIn()
        { 
            InitializeComponent();
        }

        //Eventhandler for pressing the login button
        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            //Check to see if the profile that the user has entered exists in the database.
            if (!userProcessor.checkConection())
            {
                ExceptionMessage dialogBox = new ExceptionMessage("Could not connect to MongoDB. Make sure you have an instance of MongoDB running.", "Not connected");
                dialogBox.ShowDialog();
                return;
            }
            Tuple<bool, string> userCheckResult = userProcessor.checkProfile(userName.Text, password.SecurePassword);
            if (userCheckResult.Item1)
            {
                DiscogsMain discogsWin = new DiscogsMain(userProcessor.getClient());
                discogsWin.Show();
                this.Close();
                return;
            }
            else
            {
                ExceptionMessage dialogBox = new ExceptionMessage("The username/password combination was not found.", "Invalid username/password");
                dialogBox.ShowDialog();
                return;
            }

        }
        //Event Handler for pressing the createUser text
        private void createUser_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void CreateUser_MouseEnter(object sender, MouseEventArgs e)
        {
            CreateUser.Foreground = Brushes.Blue;
            CreateUser.Cursor = Cursors.Hand;          
        }

        private void CreateUser_MouseLeave(object sender, MouseEventArgs e)
        {
            CreateUser.Foreground = Brushes.Black;
            CreateUser.Cursor = Cursors.Arrow;
        }
        private void CreateUser_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void CreateUser_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Create_Profile createProfileWindow = new Create_Profile(userProcessor);
            createProfileWindow.ShowDialog();
            this.Close();
        }

        private void logInButton_MouseEnter(object sender, MouseEventArgs e)
        {
            logInButton.Cursor = Cursors.Hand;
        }

        private void logInButton_MouseLeave(object sender, MouseEventArgs e)
        {
            logInButton.Cursor = Cursors.Arrow;
        }
    }
}
