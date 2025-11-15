using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class AgregarEjercicioRutina : ContentPage
{
	public AgregarEjercicioRutina(AgregarEjercicioRutinaViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}