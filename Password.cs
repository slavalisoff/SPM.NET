using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager
{
    public class Password
    {
        public string name;
        public string password;
        public string website;

        public Password(string a, string b, string c)
        {
            name = a;
            password = b;
            website = c;
        }

        public string[] GetLines()
        {
            string[] ret = { name, password, website };
            return ret;
        }
    }
}
