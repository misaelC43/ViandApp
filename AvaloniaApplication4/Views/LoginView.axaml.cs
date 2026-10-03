using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication4.ViewModels;

namespace AvaloniaApplication4.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            //DataContext = new LoginViewModel();
        }
    }
}