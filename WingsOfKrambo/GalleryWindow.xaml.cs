using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace WingsOfKrambo
{
    /// <summary>
    /// Interaction logic for GalleryWindow.xaml
    /// </summary>
    public partial class GalleryWindow : Window
    {
        public GalleryWindow()
        {
            InitializeComponent();
            LoadImages(); // קריאה לפונקציה שטוענת את התמונות ברגע שהחלון נפתח
        }

        private void LoadImages()
        {
            try
            {
                // הנתיב לתיקייה שבה שמרנו את התמונות
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ActivityImages");

                if (Directory.Exists(folderPath))
                {
                    // חיפוש קבצים עם סיומות של תמונות
                    string[] files = Directory.GetFiles(folderPath, "*.*");
                    List<string> imagePaths = new List<string>();

                    foreach (string file in files)
                    {
                        string ext = Path.GetExtension(file).ToLower();
                        if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                        {
                            imagePaths.Add(file);
                        }
                    }

                    // קישור רשימת הנתיבים ל-ItemsControl שב-XAML
                    ImagesItemsControl.ItemsSource = imagePaths;
                }
                else
                {
                    MessageBox.Show("עדיין לא הועלו תמונות לפעולה זו.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בטעינת התמונות: " + ex.Message);
            }
        }
    }
}
