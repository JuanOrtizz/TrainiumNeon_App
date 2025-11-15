using TrainiumNeon.ViewModels; 

namespace TrainiumNeon.Views;

public partial class Perfil : ContentPage
{
	public Perfil(PerfilViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}
}