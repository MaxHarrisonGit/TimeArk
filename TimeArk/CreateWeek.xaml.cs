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
    /// Interaction logic for CreateWeek.xaml
    /// </summary>
    public partial class CreateWeek : Window
    {
        public CreateWeek()
        {
            InitializeComponent();
            FillDropDowns();
        }



        private void FillDropDowns()
        {
            List<string> ComboBoxS = new List<string>() { "Acton Gate", "Milton Keynes", "Out of Office", "Other" };
            SundayLocal.ItemsSource = ComboBoxS;
            MondayLocal.ItemsSource = ComboBoxS;
            TuesdayLocal.ItemsSource = ComboBoxS;
            WednesdayLocal.ItemsSource = ComboBoxS;
            ThursdayLocal.ItemsSource = ComboBoxS;
            FridayLocal.ItemsSource = ComboBoxS;
            SaturdayLocal.ItemsSource = ComboBoxS;
        }

        AutomationManagementSystem.AcDataBase Data = new AutomationManagementSystem.AcDataBase();
        Dates Date = new Dates();
        DayFill dayfill = new DayFill();
        VBScriptCode VB = new VBScriptCode();

        private void DatePick_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (DatePick.Text != "")
            {
                DatePick.Text = Date.StartOfWeek(Convert.ToDateTime(DatePick.Text), 0).ToString();
                if (UpdateCheck.IsChecked == false)
                {
                    Fill("Select * From Week where [User] = '" + VB.WHO() + "' and [WeekDate] = DateValue ('" + Date.StartOfWeek(Convert.ToDateTime(DatePick.Text), 0).ToString("yyyy/MM/dd") + "')");
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            bool Done = Update(1);
            if(Done)
                MessageBox.Show("Plan Saved as AutoFill");
        }

        private bool Update(int Table)
        {
            bool Done = false;
            if (DatePick.Text != "" || Table == 1)
            {
                List<string> Values = Construct(Table);
                if (Values[23] != null)
                {
                    Done = true;
                    string com = "";
                    if (Table == 0)
                    {
                        List<string> Result = Data.get("Select * From Week where [User] = '" + VB.WHO() + "' and [WeekDate] = DateValue ('" + Date.StartOfWeek(Convert.ToDateTime(DatePick.Text), 0).ToString("yyyy/MM/dd") + "')").Rows.OfType<DataRow>().Select(dr => dr.Field<int>("ID").ToString()).ToList<string>();
                        if (Result.Count > 0)
                        {
                            com = "Update Week Set ";
                            com = com + "[SundayShift] = @1,[SundayLocal] = @2,[SundayProject] = @3";
                            com = com + ",[MondayShift] = @4,[MondayLocal] = @5,[MondayProject] = @6";
                            com = com + ",[TuesdayShift] = @7,[TuesdayLocal] = @8,[TuesdayProject] = @9";
                            com = com + ",[WednesdayShift] = @10,[WednesdalLocal] = @11,[WednesdayProject] = @12";
                            com = com + ",[ThursdayShift] = @13,[ThursdayLocal] = @14,[ThursdayProject] = @15";
                            com = com + ",[FridayShift] = @16,[FridayLocal] = @17,[FridayProject] = @18";
                            com = com + ",[SaturdayShift] = @19,[SaturdayLocal] = @20,[SaturdayProject] = @21";
                            com = com + ",[User] = @22, [WeekDate] = @23, [Total] = @24";
                            com = com + " where [ID] = " + Result[0] + "";
                        }
                        else
                        {
                            com = "Insert Into Week (";
                            com = com + "[SundayShift],[SundayLocal],[SundayProject]";
                            com = com + ",[MondayShift],[MondayLocal],[MondayProject]";
                            com = com + ",[TuesdayShift],[TuesdayLocal],[TuesdayProject]";
                            com = com + ",[WednesdayShift],[WednesdalLocal],[WednesdayProject]";
                            com = com + ",[ThursdayShift],[ThursdayLocal],[ThursdayProject]";
                            com = com + ",[FridayShift],[FridayLocal],[FridayProject]";
                            com = com + ",[SaturdayShift],[SaturdayLocal],[SaturdayProject]";
                            com = com + ",[User],[WeekDate],[Total]";
                            com = com + ") Values (";
                            com = com + "@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23,@24)";
                        }
                    }
                    else if (Table == 1)
                    {
                        com = "Update Auto Set ";
                        com = com + "[SundayShift] = @1,[SundayLocal] = @2,[SundayProject] = @3";
                        com = com + ",[MondayShift] = @4,[MondayLocal] = @5,[MondayProject] = @6";
                        com = com + ",[TuesdayShift] = @7,[TuesdayLocal] = @8,[TuesdayProject] = @9";
                        com = com + ",[WednesdayShift] = @10,[WednesdalLocal] = @11,[WednesdayProject] = @12";
                        com = com + ",[ThursdayShift] = @13,[ThursdayLocal] = @14,[ThursdayProject] = @15";
                        com = com + ",[FridayShift] = @16,[FridayLocal] = @17,[FridayProject] = @18";
                        com = com + ",[SaturdayShift] = @19,[SaturdayLocal] = @20,[SaturdayProject] = @21";
                        com = com + " where [User] = '" + Environment.UserName.ToUpper() + "'";
                    }
                    Data.sendin(Values, com);
                }
            }
            else
            {
                MessageBox.Show("Please Select The Date");
                Done = false;
            }
            return Done;
        }

        private string BlankCheck(string Text)
        {
            if (Text == "")
                Text = " ";
            return Text;
        }
        private List<string> Construct(int type)
        {
            List<string> list = new List<string>();

            if (SundayStart.Text != "")
                    list.Add("(" + SundayStart.Text.Replace(" ", "") + " - " + SundayEnd.Text.Replace(" ", "") + ")");
            else
            {
                list.Add("(00:00 - 00:00)");
                SundayStart.Text = "00:00";
                SundayEnd.Text = "00:00";
            }
            list.Add(BlankCheck(SundayLocal.Text));
            list.Add(BlankCheck(SuProject.Text));

            //Monday
            if (MondayStart.Text != "")
                list.Add("(" + MondayStart.Text.Replace(" ", "") + " - " + MondayEnd.Text.Replace(" ", "") + ")");
            else
            {
                list.Add("(00:00 - 00:00)");
                MondayStart.Text = "00:00";
                MondayEnd.Text = "00:00";
            }
            list.Add(BlankCheck(MondayLocal.Text));
            list.Add(BlankCheck(MoProject.Text));

            //Tuesday
            if (TuesdayStart.Text != "")
                list.Add("(" + TuesdayStart.Text.Replace(" ", "") + " - " + TuesdayEnd.Text.Replace(" ", "") + ")");
            else
            {
                list.Add("(00:00 - 00:00)");
                TuesdayStart.Text = "00:00";
                TuesdayEnd.Text = "00:00";
            }
            list.Add(BlankCheck(TuesdayLocal.Text));
            list.Add(BlankCheck(TuProject.Text));

            //Wednesday
            if (WednesdayStart.Text != "")
                list.Add("(" + WednesdayStart.Text.Replace(" ", "") + " - " + WednesdayEnd.Text.Replace(" ", "") + ")");
            else
            {
                list.Add("(00:00 - 00:00)");
                WednesdayStart.Text = "00:00";
                WednesdayEnd.Text = "00:00";
            }
            list.Add(BlankCheck(WednesdayLocal.Text));
            list.Add(BlankCheck(WedProject.Text));

            //Thursday
            if (ThursdayStart.Text != "")
                list.Add("(" + ThursdayStart.Text.Replace(" ", "") + " - " + ThursdayEnd.Text.Replace(" ", "") + ")");
            else
            {
                list.Add("(00:00 - 00:00)");
                ThursdayStart.Text = "00:00";
                ThursdayEnd.Text = "00:00";
            }
            list.Add(BlankCheck(ThursdayLocal.Text));
            list.Add(BlankCheck(ThProject.Text));

            //Friday
            if (FridayStart.Text != "")
                list.Add("(" + FridayStart.Text.Replace(" ", "") + " - " + FridayEnd.Text.Replace(" ", "") + ")");
            else
            {
                list.Add("(00:00 - 00:00)");
                FridayStart.Text = "00:00";
                FridayEnd.Text = "00:00";
            }
            list.Add(BlankCheck(FridayLocal.Text));
            list.Add(BlankCheck(FrProject.Text));

            //Saturday
            if (SaturdayStart.Text != "")
                list.Add("(" + SaturdayStart.Text.Replace(" ", "") + " - " + SaturdayEnd.Text.Replace(" ", "") + ")");
            else
            {
                list.Add("(00:00 - 00:00)");
                SaturdayStart.Text = "00:00";
                SaturdayEnd.Text = "00:00";
            }
            list.Add(BlankCheck(SaturdayLocal.Text));
            list.Add(BlankCheck(SatProject.Text));
            //if (type == 0)
            //{
                list.Add(VB.WHO());
                list.Add(Date.StartOfWeek(Convert.ToDateTime(DatePick.Text), 0).ToString("yyyy/MM/dd"));
                list.Add(Total());
            //}

            return list;
        }

        private string Total()
        {
            int Hours = 0;
            int Miniutes = 0;
            try
            {
                TimeSpan SuT = DateTime.Parse(SundayEnd.Text).Subtract(Convert.ToDateTime(SundayStart.Text));
                TimeSpan MoT = DateTime.Parse(MondayEnd.Text).Subtract(Convert.ToDateTime(MondayStart.Text));
                TimeSpan TuT = DateTime.Parse(TuesdayEnd.Text).Subtract(Convert.ToDateTime(TuesdayStart.Text));
                TimeSpan WeT = DateTime.Parse(WednesdayEnd.Text).Subtract(Convert.ToDateTime(WednesdayStart.Text));
                TimeSpan ThT = DateTime.Parse(ThursdayEnd.Text).Subtract(Convert.ToDateTime(ThursdayStart.Text));
                TimeSpan FrT = DateTime.Parse(FridayEnd.Text).Subtract(Convert.ToDateTime(FridayStart.Text));
                TimeSpan SaT = DateTime.Parse(SaturdayEnd.Text).Subtract(Convert.ToDateTime(SaturdayStart.Text));

                TimeSpan Time = SuT + MoT + TuT + WeT + ThT + FrT + SaT;
                Hours = Time.Hours + (Time.Days * 24);
                Miniutes = Time.Minutes;
                Hours = Hours - 40;
                return Hours.ToString("+00;-00;") + "." + Miniutes.ToString("00");
            }
            catch
            {
                MessageBox.Show("Please Ensure that the Time Formats Are Correct");
                return null;
            }

            
        }

        private void Fill(string command)
        {
            DataTable dt = Data.get(command);
            foreach(DataRow row in dt.Rows)
            {
                Week week = dayfill.FillWeek(row);

                //Sunday
                SundayStart.Text = week.Sun.StartTime;
                SundayEnd.Text = week.Sun.EndTime;
                SundayLocal.Text = week.Sun.Local;

                //Monday
                MondayStart.Text = week.Mon.StartTime;
                MondayEnd.Text = week.Mon.EndTime;
                MondayLocal.Text = week.Mon.Local;

                //Tuesday
                TuesdayStart.Text = week.Tue.StartTime;
                TuesdayEnd.Text = week.Tue.EndTime;
                TuesdayLocal.Text = week.Tue.Local;

                //Wednesday
                WednesdayStart.Text = week.Wed.StartTime;
                WednesdayEnd.Text = week.Wed.EndTime;
                WednesdayLocal.Text = week.Wed.Local;

                //Thursday
                ThursdayStart.Text = week.Thu.StartTime;
                ThursdayEnd.Text = week.Thu.EndTime;
                ThursdayLocal.Text = week.Thu.Local;

                //Friday
                FridayStart.Text = week.Fri.StartTime;
                FridayEnd.Text = week.Fri.EndTime;
                FridayLocal.Text = week.Fri.Local;

                //Saturday
                SaturdayStart.Text = week.Sat.StartTime;
                SaturdayEnd.Text = week.Sat.EndTime;
                SaturdayLocal.Text = week.Sat.Local;
            }
        }

        private void AutoFill_Click(object sender, RoutedEventArgs e)
        {
            Fill("Select * from Auto where [User] = '" + Environment.UserName.ToUpper() + "'");
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            bool Done = Update(0);
            if(Done)
                MessageBox.Show("Your Week has been uploaded");
        }
    }
}
