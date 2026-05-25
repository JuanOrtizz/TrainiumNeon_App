using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class Estadisticas : ContentPage
{
    private bool _inicializado = false;

    public Estadisticas(EstadisticasViewModel vm)
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
            if (BindingContext is EstadisticasViewModel vm)
            {
                await vm.InicializarAsync();
            }
        }
    }
}