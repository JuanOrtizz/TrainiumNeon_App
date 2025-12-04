using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class AgregarEditarRutina : ContentPage
{
    private bool _inicializado = false;

    public AgregarEditarRutina(AgregarEditarRutinaViewModel vm)
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
            if (BindingContext is AgregarEditarRutinaViewModel vm)
            {
                await vm.InicializarAsync();
            }
        }
    }

}