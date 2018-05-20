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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Database_Design_Final_Project_Do_Not_Mess.Views
{
    /// <summary>
    /// Interaction logic for MasterListView.xaml
    /// </summary>
    public partial class MasterListView : UserControl
    {
        Master master;
        IMongoDatabase discogsDatabase;

        public MasterListView(Master master, IMongoDatabase discogsDatabase)
        {
            InitializeComponent();
            this.master = master;
            this.discogsDatabase = discogsDatabase;
            Title.Text = master.Title;
            Year.Text = master.Year.ToString();
            Artist.Text = master.Artist;


        }
        //Mouse events for the master name.
        private void Name_MouseEnter(object sender, MouseEventArgs e)
        {
            Title.Foreground = Brushes.Blue;
            Title.Cursor = Cursors.Hand;
        }

        private void Name_MouseLeave(object sender, MouseEventArgs e)
        {
            Title.Foreground = Brushes.Black;
            Title.Cursor = Cursors.Arrow;
        }

        private void Name_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MasterWindowView windowView = new MasterWindowView(master, discogsDatabase);
            windowView.ShowDialog();
        }
    }
}
