using System.Text.RegularExpressions;

namespace TrainiumNeon.Services
{
    public class ValidacionService : IValidacionService
    {
        // Metodo para validar que un campo no este vacio
        public string ValidarCampoVacio(string campo)
        {
            if (string.IsNullOrWhiteSpace(campo))
            {
                return "El campo está vacío.";
            }
            return string.Empty;
        }

        // Metodo para validar el nombre completo (Nombre y Apellido) del usuario
        public string ValidarNombreCompleto(string nombreCompleto)
        {
            // Regex para validar el nombre completo con nombres compuestos y apellidos compuestos, incluyendo caracteres especiales en español
            var regex = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?:\s+(?:[A-Za-zÁÉÍÓÚáéíóúÑñ]+|de|del|la|las|los))*\s+[A-Za-zÁÉÍÓÚáéíóúÑñ]+$");

            if (string.IsNullOrWhiteSpace(nombreCompleto))
            {
                return "El campo está vacío.";
            }

            if (nombreCompleto.Length < 3)
            {
                return "El nombre debe tener al menos 3 caracteres.";
            }

            if (!regex.IsMatch(nombreCompleto))
            {
                return "El formato del nombre es invalido (Nombre y Apellido).";
            }

            return string.Empty;
        }

        // Metodo para validar el email del usuario
        public string ValidarEmail(string email)
        {
            // Regex para validar el formato del email
            var regex = new Regex(@"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$");

            if (string.IsNullOrWhiteSpace(email))
            {
                return "El campo está vacío.";
            }

            if (email.Length < 6)
            {
                return "El email debe tener al menos 6 caracteres.";
            }

            if (!regex.IsMatch(email))
            {
                return "El formato del email es invalido.";
            }

            return string.Empty;
        }

        // Metodo para validar la contraseña de un usuario
        public string ValidarContrasenia(string contrasenia)
        {
            // Regex para validar que la contraseña tenga al menos una mayuscula, una minuscula, un numero y un caracter especial
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[+\-_.\/])[A-Za-z\d+\-_.\/]+$");

            if (string.IsNullOrWhiteSpace(contrasenia))
            {
                return "El campo está vacío.";
            }

            if (contrasenia.Length < 6)
            {
                return "La contraseña debe tener al menos 6 caracteres.";
            }

            if (!regex.IsMatch(contrasenia))
            {
                return "Debe incluir mayúscula, minúscula, número y signo (+ - _ . /)";
            }

            return string.Empty;
        }

        // Metodo para validar la confirmacion de contraseña de un usuario
        public string ValidarConfirmarContrasenia(string contrasenia, string confirmarContrasenia)
        {
            if (string.IsNullOrWhiteSpace(confirmarContrasenia))
            {
                return "El campo esta vacío.";
            }

            if (contrasenia != confirmarContrasenia)
            {
                return "Las contraseñas no coinciden.";
            }

            return string.Empty;
        }

        // Metodo para validar el nombre de una rutina
        public string ValidarNombreRutina(string nombreRutina)
        {
            if (string.IsNullOrWhiteSpace(nombreRutina))
            {
                return "El campo está vacío.";
            }

            if (nombreRutina.Length < 2)
            {
                return "El nombre de la rutina debe tener al menos 2 caracteres.";
            }

            return string.Empty;
        }

        // Metodo para validar el Personal Record de un ejercicio
        public string ValidarPR(string pr)
        {
            if (string.IsNullOrWhiteSpace(pr))
            {
                return "El campo está vacío.";
            }

            if (!pr.All(char.IsDigit))
            {
                return "Solo se permiten números enteros positivos.";
            }

            return string.Empty;
        }

        // Metodo para validar las series de un ejercicioDia
        public string ValidarSeries(string series)
        {
            if (string.IsNullOrWhiteSpace(series))
            {
                return "El campo está vacío.";
            }

            if (!series.All(char.IsDigit))
            {
                return "Solo se permiten numeros enteros positivos";
            }

            if (int.TryParse(series, out int seriesInt) && seriesInt <= 0)
            {
                return "Las series deben ser mayores a 0.";
            }

            return string.Empty;
        }

        // Metodo para validar las Repeticiones de un ejercicioDia
        public string ValidarRepeticiones(string repeticiones)
        {

            if (string.IsNullOrWhiteSpace(repeticiones))
            {
                return "El campo está vacío.";
            }

            if (!repeticiones.All(char.IsDigit))
            {
                return "Solo se permiten numeros enteros positivos";
            }

            if (int.TryParse(repeticiones, out int repeticionesInt) && repeticionesInt <= 0)
            {
                return "Las repeticiones deben ser mayores a 0.";
            }

            return string.Empty;
        }

    }
}
