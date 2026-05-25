using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class AgregarEditarEjercicioRutina : ContentPage
{
    private bool _inicializado = false;

    public AgregarEditarEjercicioRutina(AgregarEditarEjercicioRutinaViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_inicializado)
        {
            _inicializado = true;
            if (BindingContext is AgregarEditarEjercicioRutinaViewModel vm)
            {
                await vm.InicializarAsync();
            }
        }
    }
}