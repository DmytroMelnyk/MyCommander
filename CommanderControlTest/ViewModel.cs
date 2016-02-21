using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommanderControlTest
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _StringProperty = "My Cool String";
        public string StringProperty
        {
            get { return _StringProperty; }
            set
            {
                _StringProperty = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StringProperty)));
            }
        }
    }
}
