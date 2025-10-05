namespace TrainiumNeon.Views;
using TrainiumNeon.ViewModels;

public partial class MenuPrincipal : ContentPage
{
	public MenuPrincipal()
	{
		InitializeComponent();
		BindingContext = new MenuPrincipalViewModel();
	}
}