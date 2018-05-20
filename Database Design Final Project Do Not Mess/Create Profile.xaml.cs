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
using System.Windows.Shapes;

namespace Database_Design_Final_Project_Do_Not_Mess
{
    /// <summary>
    /// Interaction logic for Create_Profile.xaml
    /// </summary>
    public partial class Create_Profile : Window
    {
        private userNameProcessor userProcessor;
        public Create_Profile(userNameProcessor userProcessor)
        {
            InitializeComponent();
            this.userProcessor = userProcessor;
        }

        private void createUser_Click(object sender, RoutedEventArgs e)
        {
            Tuple<bool, string> result = userProcessor.insertProfile(userName.Text, password.SecurePassword);
            if (!result.Item1)
            {
                ExceptionMessage popUp = new ExceptionMessage(result.Item2, "Failed to create user");
                popUp.ShowDialog();
            }
            else
            {
                if(userProcessor.checkProfile(userName.Text, password.SecurePassword).Item1) {
                    DiscogsMain discogsWin = new DiscogsMain(userProcessor.getClient());
                    discogsWin.Show();
                    this.Close();
                    return;
                }
            }
        }
    }
}
