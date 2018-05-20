using MongoDB.Driver;
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

namespace Database_Design_Final_Project_Do_Not_Mess.Views
{
    /// <summary>
    /// Interaction logic for MasterWindowView.xaml
    /// </summary>
    public partial class MasterWindowView : Window
    {
        Master master;
        IMongoDatabase discogsDatabase;
        public MasterWindowView(Master master, IMongoDatabase discogsDatabase)
        {
            InitializeComponent();
            this.master = master;
            this.discogsDatabase = discogsDatabase;
            name.Text = master.Title;
            profile.Text = master.DataQuality;
        }
    }
}
