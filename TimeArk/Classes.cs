using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeArk
{
    public class Week
    {
        public List<Day> Days
        {
            get
            {
                return new List<Day>() { Sun, Mon, Tue, Wed, Thu, Fri, Sat };
            }

        }

        public DGWeek DataWeek
        {
            get
            {
                DGWeek Week = new DGWeek();
                Week.User = Who;
                Week.Sunday = Sun.Time + ((char)'\n').ToString() + Sun.Local + ((char)'\n').ToString() + Sun.Project;
                Week.Monday = Mon.Time + ((char)'\n').ToString() + Mon.Local + ((char)'\n').ToString() + Mon.Project;
                Week.Tuesday = Tue.Time + ((char)'\n').ToString() + Tue.Local + ((char)'\n').ToString() + Tue.Project;
                Week.Wednesday = Wed.Time + ((char)'\n').ToString() + Wed.Local + ((char)'\n').ToString() + Wed.Project;
                Week.Thursday = Thu.Time + ((char)'\n').ToString() + Thu.Local + ((char)'\n').ToString() + Thu.Project;
                Week.Friday = Fri.Time + ((char)'\n').ToString() + Fri.Local + ((char)'\n').ToString() + Fri.Project;
                Week.Saturday = Sat.Time + ((char)'\n').ToString() + Sat.Local + ((char)'\n').ToString() + Sat.Project;
                Week.Diffrence = Total;
                return Week;
            }
        }
        public Day Sun { get; set; }
        public Day Mon { get; set; }
        public Day Tue { get; set; }
        public Day Wed { get; set; }
        public Day Thu { get; set; }
        public Day Fri { get; set; }
        public Day Sat { get; set; }
        public DateTime Date { get; set; }
        public string Who { get; set; }
        public string Total { get; set; }
    }

    public class Day
    {
        public string Time { get; set; }
        public string Local { get; set; }
        public string Project { get; set; }
        public string StartTime
        {
            get
            {
                return Time.Substring(1, 5);
            }
        }
        public string EndTime
        {
            get
            {
                return Time.Substring(9, 5);
            }
        }
    }

    public class DGWeek
    {
        public string User { get; set; }
        public string Sunday { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public double TotalHours { get{ return (Convert.ToDouble(TotalHours) + 40); } }
        public string Diffrence { get; set; }
    }

    public class User
    {
        public DataRow FillClassfromDataRow
        {
            set
            {
                ID = value.Field<int>("ID");
                PCUser= value.Field<string>("Users");
                Name = value.Field<string>("AdviserName");
                Email = value.Field<string>("Email");
                STA = value.Field<bool>("STA");
                ProjectView = value.Field<bool>("ProjectView");
                Project = value.Field<string>("Project");
            }
        }
        public List<string> DeconstructClass
        {
            get
            {
                return new List<string>() { PCUser, Name, Email, BoolCon(STA), BoolCon(ProjectView), Project };
            }
        }
        public int ID { get; set; }
        public string PCUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool STA { get; set; }
        public bool ProjectView { get; set; }
        public string Project { get; set; }

        private string BoolCon(bool item)
        {
            if (item == true)
                return "-1";
            else
                return "0";
        }
    }

    public class Timings
    {
        public List<Week> Listing { set; get; }
        public double CurretWeek
        {
            get
            {
                return Convert.ToDouble(Listing.FindAll(o => o.Date == Now).ConvertAll(o => Convert.ToDouble(o.Total)).Sum());
            }
        }
        public double PastWeeks
        {
            get
            {
                return Listing.FindAll(o => o.Date < Now).ConvertAll(o => Convert.ToDouble(o.Total)).Sum();

            }
        }
        public double FutureWeeks
        {
            get
            {
                return Listing.FindAll(o => o.Date > Now).ConvertAll(o => Convert.ToDouble(o.Total)).Sum();
            }
        }
        public double Total
        {
            get
            {
                return Listing.ConvertAll(o => Convert.ToDouble(o.Total)).Sum();
            }
        }
        public DateTime Now { get; set; }
        public string User { get; set; }
    }

}
