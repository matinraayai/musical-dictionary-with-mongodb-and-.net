using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Design_Final_Project_Do_Not_Mess.Views
{
    class helperMethods
    {
        public static string listOutput<T>(string header, List<T> list)
        {
            int i = 0;
            StringBuilder output = new StringBuilder(header + " ");
            foreach (T element in list)
            {
                output.Append(element.ToString());
                if (i != list.Count - 1)
                {
                    output.Append(", ");
                }
                i++;
            }
            return output.ToString();
        }
    }
}
