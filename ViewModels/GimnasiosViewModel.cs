using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrainiumNeon.Services;

namespace TrainiumNeon.ViewModels
{
    public class GimnasiosViewModel : INotifyPropertyChanged
    {
        // Servicio
        private readonly IPermisosService _permisosService;

        //Propiedades privadas
        private bool _isBusy;

        // Propiedades publicas
        private bool IsBusy
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
        public GimnasiosViewModel(IPermisosService permisosService)
        {
            // Inicializa servicio por DI
            _permisosService = permisosService;
            _ = InicializarAsync();
        }

        private async Task InicializarAsync()
        {
            // Pido el permiso de ubicacion
            if (await _permisosService.SolicitarUbicacionAsync())
            {
                try
                {
                    IsBusy = true;
                    // Capturo 
                    var ubicacionRequest = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                    // Capturo la ubicacion
                    var ubicacion = await Geolocation.GetLocationAsync(ubicacionRequest);
                    // Si la ubicacion no es null
                    if (ubicacion != null)
                    {
                        
                    }
                }// Manejo excepciones
                catch (FeatureNotSupportedException)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "GPS no disponible en este dispositivo.", "Aceptar");
                }
                catch (PermissionException)
                {
                    // Capturo si el usuario apreta ir a permisos
                    var abrirAjustes = await App.Current.MainPage.DisplayAlert("Permiso denegado", "Debe otorgar acceso a ubicación.", "Ir a permisos", "Cancelar");
                    // Si abrirAjustes es true navega a la pantalla de ajustes de la app
                    if (abrirAjustes)
                    {
                        AppInfo.ShowSettingsUI();
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        // Implementacion de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
