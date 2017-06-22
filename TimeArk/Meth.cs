using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeArk
{
    class Meth
    {
        public Dictionary<string,User> CreateUserDictionary(DataTable DT)
        {
            Dictionary<string, User> Object = new Dictionary<string, User>();
            foreach(DataRow row in DT.Rows)
            {
                User Item = new User() { FillClassfromDataRow = row, };
                Object.Add(Item.Name, Item);
            }

            return Object;
        }

        public List<User> CreateUserList(DataTable DT)
        {
            List<User> Object = new List<User>();
            foreach (DataRow row in DT.Rows)
            {
                User Item = new User() { FillClassfromDataRow = row, };
                Object.Add(Item);
            }

            return Object;
        }

        public Dictionary<string,User> CreateUserDictionaryPCUser(DataTable DT)
        {
            Dictionary<string, User> Object = new Dictionary<string, User>();
            foreach (DataRow row in DT.Rows)
            {
                User Item = new User() { FillClassfromDataRow = row, };
                Object.Add(Item.PCUser, Item);
            }

            return Object;
        }

    }
}
