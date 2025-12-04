using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class Rutinas : ContentPage
{
    private bool _inicializado = false;

	public Rutinas(RutinasViewModel vm)
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
            if (BindingContext is RutinasViewModel vm)
            {
                await vm.InicializarAsync();
            }
        }
    }
}