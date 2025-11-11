using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TrainiumNeon.Models;
using TrainiumNeon.Services;
using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    public class EstadisticasViewModel : INotifyPropertyChanged
    {
        //Variable para el servicio de la API de ejercicios
        private readonly ApiEjerciciosService _apiEjercicioService;

        //Variable para la lista de ejercicios
        private ObservableCollection<EjercicioModel> _ejercicios;
        public ObservableCollection<EjercicioModel> Ejercicios
        {
            get => _ejercicios;
            set
            {
                if (_ejercicios != value)
                {
                    _ejercicios = value;
                    OnPropertyChanged(nameof(Ejercicios));
                }
            }
        }

        //Lista para los timos de musculos (puede ser un enum)
        public ObservableCollection<string> Musculos { get; set; } = new ObservableCollection<string>
        {
            "Todos", "Pecho", "Espalda", "Hombro", "Bíceps", "Tríceps", "Pierna", "Core"
        };

        //Variable para filtrar por musculos por un Picker en la View
        private string _musculoSeleccionado;
        public string MusculoSeleccionado
        {
            get => _musculoSeleccionado;
            set
            {
                if(_musculoSeleccionado != value)
                {
                    _musculoSeleccionado = value;
                    _ = CargarEjerciciosAsync();
                }
            } 
        }

        //Comando para navegar y ver detalles del ejercicio
        public Command VerDetallesEjercicio { get; }

        public EstadisticasViewModel(ApiEjerciciosService apiEjerciciosService)
        {
            _apiEjercicioService = apiEjerciciosService;
            VerDetallesEjercicio = new Command<EjercicioModel>(async (ejercicio) => await NavegarDetallesEjercicio(ejercicio));
            _musculoSeleccionado = Musculos[0];
            Ejercicios = new ObservableCollection<EjercicioModel>();
            _ = CargarEjerciciosAsync();
        }

        private async Task NavegarDetallesEjercicio(EjercicioModel ejercicio)
        {
            await Shell.Current.GoToAsync($"{nameof(DetalleEjercicio)}?idEjercicio={ejercicio.Id}");
        }

        private async Task CargarEjerciciosAsync()
        {
            try
            {
                List<EjercicioModel> listaEjerciciosApi;

                if(MusculoSeleccionado == "Todos")
                {
                    listaEjerciciosApi = (List<EjercicioModel>)await _apiEjercicioService.GetEjerciciosAsync();
                }
                else
                {
                    listaEjerciciosApi = (List<EjercicioModel>)await _apiEjercicioService.GetEjerciciosByGrupoMuscularAsync(MusculoSeleccionado);
                }

                Ejercicios = new ObservableCollection<EjercicioModel>(listaEjerciciosApi);

            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error al usuario
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
