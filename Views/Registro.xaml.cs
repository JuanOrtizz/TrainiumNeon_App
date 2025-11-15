using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class Registro : ContentPage
{
	public Registro(RegistroViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        vm.SetNavigation(Navigation);
    }
}