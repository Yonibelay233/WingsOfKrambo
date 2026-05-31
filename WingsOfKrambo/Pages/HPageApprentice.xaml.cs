using Microsoft.Win32;
using System;
using System;    // בשביל AppDomain
using System.Collections.Generic;
using System.IO; // בשביל קבצים
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WingsOfKaramboProject;

namespace WingsOfKrambo.Pages
{
    public partial class HPageApprentice : Page
    {
        public static ApprenticeTBL currentApprentice=null; // משתנה סטטי לשמירת האפנטיס הנוכחי
        public HPageApprentice(ApprenticeTBL a)
        {
            InitializeComponent();
            currentApprentice = a;

            if (currentApprentice != null)
            {
                lblWelcome.Text = $"שלום, {currentApprentice.FirstName}!";
            }
        }
       

        private void BtnUploadImages_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "בחר תמונה מהפעולה";
            op.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";

            if (op.ShowDialog() == true)
            {
                try
                {
                    // שימוש בנתיב מלא כדי למנוע את השגיאה שקיבלת
                    string folderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ActivityImages");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = System.IO.Path.GetFileName(op.FileName);
                    string destPath = System.IO.Path.Combine(folderPath, fileName);

                    File.Copy(op.FileName, destPath, true);

                    //MessageBox.Show("התמונה נשמרה בהצלחה!", "הצלחה", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("שגיאה בשמירה: " + ex.Message);
                }
            }
            // אחרי שהקובץ הועתק בהצלחה:
            MessageBoxResult result = MessageBox.Show("התמונה נשמרה! האם תרצה לראות את הגלריה?",
                                                      "הצלחה",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                GalleryWindow gallery = new GalleryWindow();
                gallery.Show(); // פותח את חלון הגלריה
            }
        }

        private void OpenActions(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ApprenticeAction(currentApprentice));
        }
    }
}
