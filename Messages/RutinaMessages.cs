namespace TrainiumNeon.Messages
{
    public class RutinaMessages
    {
        public record RutinaCreadaMessage(string mensaje);
        public record RutinaActualizadaMessage(string mensaje);
        public record RutinaEliminadaMessage(string mensaje);
        public record RutinaSeleccionadaActualizadaMessage(string mensaje);

    }
}
