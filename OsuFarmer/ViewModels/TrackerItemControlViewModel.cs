using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.ViewModels
{
    public class TrackerItemControlViewModel : BaseViewModel
    {
        public static readonly string PositiveCharacter = "\u2227";
        public static readonly IBrush PositiveColor = Brushes.LightGreen;
        public static readonly string NegativeCharacter = "\u2228";
        public static readonly IBrush NegativeColor = Brushes.Red;

        public string? AttachedProperty { get; set; }
        public bool InvertSign { get; set; } = false;

        private string _Title = "Example Tracker";
        private double _OriginalValue = 0;
        private double _ChangedValue = 0;
        private string _ChangedSign = PositiveCharacter;
        private IBrush _ChangedSignColor = PositiveColor;

        public string Title { get { return _Title; } set { _Title = value; OnPropertyChanged(nameof(Title)); } }
        public string OriginalValue { get { return string.Format("{0:n0}", _OriginalValue); } set { _OriginalValue = Convert.ToDouble(value); OnPropertyChanged(nameof(OriginalValue)); } }
        public string ChangedValue { get { return string.Format("{0:n0}", _ChangedValue); } set { _ChangedValue = Convert.ToDouble(value); OnPropertyChanged(nameof(ChangedValue)); } }
        public string ChangedSign { get { return _ChangedSign; } set { _ChangedSign = value; OnPropertyChanged(nameof(ChangedSign)); } }
        public IBrush ChangedSignColor { get { return _ChangedSignColor; } set { _ChangedSignColor = value; OnPropertyChanged(nameof(ChangedSignColor)); } }
    }
}
