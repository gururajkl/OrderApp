using System.Text.RegularExpressions;
using System.Windows;

namespace OrderApp.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static readonly Regex _regex = new("[^0-9]+");

    public MainWindow()
    {
        InitializeComponent();
    }

    private void NumberOnly(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = _regex.IsMatch(e.Text);
    }
}