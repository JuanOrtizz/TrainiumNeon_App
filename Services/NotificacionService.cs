using Plugin.LocalNotification;

namespace TrainiumNeon.Services
{
    public class NotificacionService : INotificacionService
    {
        public void EnviarNotificacion(string titulo, string mensaje, DateTime? programarHorario = null, bool repetirDiariamente = false)
        {

            // creo la notificacion
            var notification = new NotificationRequest
            {
                Title = titulo,
                Description = mensaje,
            };

            // programo la notificacion si se especifica un horario
            if (programarHorario.HasValue)
            {
                notification.Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = programarHorario.Value,
                    RepeatType = repetirDiariamente ? NotificationRepeat.Daily : NotificationRepeat.No
                };
            }

            // envio la notificacion al sistema
            LocalNotificationCenter.Current.Show(notification);
        }
    }
}
