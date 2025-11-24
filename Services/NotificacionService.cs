using Plugin.LocalNotification;

namespace TrainiumNeon.Services
{
    public class NotificacionService : INotificacionService
    {
        public void EnviarNotificacion(string titulo, string mensaje, DateTime? programarHorario = null, bool repetirDiariamente = false)
        {
            var programarNotificacion = programarHorario.HasValue ? new NotificationRequestSchedule
            {
                NotifyTime = programarHorario.Value,
                RepeatType = repetirDiariamente ? NotificationRepeat.Daily : NotificationRepeat.No
            }: null;

            var notification = new NotificationRequest
            {
                Title = titulo,
                Description = mensaje,
                Schedule = programarNotificacion
            };

            LocalNotificationCenter.Current.Show(notification);
        }
    }
}
