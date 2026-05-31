using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WingsOfKrambo
{
    public partial class ApprenticeAction : Page
    {
        private dynamic _currentApprentice;

        public ApprenticeAction()
        {
            InitializeComponent();
            SetNextTuesday(); // מגדיר את התאריך אוטומטית
        }

        public ApprenticeAction(dynamic currentApprentice) : this()
        {
            _currentApprentice = currentApprentice;
        }

        private void SetNextTuesday()
        {
            DateTime today = DateTime.Today;
            // מוצא את יום שלישי הקרוב (שלישי = 2 ב-DayOfWeek)
            int daysUntilTuesday = ((int)DayOfWeek.Tuesday - (int)today.DayOfWeek + 7) % 7;
            if (daysUntilTuesday == 0) daysUntilTuesday = 7;

            DateTime nextTuesday = today.AddDays(daysUntilTuesday);
            TxtAutoDate.Text = nextTuesday.ToString("dd/MM/yyyy");
            //runDateInCheck.Text = TxtAutoDate.Text;
        }


        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // 1. בדיקה שנתוני החניך קיימים כדי למנוע קריסה
            if (_currentApprentice == null)
            {
                MessageBox.Show("שגיאה: לא נמצאו נתוני חניך מחובר.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // 2. שליפת הנתונים לפי השמות המדויקים בטבלה שלך
                string firstName = _currentApprentice.FirstName;
                string lastName = _currentApprentice.LastName;

                // כאן השתמשנו בשם המדויק שנתת: idPerson
                string apprenticeId = _currentApprentice.IdPerson.ToString();

                // 3. איסוף התשובה מהממשק (ה-RadioButtons והתאריך)
                string attendanceStatus = RbComing.IsChecked == true ? "מגיע/ה" : "לא מגיע/ה";
                string activityDate = TxtAutoDate.Text;

                // 4. בניית הדיווח להצגה/שליחה
                //string report = $"דיווח נוכחות חדש:\n\n" +
                //                $"שם מלא: {firstName} {lastName}\n" +
                //                $"תעודת זהות: {apprenticeId}\n" +
                //                $"סטטוס: {attendanceStatus}\n" +
                //                $"לתאריך: {activityDate}";

                ////הצגת אישור למשתמש
                //MessageBox.Show(/*report *//*,*/ "נשלח למדריך בהצלחה!", MessageBoxButton.OK, MessageBoxImage.Information);

                // 1. בדיקה איזה כפתור רדיו מסומן וקביעת הסטטוס
                string selectedStatus = "לא מולא";
                if (RbComing.IsChecked == true)
                {
                    selectedStatus = "מגיע";
                }
                else if (RbNotComing.IsChecked == true)
                {
                    selectedStatus = "לא מגיע";
                }

                // 2. עדכון הסטטוס של החניך הנוכחי ברשימה הגלובלית
                var existingApprentice = App.GlobalApprentices.FirstOrDefault(a => a.ID == _currentApprentice.ID);

                if (existingApprentice != null)
                {
                    // אם הוא כבר קיים ברשימה, נעדכן לו את הסטטוס
                    existingApprentice.AttendanceStatus = selectedStatus;
                }
                else
                {
                    // אם הוא לא קיים (למשל נרשם עכשיו), נוסיף אותו
                    App.GlobalApprentices.Add(new Apprentice
                    {
                        FirstName = _currentApprentice.FirstName,
                        LastName = _currentApprentice.LastName,
                        ID = _currentApprentice.ID,
                        AttendanceStatus = selectedStatus
                    });
                }

                // 5. חזרה אוטומטית לדף הקודם (דף הבית של החניך)
                if (this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                // אם עדיין יש שגיאה, נציג הודעה מפורטת
                MessageBox.Show("חלה שגיאה בשליפת הנתונים: " + ex.Message, "שגיאה טכנית", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // פונקציות ריקות כדי למנוע שגיאות קומפילציה אם ה-Events מוגדרים ב-XAML
        private void cmbActivityDate_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void TxtIdNumber_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) { }
        private void TxtPhone_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) { }
        private void ChkParentConfirm_Changed(object sender, RoutedEventArgs e) { }
    }
}