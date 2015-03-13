using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryTemplate
{
    public class AdTmpl
    {
        public string SearchBase { get; set; }
        public bool UserIntegratedSecurity { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public AdTmpl(string searchBase)
        {
            UserIntegratedSecurity = true;
            SearchBase = searchBase;
        }
        public AdTmpl(string searchBase, string username, string password)
        {
            UserIntegratedSecurity = false;
            SearchBase = searchBase;
            Username = username;
            Password = password;
        }
        public List<T> Search<T>(string filter, HashSet<string> propsToLoad, Func<SearchResult, T> mapper)
        { 
            DirectoryEntry entry = null;
            if (UserIntegratedSecurity)
            {
                entry = new DirectoryEntry(SearchBase);
            }
            else
            {
                entry = new DirectoryEntry(SearchBase, Username, Password);
            }

            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = filter;
            foreach (string prop in propsToLoad)
            { 
                searcher.PropertiesToLoad.Add(prop);
            }

            SearchResultCollection result = searcher.FindAll();
            List<T> resultList = new List<T>();
            foreach(SearchResult sr in result)
            {
                T resultObj = mapper.Invoke(sr);
                resultList.Add(resultObj);
            }

            return resultList;
        }
        public static string GetProp(SearchResult rs, string propName)
        {
            ResultPropertyValueCollection propCol = rs.Properties[propName];
            if (propCol != null && propCol.Count > 0)
            {
                return propCol[0].ToString();
            }
            return string.Empty;
        }
    }
}
