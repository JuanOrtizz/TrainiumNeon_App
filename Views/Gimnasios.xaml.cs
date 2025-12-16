using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class Gimnasios : ContentPage
{
    private bool _inicializado = false;

    public Gimnasios(GimnasiosViewModel vm)
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
            if (BindingContext is GimnasiosViewModel vm)
			{
				await vm.InicializarAsync();
			}
		}
    }

}