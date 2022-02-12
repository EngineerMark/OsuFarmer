using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Managers
{
    public class PageManager : Manager<PageManager>
    {
        protected List<Type> Pages { get; } = new List<Type>(){
            typeof(TrackerPage),
            typeof(SessionsPage),
            typeof(SettingsPage),
            //typeof(AboutPage),
            //typeof(BeatmapsPage),
        };

        public List<ContentPage> ContentPages { get; }

        public PageManager(){
            Register(this);

            ContentPages = new List<ContentPage>();
            
            foreach(Type t in Pages){
                AppShell? s = AppManager.Instance?.GetShell();
                ContentPage? page = s.AddPage(t);
                if (page != null)
                {
                    ContentPages.Add(page);
                }
            }
        }

        public T? GetPage<T>() where T : ContentPage{
            //return (T?)ContentPages.Find(p => p.GetType().GetGenericTypeDefinition() == typeof(T));
            T? page = null;
            foreach (ContentPage _page in ContentPages)
            {
                if(_page is T){
                    page = (T?)_page;
                    break;
                }
            }
            return page;
        }

        public void GoTo<T>() where T : ContentPage {
            Device.BeginInvokeOnMainThread(async () => await Shell.Current?.GoToAsync("//" + GetPage<T>()?.Title?.ToLower()));
        }
    }
}
