using System.Windows.Input;

namespace OrderApp.Commands;

public class RelayCommand(Func<Task> execute, Func<bool> canExecute) : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
        return canExecute();
    }

    public async void Execute(object parameter)
    {
        await ExecuteAsync();
    }

    public Task ExecuteAsync()
    {
        return execute();
    }
}
