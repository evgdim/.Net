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
            AdTmpl tmpl = new AdTmpl(@"LDAP://192.168.8.10/DC=csr,DC=nevexis,DC=com", @"Administrator", @"Mi4igan3");
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
