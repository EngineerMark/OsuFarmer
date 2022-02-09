using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_farmer.Managers
{
    public class AppManager : Manager<AppManager>
    {
        private AppManagerData data;

        public SessionManager SessionManager { get; set; }
        public PageManager PageManager { get; set; }

        public AppManager(AppManagerData data)
        {
            Register(this);

            this.data = data;

            Console.WriteLine("Created AppManager");
        }

        public void Start(){
            SessionManager = new SessionManager();
            PageManager = new PageManager();

            Device.BeginInvokeOnMainThread(ApplicationLoop);
        }

        private async void ApplicationLoop(){
            await Task.Delay(2000);
            while(true)
            {
                await Task.Delay(1000);
            }
        }

        public AppShell? GetShell(){
            return data.Shell;
        }

        public Page? GetCurrentPage(){
            return (Shell.Current.CurrentItem.Items[0] as IShellSectionController).PresentedPage;
        }

        public Page? FindPage(string name){
            //ShellItem? item = Shell.Current.Items.Where(a=>a.Title.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            //IEnumerable<ShellItem> items = Shell.Current.Items.Where(a => a is FlyoutItem);
            //if(items==null){
            //    return null;
            //}
            return null;
            //if(item==null)
            //    return null;

            //return (item as IShellSectionController)?.PresentedPage;
        }
    }


    public struct AppManagerData {
        public AppShell Shell;
    }
}
