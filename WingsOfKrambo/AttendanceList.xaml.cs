using System.Linq;
using System.Windows.Controls;

namespace WingsOfKrambo
{
    public partial class AttendanceList : Page
    {
        public AttendanceList()
        {
            InitializeComponent();

            // טעינת הנתונים והצגת החניכים שמילאו נוכחות
            ShowFilledAttendance();
        }

        private void ShowFilledAttendance()
        {
            // סינון LINQ: לוקחים מהרשימה הגלובלית רק את מי שסימן נוכחות
            var filteredList = App.GlobalApprentices
                .Where(a => a.AttendanceStatus == "מגיע" || a.AttendanceStatus == "לא מגיע")
                .ToList();

            // הצגת הרשימה המסוננת בתוך ה-DataGrid
            dgApprentices.ItemsSource = filteredList;
        }
    }
}