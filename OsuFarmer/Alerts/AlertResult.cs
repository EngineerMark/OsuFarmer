using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Alerts
{
    public struct AlertResult
    {
        /// <summary>
        /// If input alert, this will contain the content of the textbox
        /// </summary>
        public string? Input { get; set; }
        public string? PressedButton { get; set; }
        public bool SuccessfullResult { get; set; }
    }
}
