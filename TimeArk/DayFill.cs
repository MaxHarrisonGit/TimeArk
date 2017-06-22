using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeArk
{
    class DayFill
    {

        public List<Week> FillWeekList(DataTable DT)
        {
            List<Week> weeks = new List<Week>();
            foreach (DataRow row in DT.Rows)
            {
                weeks.Add(FillWeek(row));
            }
            return weeks;
        }

        public Week FillWeek(DataRow row)
        {
            Week week = new Week();
            int i = 1;
            Day day = FillDay(row[i].ToString(), row[i + 1].ToString(), row[i + 2].ToString());
            week.Sun = day;
            i = i + 3;
            day = FillDay(row[i].ToString(), row[i + 1].ToString(), row[i + 2].ToString());
            week.Mon = day;
            i = i + 3;
            day = FillDay(row[i].ToString(), row[i + 1].ToString(), row[i + 2].ToString());
            week.Tue = day;
            i = i + 3;
            day = FillDay(row[i].ToString(), row[i + 1].ToString(), row[i + 2].ToString());
            week.Wed = day;
            i = i + 3;
            day = FillDay(row[i].ToString(), row[i + 1].ToString(), row[i + 2].ToString());
            week.Thu = day;
            i = i + 3;
            day = FillDay(row[i].ToString(), row[i + 1].ToString(), row[i + 2].ToString());
            week.Fri = day;
            i = i + 3;
            day = FillDay(row[i].ToString(), row[i + 1].ToString(), row[i + 2].ToString());
            week.Sat = day;
            i = i + 3;
            week.Who = row[22].ToString();
            if (row.Table.Columns.Contains("Total"))
            {
                week.Total = (string)row[24];
                week.Date = (DateTime)row[23];
            }
            return week;
        }

        private Day FillDay(string Time, string Local, string Project)
        {
            Day day = new Day()
            {
                Time = Time,
                Local = Local,
                Project = Project,
            };
            return day;
        }

    }
}
