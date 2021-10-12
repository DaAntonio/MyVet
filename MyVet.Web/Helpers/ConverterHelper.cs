#region Using
using MyVet.Web.Data;
using MyVet.Web.Data.Entidades;
using MyVet.Web.Models;
using System.Threading.Tasks;
#endregion
namespace MyVet.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        #region Variables
        private readonly DataContext _dataContext;
        #endregion

        #region Constructor	
        public ConverterHelper(DataContext dataContext)
        {
            _dataContext = dataContext;

        }
        #endregion

        #region Metodos	
        public async Task<Mascota> OMascotaAsync(MascotaViewModel modelo, string path, bool isNew)
        {

            var mascota = new Mascota
            {
                Agendas = modelo.Agendas,
                FechaNacimiento = modelo.FechaNacimiento,
                HistorialMedicos = modelo.HistorialMedicos,
                Id = isNew ? 0 : modelo.Id,
                UrlImagen = path,
                Nombre = modelo.Nombre,
                Cliente = await _dataContext.Clientes.FindAsync(modelo.ClienteId),
                TipoMascota = await _dataContext.TipoMascotas.FindAsync(modelo.TipoMascotaId),
                Rasa = modelo.Rasa,
                Comentarios = modelo.Comentarios
            };
            if (modelo.Id != 0)
            {
                mascota.Id = modelo.Id;
            }

            return mascota;
        }

        #endregion




    }
}
