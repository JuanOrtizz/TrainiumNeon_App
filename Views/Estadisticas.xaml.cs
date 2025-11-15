using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class Estadisticas : ContentPage
{
	public Estadisticas(EstadisticasViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}