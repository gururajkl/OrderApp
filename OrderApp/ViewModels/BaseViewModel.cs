using System.ComponentModel;

namespace OrderApp.ViewModels;

/// <summary>
/// I created this view model to implement the INotifyPropertyChanged interface.
/// So that all the viewmodels can inherit from this and don't worry about the implementation of the INotifyPropertyChanged interface.
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
