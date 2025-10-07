using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainiumNeon.Models;

namespace TrainiumNeon.Services
{
    interface IApiEjerciciosService
    {
        Task<IReadOnlyList<EjercicioModel>> GetEjerciciosAsync();

        Task<EjercicioModel> GetEjercicioByIdAsync(int id);

        Task<IReadOnlyList<EjercicioModel>> GetEjerciciosByGrupoMuscularAsync(string grupoMuscular);
    }
}
