using System.Collections.Generic;
using System.Windows;

namespace WingsOfKrambo
{
    public partial class App : Application
    {
        // רשימה סטטית שמשותפת לכל המסכים באפליקציה
        public static List<Apprentice> GlobalApprentices { get; set; } = new List<Apprentice>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // חניכים לדוגמה כדי שהמערכת לא תהיה ריקה בהתחלה (סטטוס: לא מולא)
            GlobalApprentices.Add(new Apprentice { FirstName = "אורי", LastName = "מזרחי", ID = "321654987", AttendanceStatus = "לא מולא" });
            GlobalApprentices.Add(new Apprentice { FirstName = "שירה", LastName = "לוי", ID = "123456789", AttendanceStatus = "לא מולא" });
            GlobalApprentices.Add(new Apprentice { FirstName = "תומר", LastName = "כהן", ID = "456789123", AttendanceStatus = "לא מולא" });
        }
    }

    // מחלקת החניך שמייצגת את מבנה הנתונים בהתאם ל-Binding שלך
    public class Apprentice
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ID { get; set; }
        public string AttendanceStatus { get; set; }
    }
}