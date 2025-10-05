using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class Estadisticas : ContentPage
{
	public Estadisticas()
	{
		InitializeComponent();
		BindingContext = new EstadisticasViewModel();
    }
}