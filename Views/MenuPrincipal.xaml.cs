using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class MenuPrincipal : ContentPage
{
    private bool _inicializado = false;

    public MenuPrincipal(MenuPrincipalViewModel vm)
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
            if (BindingContext is MenuPrincipalViewModel vm)
            {
                await vm.InicializarAsync();
            }
        }
    }
}