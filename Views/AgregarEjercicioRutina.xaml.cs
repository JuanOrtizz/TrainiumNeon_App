using TrainiumNeon.ViewModels;
namespace TrainiumNeon.Views;

public partial class AgregarEjercicioRutina : ContentPage
{
	public AgregarEjercicioRutina()
	{
		InitializeComponent();
		BindingContext = new AgregarEjercicioRutinaViewModel();
	}
}