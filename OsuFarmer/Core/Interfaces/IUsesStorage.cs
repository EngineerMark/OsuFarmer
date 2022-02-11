using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Core.Interfaces
{
    public interface IUsesStorage
    {
        public string FileLocation { get; }
    }
}
