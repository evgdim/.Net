using Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Utils;
using IBM.WMQ;
namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //AdTmpl tmpl = new AdTmpl(@"LDAP://192.168.8.10/DC=csr,DC=nevexis,DC=com", @"Administrator", @"Mi4igan3");
            //List<User> u = tmpl.Search<User>("(&(objectClass=person)(sAMAccountName=EVGENI))", new HashSet<string> { AdProperties.LOGINNAME },
            //            a =>
            //            {
            //                return new User
            //                    {
            //                        SamAccName = AdTmpl.GetProp(a, AdProperties.LOGINNAME)
            //                    };
            //            });
            //Console.Write(u);

            //DbTmpl db = new DbTmpl(@"Data Source=.\SQLEXPRESS; Database=dskclaims-mgr; Integrated Security=True;");
            //List<string> names = db.ExecuteSelect("select DisplayName from ADUsers", null, r => r.GetString(0));
            //Console.Write(names);
            
            MqTmpl mq = new MqTmpl("localhost", "DCARDCH", 1415, "DCARD");
            mq.WaitInterval = 100000;
            string resp = mq.PutAndGetByCorrelationId<string>(MqTmpl.BuildMessage("asd"), "TEST", "TRANSACTION.QUEUE", m => { return m.ReadString(m.MessageLength); });
            Console.WriteLine(resp);
        }
    }
}
