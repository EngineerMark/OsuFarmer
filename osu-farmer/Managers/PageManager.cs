using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_farmer.Managers
{
    public class PageManager : Manager<PageManager>
    {
        protected List<Type> Pages { get; } = new List<Type>(){
            typeof(TrackerPage),
            typeof(SessionsPage),
            typeof(SettingsPage),
            typeof(AboutPage),
        };

        public List<ContentPage> ContentPages { get; }

        public PageManager(){
            Register(this);

            ContentPages = new List<ContentPage>();
            
            foreach(Type t in Pages){
                ContentPage? page = AppManager.Instance?.GetShell().AddPage(t);
                if(page!=null){
                    ContentPages.Add(page);
                }
            }
        }

        public T? GetPage<T>() where T : ContentPage{
            return (T?)ContentPages.Find(p => p.GetType().GetGenericTypeDefinition() == typeof(T));
        }
    }
}
