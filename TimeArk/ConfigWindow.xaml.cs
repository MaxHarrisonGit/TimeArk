using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace TimeArk
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
            Fill();
            UserFill();
        }

        private ObservableCollection<Timings> _times = new ObservableCollection<Timings>();
        public ObservableCollection<Timings> Times
        {
            get { return _times; }
            set { _times = value; }
        }

        private ObservableCollection<User> _UserL = new ObservableCollection<User>();
        public ObservableCollection<User> UserL
        {
            get { return _UserL; }
            set { _UserL = value; }
        }

        Meth meth = new Meth();
        AutomationManagementSystem.AcDataBase Data = new AutomationManagementSystem.AcDataBase();

        private void Fill()
        {
            List<User> Object = meth.CreateUserList(Data.get("select * from Users")).OrderBy(o => o.Name).ToList();
            foreach (User user in Object)
                UserL.Add(user);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string com = "Update Users set [Users] = @1, [AdviserName] = @2, [Email] = @3, [STA] = @4, [ProjectView] = @5, [Project] = @6 Where [ID] = ";
            foreach (User item in UserL) 
                Data.sendin(item.DeconstructClass, com + item.ID);
            MessageBox.Show("Changes have been saved");
        }



        private void Select_Click(object sender, RoutedEventArgs e)
        {
            StartD.IsEnabled = true;
            EndD.IsEnabled = true;
            label.IsEnabled = true;
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            StartD.IsEnabled = false;
            EndD.IsEnabled = false;
            label.IsEnabled = false;
            FillTimes("Select * from Week");
        }
        Dates Date = new Dates();
        DayFill DFill = new DayFill();
        private Dictionary<string, User> UserClassList = new Dictionary<string, User>();

        public void UserFill()
        {
            Users = GetUserList();
            UserClassList = UserObject(Users);
        }


        Meth Method = new Meth();
        public Dictionary<string, User> UserObject(DataTable UserDT)
        {
            return Method.CreateUserDictionary(UserDT);

        }
        public DataTable GetUserList()
        {
            return Data.get("select * from Users");
        }
        private static DataTable Users = new DataTable();



        private void FillTimes(string com)
        {
            Times.Clear();
            List<Week> Item = DFill.FillWeekList(Data.get(com));
            List<string> UserNames = new List<string>((UserClassList.Keys.ToList()));
            UserNames.Sort();
            foreach (string Users in UserNames)
            {
                Timings Time = new Timings() { Listing = Item.FindAll(o => o.Who == Users), Now = Date.StartOfWeek(DateTime.Now, 0), User = Users};
                Times.Add(Time);
            }
        }

        private void StartD_CalendarClosed(object sender, RoutedEventArgs e)
        {

            if(StartD.Text != "")
                StartD.Text = Date.StartOfWeek(Convert.ToDateTime(StartD.Text), 0).ToString();
            if (EndD.Text != "")
                EndD.Text = Date.StartOfWeek(Convert.ToDateTime(EndD.Text), 0).ToString();

            if (StartD.Text != "" && EndD.Text != "")
            {
                FillTimes("Select * from Week where [WeekDate] BETWEEN DateValue ('" + Convert.ToDateTime(StartD.Text).ToString("yyyy/MM/dd") + "') AND DateValue ('" +Convert.ToDateTime(EndD.Text).ToString("yyyy/MM/dd") + "')");
            }
            
        }
    }
}
