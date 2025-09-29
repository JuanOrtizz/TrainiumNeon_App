using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class Registro : ContentPage
{
	public Registro()
	{
		InitializeComponent();
        BindingContext = new RegistroViewModel(this.Navigation);
    }
}