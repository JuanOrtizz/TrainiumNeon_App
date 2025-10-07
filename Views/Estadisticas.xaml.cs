using TrainiumNeon.Services;
using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class Estadisticas : ContentPage
{
	public Estadisticas()
	{
		InitializeComponent();
        //Temporal hasta que agregue DI
        var apiService = new ApiEjerciciosService(new HttpClient());
        BindingContext = new EstadisticasViewModel(apiService);
    }
}