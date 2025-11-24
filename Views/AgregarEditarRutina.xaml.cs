using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class AgregarEditarRutina : ContentPage
{
    public AgregarEditarRutina(AgregarEditarRutinaViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AgregarEditarRutinaViewModel vm)
        {
            await vm.InicializarAsync();
        }
    }

}