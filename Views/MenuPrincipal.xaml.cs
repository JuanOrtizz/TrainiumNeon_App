using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class MenuPrincipal : ContentPage
{
	public MenuPrincipal(MenuPrincipalViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}