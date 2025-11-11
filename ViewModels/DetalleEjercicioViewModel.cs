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

namespace TrainiumNeon.ViewModels
{
    [QueryProperty(nameof(IdEjercicio), "idEjercicio")]
    public class DetalleEjercicioViewModel: INotifyPropertyChanged
    {
        private readonly ApiEjerciciosService _apiEjerciciosService;
        private int idEjercicio;
        private EjercicioModel ejercicio;

        public int IdEjercicio
        {
            get => idEjercicio;
            set
            {
                if(idEjercicio != value)
                {
                    idEjercicio = value;
                    _ = CargarEjercicioAsync();
                } 
            }
        }

        public EjercicioModel Ejercicio
        {
            get => ejercicio;
            set
            {
                if(ejercicio != value)
                {
                    ejercicio = value;
                    OnPropertyChanged(nameof(Ejercicio));
                    OnPropertyChanged(nameof(Nombre));
                    OnPropertyChanged(nameof(GrupoMuscular));
                    OnPropertyChanged(nameof(ImagenUrl));
                }
            }
        }

        public string Nombre => Ejercicio?.Nombre ?? "";
        public string GrupoMuscular => Ejercicio?.GrupoMuscular ?? "";
        public string ImagenUrl => Ejercicio?.ImagenUrl ?? "default_ejercicio.webp";

        public DetalleEjercicioViewModel(ApiEjerciciosService apiEjerciciosService)
        {
            _apiEjerciciosService = apiEjerciciosService;
        }

        private async Task CargarEjercicioAsync()
        {
            try
            {
                var ejercicioApi = await _apiEjerciciosService.GetEjercicioByIdAsync(idEjercicio);
                Ejercicio = ejercicioApi;
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
