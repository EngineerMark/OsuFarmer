using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_farmer.Core.Interfaces
{
    public interface IUsesStorage
    {
        public string FileLocation { get; }
    }
}
