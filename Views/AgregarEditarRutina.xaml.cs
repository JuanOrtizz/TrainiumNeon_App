using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class AgregarEditarRutina : ContentPage
{
	public AgregarEditarRutina(AgregarEditarRutinaViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}