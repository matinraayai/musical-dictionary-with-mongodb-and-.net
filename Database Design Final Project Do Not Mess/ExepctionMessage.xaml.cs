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
    /// Interaction logic for ConnectionFaliure.xaml
    /// </summary>
    public partial class ExceptionMessage : Window
    {
        public ExceptionMessage(string errorMsg, string title)
        {
            InitializeComponent();
            this.message.Text = errorMsg;
            this.Title = title;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
