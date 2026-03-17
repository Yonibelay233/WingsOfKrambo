using APIClient;
using System;
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WingsOfKaramboProject;

namespace WingsOfKrambo.Pages // שים לב לסיומת .LogIn
{
    public partial class LogIn : Page
    {
        ApiService apiService=new ApiService();
        public LogIn()
        {
            InitializeComponent();
            if (WingsOfKrambo.Properties.Settings.Default.IsRemebembered)
            {
                txtFirst.Text = WingsOfKrambo.Properties.Settings.Default.RememberedUser;
                // כאן הקסם:
                txtID.Password = WingsOfKrambo.Properties.Settings.Default.PASS;
                RememberMeCheckBox.IsChecked = true;
            }
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirst.Text; // השם הפרטי (שם המשתמש)
            string idPassword = txtID.Password; // תעודת הזהות (הסיסמה)

            // 1. בדיקה אם השדות ריקים
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(firstName))
            {
                MessageBox.Show("נא למלא את כל השדות");
                return;
            }

            // 2. בדיקת תקינות תעודת זהות (אורך של 9 ספרות ורק מספרים)
            // 1. בדיקה שהשדות לא ריקים
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(idPassword))
            {
                MessageBox.Show("נא למלא גם שם פרטי וגם תעודת זהות");
                return;
            }

            // 2. בדיקה שתעודת הזהות (הסיסמה) תקינה
            if (!ValidateID(idPassword))
            {
                MessageBox.Show("תעודת הזהות שהוזנה אינה תקינה (חייבת להיות 9 ספרות תקניות)");
                return;
            }

            // אם הקוד הגיע לכאן - הכל תקין!

            ChecksumData();
            // כאן אנחנו עוברים לעמוד הבית:

            // בדיקה אם המשתמש סימן שהוא רוצה שנזכור אותו
            //if (RememberMeCheckBox.IsChecked == true)
            //{
            //    WingsOfKrambo.Properties.Settings.Default.RememberedUser = txtFirst.Text;

            //    WingsOfKrambo.Properties.Settings.Default.IsRemebembered = true;
            //}
            //else
            //{
            //    // אם הוא לא סימן, ננקה את הנתונים הישנים
            //    WingsOfKrambo.Properties.Settings.Default.RememberedUser = "";
            //    WingsOfKrambo.Properties.Settings.Default.IsRemebembered = false;
            //}

            // פקודה קריטית: שמירת השינויים לקובץ במחשב
            //WingsOfKrambo.Properties.Settings.Default.Save();
            string currentName = txtFirst.Text;
            string currentID = idPassword;

            // צעד 2: בדיקות תקינות על המשתנים (לא על הפקדים)
            if (string.IsNullOrEmpty(currentName) || string.IsNullOrEmpty(currentID))
            {
                MessageBox.Show("נא למלא שדות");
                return;
            }

            // צעד 3: שמירה ל-Settings
            if (RememberMeCheckBox.IsChecked == true)
            {
                WingsOfKrambo.Properties.Settings.Default.RememberedUser = currentName;
                WingsOfKrambo.Properties.Settings.Default.PASS = idPassword; // השתמש בשם המדויק שהגדרת ב-Properties
                WingsOfKrambo.Properties.Settings.Default.IsRemebembered = true;
            }
            else
            {
                WingsOfKrambo.Properties.Settings.Default.IsRemebembered = false;
                //WingsOfKrambo.Properties.Settings.Default.PASS = "";
            }

            WingsOfKrambo.Properties.Settings.Default.Save();

            // צעד 4: שליחה ל-API (שלח את המשתנים, לא את הפקדים)
           
        }



        private async void ChecksumData()
        {
            GuideTBList guideList = await apiService.GetAllGuides();
            GuideTBL g = guideList.Find(x => x.IdPerson == txtID.Password && x.FirstName == txtFirst.Text);

            ApprenticeTBList apprenticeList = await apiService.GetAllApprentices();
            ApprenticeTBL a = apprenticeList.Find(x => x.IdPerson == txtID.Password && x.FirstName == txtFirst.Text);

            StaffMemberTBList staffMemberList = await apiService.GetAllStaffMembers();
            StaffMemberTBL s = staffMemberList.Find(x => x.IdPerson == txtID.Password && x.FirstName == txtFirst.Text);

            ChildWithSpecialNeedList childWithSpecialNeedList = await apiService.GetAllChildWithSpecialNeeds();
            ChildWithSpecialNeedTBL c = childWithSpecialNeedList.Find(x => x.IdPerson == txtID.Password && x.FirstName == txtFirst.Text);
            if (g != null)
            {
                NavigationService.Navigate(new HomePage());
            }
            if (a != null)
            {
                NavigationService.Navigate(new HPageApprentice());
            }
            if (s != null)
            {
                NavigationService.Navigate(new HomePageStaffMember());
            }
            if (c != null)
            {
                NavigationService.Navigate(new HomePageChildWithSpecialNeeds());
            }
            // בדיקה אם אף אחד מהחיפושים לא החזיר תוצאה
            if (g == null && a == null && s == null && c == null)
            {
                MessageBox.Show("שגיאה: שם משתמש או סיסמה שגויים. במידה ואינך רשום, אנא הירשם במערכת.", "התחברות נכשלה", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // עוצר את המשך הפעולה
            }
        }
        public static bool ValidateID(string id)
        {
            if (id.Length != 9 || !id.All(char.IsDigit)) return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int num = int.Parse(id[i].ToString()) * ((i % 2) + 1);
                sum += num > 9 ? num - 9 : num;
            }
            return sum % 10 == 0;
        }


        // יצירת מופע חדש של חלון הרישום


        // פתיחת החלון
        private void Register_Click(object sender, MouseButtonEventArgs e)
        {
            // במקום ליצור חלון חדש, אנחנו מנווטים לדף הרישום
            this.NavigationService.Navigate(new RegistrationWindow());
        }

        // אופציונלי: אם אתה רוצה לסגור את מסך הכניסה הנוכחי כשנפתח הרישום
        //Window.GetWindow(this)?.Close();


        // פונקציה שמתרחשת כשלוחצים על הכפתור (הצגת הסיסמה)
        private void btnShowPass_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // מעתיקים את הטקסט מהסיסמה לתיבה הגלויה
            txtIDVisible.Text = txtID.Password;

            // מחליפים נראות
            txtID.Visibility = Visibility.Collapsed;
            txtIDVisible.Visibility = Visibility.Visible;
        }

        // פונקציה שמתרחשת כשעוזבים את הכפתור (הסתרת הסיסמה)
        private void btnShowPass_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        // פונקציה למקרה שהעכבר יצא מגבולות הכפתור בזמן לחיצה
        private void btnShowPass_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        private void HidePassword()
        {
            txtIDVisible.Visibility = Visibility.Collapsed;
            txtID.Visibility = Visibility.Visible;
            txtID.Focus(); // מחזיר את הפוקוס לשדה הסיסמה
        }

    }
    }
