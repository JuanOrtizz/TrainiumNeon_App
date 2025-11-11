using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class AgregarEditarRutina : ContentPage
{
	public AgregarEditarRutina()
	{
		InitializeComponent();
		BindingContext = new AgregarEditarRutinaViewModel();
	}
}