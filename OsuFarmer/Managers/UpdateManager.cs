using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Managers
{
    public class UpdateManager : Manager<UpdateManager>
    {
        public UpdateManager()
        {
            Register(this);
        }

        public Version GetAppVersion()
        {
            string version = GetType().Assembly.GetName().Version.ToString();
            Version v = new Version(version);
            return v;
        }
    }
}
