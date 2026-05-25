using TrainiumNeon.ViewModels; 

namespace TrainiumNeon.Views;

public partial class Perfil : ContentPage
{
    private bool _inicializado = false;

    public Perfil(PerfilViewModel vm)
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
            if (BindingContext is PerfilViewModel vm)
            {
                await vm.InicializarAsync();
            }
        }
    }

}