using TrainiumNeon.ViewModels;

namespace TrainiumNeon.Views;

public partial class Gimnasios : ContentPage
{
	public Gimnasios(GimnasiosViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}