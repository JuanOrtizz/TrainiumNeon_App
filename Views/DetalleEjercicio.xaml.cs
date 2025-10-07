namespace TrainiumNeon.Views;
using TrainiumNeon.Services;
using TrainiumNeon.ViewModels;

public partial class DetalleEjercicio : ContentPage
{
	public DetalleEjercicio()
	{
		InitializeComponent();
        //Temporal hasta que agregue DI
        var apiService = new ApiEjerciciosService(new HttpClient());
        BindingContext = new DetalleEjercicioViewModel(apiService);
    }
}