using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class AgregarEditarEjercicioRutina : ContentPage
{
	public AgregarEditarEjercicioRutina(AgregarEditarEjercicioRutinaViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AgregarEditarEjercicioRutinaViewModel vm)
        {
            await vm.InicializarAsync();
        }
    }
}