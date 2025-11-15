using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class Rutinas : ContentPage
{
	public Rutinas(RutinasViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}