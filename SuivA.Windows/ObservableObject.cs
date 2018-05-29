using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;

namespace SuivA.Windows
{
    public class ObservableObject : BindableBase, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}