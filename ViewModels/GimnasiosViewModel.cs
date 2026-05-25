using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    public class GimnasiosViewModel : INotifyPropertyChanged
    {
        // Servicios
        private readonly IPermisosService _permisosService;
        private readonly IDisplayAlertService _displayAlertService;

        //Propiedades privadas
        private bool _isBusy;

        // Propiedades publicas
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

        // Constructor
        public GimnasiosViewModel(IPermisosService permisosService, IDisplayAlertService displayAlertService)
        {
            // Inicializan servicios por DI
            _permisosService = permisosService;
            _displayAlertService = displayAlertService;
        }

        public async Task InicializarAsync()
        {
            // Pido el permiso de ubicacion
            if (await _permisosService.SolicitarUbicacionAsync())
            {
                try
                {
                    IsBusy = true;
                    // solicito la ubicacion con precision media y timeout de 10 segundos
                    var ubicacionRequest = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                    if (ubicacionRequest != null)
                    {
                        // Capturo la ubicacion
                        var ubicacion = await Geolocation.GetLocationAsync(ubicacionRequest);
                        // Si la ubicacion no es null
                        if (ubicacion != null)
                        {
                            // Uso la ubicacion para el radar de gimnasios
                            Console.WriteLine($"Latitud: {ubicacion.Latitude}, Longitud: {ubicacion.Longitude}");
                        }
                    }  
                }
                catch (FeatureNotSupportedException)
                {
                    await _displayAlertService.MostrarAlertAsync("Error", "GPS no disponible en este dispositivo.", "OK");
                }
                catch (PermissionException)
                {
                    await SolicitarPermisosUbicacionAlertAsync();
                }
                catch (Exception)
                {
                    await _displayAlertService.MostrarAlertAsync("Error", "No se pudieron conceder los permisos de ubicación. Revisa los permisos de la app e intentá nuevamente.", "Aceptar");
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                await SolicitarPermisosUbicacionAlertAsync();
            }
        }

        private async Task SolicitarPermisosUbicacionAlertAsync()
        {
            // Capturo si el usuario apreta ir a permisos
            var abrirAjustes = await _displayAlertService.MostrarAlertConConfirmacionAsync("Permiso denegado", "Debes otorgar acceso a ubicación para encontrar los gimnasios mas cercanos.", "Ir a permisos", "OK");
            // Si abrirAjustes es true navega a la pantalla de ajustes de la app
            if (abrirAjustes)
            {
                AppInfo.ShowSettingsUI();
            }
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
