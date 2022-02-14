using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.ViewModels
{
    public class AlertWindowViewModel : BaseViewModel
    {
        public bool IsFinished { get; set; } = false;

        private string? _InputData = "";
        public string? InputData { get { return _InputData; } set { _InputData = value; OnPropertyChanged(nameof(InputData)); } }

        private bool _ShowInputField = false;
        public bool ShowInputField { get { return _ShowInputField; } set { _ShowInputField = value; OnPropertyChanged(nameof(ShowInputField)); } }

        private bool _InputFieldPassword = false;
        public bool InputFieldPassword { get { return _InputFieldPassword; } set { _InputFieldPassword = value; OnPropertyChanged(nameof(InputFieldPassword)); } }

        private string? _Title = "Alert Title";
        public string? Title { get { return _Title; } set { _Title = value; OnPropertyChanged(nameof(Title)); } }

        private string? _Description = "Alert Description";
        public string? Description { get { return _Description; } set { _Description = value; OnPropertyChanged(nameof(Description)); } }
    }
}
