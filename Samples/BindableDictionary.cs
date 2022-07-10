using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Samples
{
    public class BindableItem<V> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private V _value;
        public V Value 
        {
            set
            {
                if (_value == null || !_value.Equals(value))
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }

            get => _value; 
        }
    }
}
