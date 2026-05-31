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

namespace WingsOfKrambo.Pages
{
    /// <summary>
    /// Interaction logic for HomeGuidePage1.xaml
    /// </summary>
    public partial class HomeGuidePage1 : Page
    {
        public HomeGuidePage1()
        {
            InitializeComponent();
        }

        private void AttendanceReport_Click(object sender, RoutedEventArgs e)
        {
            // מעבר לדף שמציג את רשימת כל החניכים
            NavigationService.Navigate(new AttendanceList());
        }
    }
}
