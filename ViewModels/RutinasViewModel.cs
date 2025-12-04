using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TrainiumNeon.Data.Repositories;
using TrainiumNeon.Messages;
using TrainiumNeon.Models;
using TrainiumNeon.Services;
using TrainiumNeon.Views;

namespace TrainiumNeon.ViewModels
{
    public class RutinasViewModel : INotifyPropertyChanged, IRecipient<RutinaMessages.RutinaCreadaMessage>, IRecipient<RutinaMessages.RutinaActualizadaMessage>, IRecipient<RutinaMessages.RutinaEliminadaMessage>
    {
        // Servicio y repositorio
        private readonly ISesionService _sesionService;
        private readonly IRutinaRepositorio _rutinaRepositorio;

        //Propiedades privadas
        private int _idUsuarioActivo;
        private ObservableCollection<RutinaModel> _rutinas;
        private RutinaModel _rutinaSeleccionada;

        //Propiedades Publicas
        public int IdUsuarioActivo
        {
            get => _idUsuarioActivo;
            set
            {
                if (_idUsuarioActivo != value) 
                {
                    _idUsuarioActivo = value;
                    OnPropertyChanged();
                }

            }
        }
        public ObservableCollection<RutinaModel> Rutinas
        {
            get => _rutinas;
            set
            {
                if (_rutinas != value)
                {
                    _rutinas = value;
                    OnPropertyChanged();
                }
            }
        }
        public RutinaModel RutinaSeleccionada
        {
            get => _rutinaSeleccionada;
            set
            {
                if(_rutinaSeleccionada != value)
                {
                    _rutinaSeleccionada = value;
                    _ = MarcarRutinaComoSeleccionada();
                    OnPropertyChanged();
                }
            }
        }

        // Comando 
        public ICommand AgregarRutinaCommand { get; }
        public ICommand EditarRutinaCommand { get; }

        // Constructor
        public RutinasViewModel(ISesionService sesionService, IRutinaRepositorio rutinaRepositorio)
        {
            // Inicializa servicio y repositorio por DI
            _sesionService = sesionService;
            _rutinaRepositorio = rutinaRepositorio;
            // InicializaN comandoS
            AgregarRutinaCommand = new Command(async () => await NavegarAgregarRutina());
            EditarRutinaCommand = new Command<int>(async (idRutina) => await NavegarEditarRutina(idRutina));
        }

        // Task asincrona para cargar las rutinas al iniciar
        public async Task InicializarAsync()
        {
            // Capturo el Id del usuario activo (logeado)
            IdUsuarioActivo = _sesionService.ObtenerSesion();
            await ObtenerRutinasAsync();
        }

        // Task asincrona para marcar una rutina como seleccionada
        private async Task MarcarRutinaComoSeleccionada()
        {
            if(await _rutinaRepositorio.MarcarRutinaSeleccionadaAsync(RutinaSeleccionada.Id))
            {
                // Muestra un toast de confirmacion
                var toast = Toast.Make($"Marcaste la rutina {RutinaSeleccionada.Nombre} como seleccionada.", ToastDuration.Short);
                await toast.Show();

                //Envia mensaje de actualizacion para que se actualicen los datos en otros viewModels
                WeakReferenceMessenger.Default.Send(new RutinaMessages.RutinaSeleccionadaActualizadaMessage());
            }

        }

        // Task asincrona para navegar a la pagina de agregar rutina
        private async Task NavegarAgregarRutina()
        {
            await Shell.Current.GoToAsync($"{nameof(AgregarEditarRutina)}?accion=Agregar");
        }

        // Task asincrona para navegar a la pagina de editar rutina
        private async Task NavegarEditarRutina(int idRutina)
        {
            await Shell.Current.GoToAsync($"{nameof(AgregarEditarRutina)}?accion=Editar&idRutina={idRutina}");
        }

        private async Task ObtenerRutinasAsync()
        {
            // Inicializo la lista
            IReadOnlyList<RutinaModel> listaRutinas;
            // Obtengo las rutinas del usuario
            listaRutinas = await _rutinaRepositorio.ObtenerRutinasPorUsuarioAsync(IdUsuarioActivo);
            Rutinas = new ObservableCollection<RutinaModel>(listaRutinas);
            // cargo la rutina seleccionada
            var rutinaSeleccionada = await _rutinaRepositorio.ObtenerRutinaSeleccionadaAsync(IdUsuarioActivo);
            RutinaSeleccionada = Rutinas.FirstOrDefault(r => r.Id == rutinaSeleccionada.Id);
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Implementacion de IRecipient para Rutina creada
        public async void Receive(RutinaMessages.RutinaCreadaMessage message)
        {
            await ObtenerRutinasAsync();
        }

        // Implementacion de IRecipient para Rutina actualizada
        public async void Receive(RutinaMessages.RutinaActualizadaMessage message)
        {
            await ObtenerRutinasAsync();
        }

        // Implementacion de IRecipient para Rutina eliminada
        public async void Receive(RutinaMessages.RutinaEliminadaMessage message)
        {
            await ObtenerRutinasAsync();
        }

    }
}
