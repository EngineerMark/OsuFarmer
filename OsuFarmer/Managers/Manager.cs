using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Managers
{
    public class Manager<T>
    {
        public static T? Instance { get; set; }

        protected void Register(T o){
            Instance = o;
        }
    }
}
