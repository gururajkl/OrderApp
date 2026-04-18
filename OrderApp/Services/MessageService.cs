using System.Windows;

namespace OrderApp.Services;

class MessageService : IMessageService
{
    public void ShowInfoMessage(string message)
    {
        MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
