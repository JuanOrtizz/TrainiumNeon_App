using TrainiumNeon.ViewModels; 

namespace TrainiumNeon.Views;

public partial class Perfil : ContentPage
{
	public Perfil(PerfilViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PerfilViewModel vm)
            await vm.CargarDatosUsuarioAsync();
    }

}