using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        vm.SetNavigation(Navigation);
    }

}