using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class DetalleEjercicio : ContentPage
{
	public DetalleEjercicio(DetalleEjercicioViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}