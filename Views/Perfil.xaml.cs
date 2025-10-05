using TrainiumNeon.ViewModels; 

namespace TrainiumNeon.Views;

public partial class Perfil : ContentPage
{
	public Perfil()
	{
		InitializeComponent();
		BindingContext = new PerfilViewModel();
	}
}