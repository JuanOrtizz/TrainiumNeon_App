using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainiumNeon.Models;
using TrainiumNeon.Services;
using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    public class EstadisticasViewModel : INotifyPropertyChanged
    {
        // Servicio
        private readonly IApiEjerciciosService _apiEjercicioService;
        // Propiedades privadas
        private ObservableCollection<EjercicioModel> _ejercicios;
        private ObservableCollection<string> _gruposMusculares;
        private string _grupoMuscularSeleccionado;
        private bool _isBusy;
        // Propiedades publicas
        public ObservableCollection<EjercicioModel> Ejercicios
        {
            get => _ejercicios;
            set
            {
                if (_ejercicios != value)
                {
                    _ejercicios = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<string> GruposMusculares
        {
            get => _gruposMusculares;
            set
            {
                if (_gruposMusculares != value)
                {
                    _gruposMusculares = value;
                    OnPropertyChanged();
                }
            }
        }
        public string GrupoMuscularSeleccionado
        {
            get => _grupoMuscularSeleccionado;
            set
            {
                if(_grupoMuscularSeleccionado != value)
                {
                    _grupoMuscularSeleccionado = value;
                    _ = CargarEjerciciosAsync();
                }
            } 
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        //Comando
        public Command VerDetallesEjercicio { get; }

        // Constructor
        public EstadisticasViewModel(IApiEjerciciosService apiEjerciciosService)
        {
            // Inicializa servicio por DI
            _apiEjercicioService = apiEjerciciosService;
            //Inicializan grupos musculares al cargar la pantalla
            GruposMusculares = ObtenerGruposMusculares();
            // Inicializa comando
            VerDetallesEjercicio = new Command<EjercicioModel>(async (ejercicio) => await NavegarDetallesEjercicio(ejercicio));
            // Inicializa grupo muscular seleccionado por defecto (Todos)
            _grupoMuscularSeleccionado = GruposMusculares[0];
            // Inicializa coleccion de ejercicios
            Ejercicios = new ObservableCollection<EjercicioModel>();
            // Se cargan todos los ejericicos al cargar la pantalla
            _ = CargarEjerciciosAsync();
        }

        // Task privada para navegar a detalles de ejercicio
        private async Task NavegarDetallesEjercicio(EjercicioModel ejercicio)
        {
            await Shell.Current.GoToAsync($"{nameof(DetalleEjercicio)}?idEjercicio={ejercicio.Id}");
        }

        // Task privada para cargar ejercicios desde API
        private async Task CargarEjerciciosAsync()
        {
            try
            {
                // Indica que se esta cargando y muestra el spinner
                IsBusy = true;
                List<EjercicioModel> listaEjerciciosApi;

                if (GrupoMuscularSeleccionado == "Todos")
                {
                    listaEjerciciosApi = (List<EjercicioModel>)await _apiEjercicioService.GetEjerciciosAsync();
                }
                else
                {
                    listaEjerciciosApi = (List<EjercicioModel>)await _apiEjercicioService.GetEjerciciosByGrupoMuscularAsync(GrupoMuscularSeleccionado);
                }
                // Asigna la lista obtenida a la coleccion observable
                Ejercicios = new ObservableCollection<EjercicioModel>(listaEjerciciosApi);

            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error al usuario
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                // Al terminar deja de mostrar el spinner
                IsBusy = false;
            }
        }

        // Metodo privado para obtener los grupos musculares desde el enum
        private ObservableCollection<string> ObtenerGruposMusculares()
        {
            ObservableCollection<string> coleccionGM = new ObservableCollection<string>();

            foreach(var gm in Enum.GetValues(typeof(GrupoMuscularEnum)).Cast<GrupoMuscularEnum>())
            {
                coleccionGM.Add(gm.ToString());
            }
            return coleccionGM;
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
