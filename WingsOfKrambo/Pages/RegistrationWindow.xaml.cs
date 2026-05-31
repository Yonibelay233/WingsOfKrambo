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
using WingsOfKaramboProject;
using APIClient;



namespace WingsOfKrambo.Pages
{
    public partial class RegistrationWindow : Page
    {
        ApiService api = new ApiService();
      
        List<string> cityStringList = new();
        CityTBList cList=new CityTBList();

        List<string> BranchStringList = new();
        BranchTBList bList = new BranchTBList();

        List<string> GradeStringList = new();
        GradeTBList gList = new GradeTBList();

        List<string> SchoolStringList = new();
        SchoolTBList sList = new SchoolTBList();

        public RegistrationWindow()
        {
            InitializeComponent();
            AllList();
            //הגבלת התאריך בלוח השנה כך שהמשתמש חייב להיות לפחות בן 9
                dpDateOfBirth.DisplayDateEnd = DateTime.Now.AddYears(-9);
        }

        public async void AllList()
        {
            cList = await api.GetAllCities();
            cityStringList = (List<string>)cList.Select(x => x.WhatCity).ToList();
            cityChooser.ItemsSource= cityStringList;

            bList = await api.GetAllBranchs();
            BranchStringList = (List<string>)bList.Select(x => x.NameOfBranch).ToList();
            branchChooser.ItemsSource = BranchStringList;

            gList = await api.GetAllGrades();
            GradeStringList = (List<string>)gList.Select(x => x.WhatGrade).ToList();
            gradeChooser.ItemsSource = GradeStringList;

            sList = await api.GetAllSchools();
            SchoolStringList = (List<string>)sList.Select(x => x.WhatSchool).ToList();
            schoolChooser.ItemsSource = SchoolStringList;
        }

        public async void Insert()
        {
            string fName = txtFirstName.Text;
            string fLastName = txtLastName.Text;
            string fID = txtRegID.Text;
            CityTBL fCity = cList[cityChooser.SelectedIndex];
            BranchTBL fBranch = bList[branchChooser.SelectedIndex];
            string fBuildingNumber = txBuildingNumber.Text;
            GradeTBL fGrade = gList[gradeChooser.SelectedIndex];
            SchoolTBL fSchool = sList[schoolChooser.SelectedIndex];
            //חילוץ המחרוזת מה - DatePicker
            string bDate = dpDateOfBirth.SelectedDate?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd");
            string fStreet = txStreet.Text;
            int rows = 0;
            var role = cmbRole.Items[cmbRole.SelectedIndex].ToString();
            role = role.Remove(0, role.Length - 6);
            if (role == "חניך/ה")
            {
                apprentice.Visibility = Visibility.Visible;

                ApprenticeTBL a = new ApprenticeTBL()
                {
                    BranchCode = bList[branchChooser.SelectedIndex],
                    BuildingNumber = int.Parse(fBuildingNumber),
                    FirstName = fName,
                    LastName = fLastName,
                    City = cList[cityChooser.SelectedIndex],
                    IdPerson = txtRegID.Text,
                    DateOfBirth = DateTime.Parse(bDate),
                    Grade = gList[gradeChooser.SelectedIndex],
                    School = sList[schoolChooser.SelectedIndex],
                    Street = fStreet,

                };

                int ans=await api.InsertAApprentice(a);

            }
            if (role != "חניך/ה")
            {
                role = role.Remove(0, role.Length - 7);

                if (cmbRole.SelectedItem?.ToString() == "מדריך/ה")
                {
                    GuideTBL g = new GuideTBL()
                    {
                        BranchCode = bList[branchChooser.SelectedIndex],
                        BuildingNumber = int.Parse(fBuildingNumber),
                        FirstName = fName,
                        LastName = fLastName,
                        City = cList[cityChooser.SelectedIndex],
                        IdPerson = txtRegID.Text,
                        DateOfBirth = DateTime.Parse(bDate),
                        Street = fStreet,

                    };
                    api.InsertAGuide(g);

                }
                if (role != "מדריך/ה")
                {
                    role = role.Remove(0, role.Length - 9);

                    if (cmbRole.SelectedItem?.ToString() == "איש/ה צוות")
                    {
                        StaffMemberTBL s = new StaffMemberTBL()
                        {
                            BranchCode = bList[branchChooser.SelectedIndex],
                            BuildingNumber = int.Parse(fBuildingNumber),
                            FirstName = fName,
                            LastName = fLastName,
                            City = cList[cityChooser.SelectedIndex],
                            IdPerson = txtRegID.Text,
                            DateOfBirth = DateTime.Parse(bDate),
                            Street = fStreet,

                        };
                        api.InsertAStaffMember(s);

                    }
                    if (role != "איש/ה צוות")
                    {
                        role = role.Remove(0, role.Length - 19);

                        if (cmbRole.SelectedItem?.ToString() == "ילד/ה עם צרכים מיוחדים")
                        {
                            ChildWithSpecialNeedTBL c = new ChildWithSpecialNeedTBL()
                            {
                                BranchCode = bList[branchChooser.SelectedIndex],
                                BuildingNumber = int.Parse(fBuildingNumber),
                                FirstName = fName,
                                LastName = fLastName,
                                City = cList[cityChooser.SelectedIndex],
                                IdPerson = txtRegID.Text,
                                DateOfBirth = DateTime.Parse(bDate),
                                School = sList[schoolChooser.SelectedIndex],
                                Street = fStreet,

                            };
                            api.InsertAChildWithSpecialNeed(c);

                        }
                    }
                }

            }

            // כאן תוסיף את בדיקת התקינות שלך
            MessageBox.Show("נרשמת בהצלחה למשפחת קרמבו!");

            // חזרה למסך הכניסה אחרי ההרשמה
            if (this.NavigationService != null)
            {
                this.NavigationService.GoBack(); // חוזר דף אחד אחורה ללוגין
            }
        }

        private void btnFinishRegister_Click(object sender, RoutedEventArgs e)
        {
            Insert();
        }
    }
}



