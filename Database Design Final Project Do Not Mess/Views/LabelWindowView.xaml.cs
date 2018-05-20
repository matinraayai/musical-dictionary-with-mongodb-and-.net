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
    /// Interaction logic for LabelWindowView.xaml
    /// </summary>
    public partial class LabelWindowView : Window
    {
        Label label;
        IMongoDatabase discogsDatabase;
        public LabelWindowView(Label label, IMongoDatabase discogsDatabase)
        {
            InitializeComponent();
            this.label = label;
            this.discogsDatabase = discogsDatabase;
            name.Text = label.Name;
        }
    }
}
