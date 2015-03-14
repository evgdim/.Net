using ActiveDirectoryTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            AdTmpl tmpl = new AdTmpl(@"LDAP://<host>/DC=<a>,DC=<b>,DC=<c>", @"", @"");
            List<User> u = tmpl.Search<User>("(&(objectClass=person)(sAMAccountName=EVGENI))", new HashSet<string> { AdProperties.LOGINNAME },
                        a =>
                        {
                            return new User
                                {
                                    SamAccName = AdTmpl.GetProp(a, AdProperties.LOGINNAME)
                                };
                        });
            Console.Write(u);
        }
    }
}
