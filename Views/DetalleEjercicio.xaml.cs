using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class DetalleEjercicio : ContentPage
{
    private bool _inicializado = false;

    public DetalleEjercicio(DetalleEjercicioViewModel vm)
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
            if (BindingContext is DetalleEjercicioViewModel vm)
            {
                await vm.InicializarAsync();
            }
        }
    }
}