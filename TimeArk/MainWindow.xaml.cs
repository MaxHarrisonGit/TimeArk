using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

namespace TimeArk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

                InitializeComponent();
                When = Date.StartOfWeek(DateTime.Now, 0);
                FillData(When);
                Datepick.Text = When.ToString();
        }
        private static DateTime When = new DateTime();
        private static DataTable Users = new DataTable();
        private static User ME = new User();
        private ObservableCollection<DGWeek> _week = new ObservableCollection<DGWeek>();
        public ObservableCollection<DGWeek> OCWeek
        {
            get { return _week; }
            set { _week = value; }
        }

        private User Findme()
        {
            User Item = new User();
            if (UserClassListUserID.ContainsKey(Environment.UserName.ToUpper()))
            {
                Item = UserClassListUserID[Environment.UserName.ToUpper()];
                if(Item.STA)
                    Config.Visibility = Visibility.Visible;
                else
                    Config.Visibility = Visibility.Hidden;
                if (Item.Name != VB.WHO())
                {
                    Data.sendin(new List<string>() { VB.WHO() }, "Update Users Set [AdviserName] = @1 where [ID] = " + Item.ID);
                }
            }
            else
            {
                MessageBox.Show("You are not in the system");
            }
            return Item;
        }

        Meth Method = new Meth();
        VBScriptCode VB = new VBScriptCode();
        private Dictionary<string, User> UserClassList = new Dictionary<string, User>();
        private Dictionary<string, User> UserClassListUserID = new Dictionary<string, User>();
        AutomationManagementSystem.AcDataBase Data = new AutomationManagementSystem.AcDataBase();
        Dates Date = new Dates();
        DayFill dayfill = new DayFill();
        // ToString("dd/MM/yyyy")
        // ToString("yyyy/MM/dd")
        public void UserFill()
        {
            Users = GetUserList();
            UserClassList = UserObject(Users);
        }


        public Dictionary<string, User> UserObject(DataTable UserDT)
        {
            UserClassListUserID = Method.CreateUserDictionaryPCUser(UserDT);
            return Method.CreateUserDictionary(UserDT);

        }
        public DataTable GetUserList()
        {
            //Dictionary<string, bool> UsersDT = Data.get("select * from Users").Rows.OfType<DataRow>().Select(dr => dr.Field<string>("AdviserName")).ToDictionary(x => x, x => false);
            return Data.get("select * from Users");
        }


        public void FillData(DateTime SearchDate)
        {
            UserFill();
            ME = Findme();
            double Total = 0;
            Dictionary<string, DGWeek> Final = new Dictionary<string, DGWeek>();
            DataTable dt = Data.get("select * from Week where [WeekDate] = DateValue ('" + Date.StartOfWeek(SearchDate, 0).ToString("yyyy/MM/dd") + "')");
            //Dictionary<string, string> UsersDT = Users.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("AdviserName")).ToDictionary(x => x, x => x);
            Dictionary<string, string> Timings = new Dictionary<string, string>();
            Dictionary<string, string> UsersDT = Users.AsEnumerable().ToDictionary(row => row.Field<string>("AdviserName"), row => row.Field<string>("Users"));
            List<Week> Weeks = dayfill.FillWeekList(dt);
            foreach (Week Data in Weeks)
            {
                
                Timings.Add(UsersDT[Data.Who], Data.Total);
                var Test = Data.DataWeek.Diffrence;
                Total = Total + Convert.ToDouble(Data.Total);
                if (UsersDT.ContainsKey(Data.Who))
                {
                    UsersDT.Remove(Data.Who);

                }
                Final.Add(Data.Who, Data.DataWeek);
            }
            foreach(KeyValuePair<string, string> Data in UsersDT)
            {
                DGWeek UserNames = new DGWeek() { User = Data.Key };
                Final.Add(UserNames.User, UserNames);
            }
            var list = Final.Keys.ToList();
            list.Sort();
            //this.dataGrid.ColumnFromDisplayIndex(0).SortDirection = ListSortDirection.Descending;
            OCWeek.Clear();
            foreach (var Key in list)
            {
                if (UserClassList[Key].PCUser == Environment.UserName.ToUpper() || UserClassList[Key].STA == true || ME.STA == true || (UserClassList[Key].Project == ME.Project && ME.ProjectView == true))
                    OCWeek.Add(Final[Key]);
                //else if (UserClassList.ContainsKey(VB.WHO()))
                //    if (UserClassList[VB.WHO()].STA == true || (UserClassList[Key].Project == UserClassList[VB.WHO()].Project && UserClassList[VB.WHO()].ProjectView == true))
                //        OCWeek.Add(Final[Key]);
                //else if (UserClassListUserID.ContainsKey(Environment.UserName.ToUpper()))
                //    {
                //        //Data.sendin(new List<string>(){ VB.WHO() },"Update Users Set [AdviserName] = @1 where [ID] = " + UserClassListUserID[Environment.UserName.ToUpper()].ID);
                //        UserFill();
                //        if (UserClassList[VB.WHO()].STA == true || (UserClassList[Key].Project == UserClassList[VB.WHO()].Project && UserClassList[VB.WHO()].ProjectView == true))
                //            OCWeek.Add(Final[Key]);
                //    }
            }
            Totals.Text = "";
            if (Timings.ContainsKey(Environment.UserName.ToUpper()))
                Totals.Text = "You are working a Total of " + Timings[Environment.UserName.ToUpper()] + " Additional Hours this week" + ((char)'\n').ToString();
            Totals.Text = Totals.Text + "There is a Total of " + Total.ToString("+00.00; -00.00;") + " Additional Hours Being Worked this week";
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
           FillData((When = When.AddDays(7)));
           Datepick.Text = When.ToString();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            FillData((When = When.AddDays(-7)));
            Datepick.Text = When.ToString();
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            FillData((When = Convert.ToDateTime(Datepick.Text)));
            Datepick.Text = Date.StartOfWeek(Convert.ToDateTime(Datepick.Text), 0).ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            CreateWeek Object = new CreateWeek();
            Object.Show();
            //FillData((When = DateTime.Now));
            //Datepick.Text = When.ToString();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
                FillData(When);
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            ConfigWindow Object = new ConfigWindow();
            Object.ShowDialog();
        }
    }
}

