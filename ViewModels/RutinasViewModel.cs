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
    public class RutinasViewModel : INotifyPropertyChanged, IRecipient<RutinaMessages.RutinaGuardadaMessage>, IRecipient<RutinaMessages.RutinaEliminadaMessage>
    {
        // Servicios y repositorio
        private readonly ISesionService _sesionService;
        private readonly IDisplayAlertService _displayAlertService;
        private readonly IRutinaRepositorio _rutinaRepositorio;

        //Propiedades privadas
        private int _idUsuarioActivo;
        private ObservableCollection<RutinaModel> _rutinas = new ObservableCollection<RutinaModel>();
        private RutinaModel? _rutinaSeleccionada;
        private bool _isBusy;

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
        public RutinaModel? RutinaSeleccionada
        {
            get => _rutinaSeleccionada;
            set
            {
                if (_rutinaSeleccionada != value)
                {
                    _rutinaSeleccionada = value;
                    OnPropertyChanged();

                    if (!IsBusy && _rutinaSeleccionada != null)
                    {
                        MarcarRutinaComoSeleccionadaCommand.Execute(null);
                    }
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

        // Comando 
        public ICommand MarcarRutinaComoSeleccionadaCommand { get; }
        public ICommand AgregarRutinaCommand { get; }
        public ICommand EditarRutinaCommand { get; }

        // Constructor
        public RutinasViewModel(ISesionService sesionService, IDisplayAlertService displayAlertService, IRutinaRepositorio rutinaRepositorio)
        {
            // Inicializa servicio y repositorio por DI
            _sesionService = sesionService;
            _displayAlertService = displayAlertService;
            _rutinaRepositorio = rutinaRepositorio;
            // Inicializan comandos
            MarcarRutinaComoSeleccionadaCommand = new Command(async () =>  await MarcarRutinaComoSeleccionadaAsync());
            AgregarRutinaCommand = new Command(async () => await NavegarAgregarRutinaAsync());
            EditarRutinaCommand = new Command<int>(async (idRutina) => await NavegarEditarRutinaAsync(idRutina));
            // Suscripcion a mensajeria para actualizar datos al guardar o eliminar rutina 
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        // Task asincrona para cargar las rutinas al iniciar
        public async Task InicializarAsync()
        {
            try
            {
                IsBusy = true;
                // Capturo el Id del usuario activo (logeado)
                IdUsuarioActivo = _sesionService.ObtenerSesion();
                await ObtenerRutinasAsync();
                IsBusy = false;
            }
            catch (Exception)
            {
               await _displayAlertService.MostrarAlertAsync("Error", "Ocurrió un error al cargar las rutinas. Intente nuevamente.", "Aceptar");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Task asincrona para marcar una rutina como seleccionada
        private async Task MarcarRutinaComoSeleccionadaAsync()
        {
            try
            {
                var rutinaSeleccionadaActualizada = await _rutinaRepositorio.MarcarRutinaSeleccionadaAsync(RutinaSeleccionada!.Id);
                if (rutinaSeleccionadaActualizada)
                {
                    // Muestra un toast de confirmacion
                    var toast = Toast.Make($"Marcaste la rutina {RutinaSeleccionada.Nombre} como seleccionada.", ToastDuration.Short);
                    await toast.Show();

                    //Envia mensaje de actualizacion para que se actualicen los datos en otros viewModels
                    WeakReferenceMessenger.Default.Send(new RutinaMessages.RutinaSeleccionadaActualizadaMessage());
                }
            }
            catch (Exception)
            {
               var reintentar = await _displayAlertService.MostrarAlertConConfirmacionAsync("Error", "Ocurrió un error al seleccionar la rutina. Intente nuevamente.", "Reintentar", "Cancelar");
               if (reintentar)
               {
                   await MarcarRutinaComoSeleccionadaAsync();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Task asincrona para navegar a la pagina de agregar rutina
        private async Task NavegarAgregarRutinaAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(AgregarEditarRutina)}?accion=Agregar");
        }

        // Task asincrona para navegar a la pagina de editar rutina
        private async Task NavegarEditarRutinaAsync(int idRutina)
        {
            await Shell.Current.GoToAsync($"{nameof(AgregarEditarRutina)}?accion=Editar&idRutina={idRutina}");
        }

        private async Task ObtenerRutinasAsync()
        {
            // Inicializo la lista
            IReadOnlyList<RutinaModel> listaRutinas;
            // Obtengo las rutinas del usuario
            listaRutinas = await _rutinaRepositorio.ObtenerRutinasPorUsuarioAsync(IdUsuarioActivo);
            var contadorRutinas = 0;
            Rutinas = new ObservableCollection<RutinaModel>(listaRutinas);
            foreach (var r in listaRutinas)
            {
                contadorRutinas++;
                r.NumeroRutina = contadorRutinas;
            }
            // cargo la rutina seleccionada
            var rutinaSeleccionada = await _rutinaRepositorio.ObtenerRutinaSeleccionadaAsync(IdUsuarioActivo);
            RutinaSeleccionada = Rutinas.FirstOrDefault(r => r.Id == rutinaSeleccionada.Id);
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Implementacion de IRecipient para Rutina Guardada
        public async void Receive(RutinaMessages.RutinaGuardadaMessage message)
        {
            try
            {
                IsBusy = true;
                // Actualizo la lista de rutinas
                await ObtenerRutinasAsync();
                // Muestro un toast de confirmacion
                var toast = Toast.Make(message.mensaje, ToastDuration.Short);
                await toast.Show();
            }
            catch
            {
                Console.WriteLine("Error al actualizar la lista de rutinas despues de actualizar.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Implementacion de IRecipient para Rutina eliminada
        public async void Receive(RutinaMessages.RutinaEliminadaMessage message)
        {
            try
            {
                IsBusy = true;
                // Actualizo la lista de rutinas
                await ObtenerRutinasAsync();
                // Muestro un toast de confirmacion
                var toast = Toast.Make(message.mensaje, ToastDuration.Short);
                await toast.Show();
            }
            catch (Exception)
            {
                Console.WriteLine("Error al actualizar la lista de rutinas despues de eliminar.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
