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
        private readonly ICombosHelper _combosHelper;
        #endregion

        #region Constructor	
        public ConverterHelper(DataContext dataContext, ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;

        }
        #endregion

        #region Metodos	
        public async Task<Mascota> OjMascotaAsync(MascotaViewModel modelo, string path, bool nuevaMascota)
        {
            var mascota = new Mascota
            {
                Agendas = modelo.Agendas,
                FechaNacimiento = modelo.FechaNacimiento,
                HistorialMedicos = modelo.HistorialMedicos,
                Id = nuevaMascota ? 0 : modelo.Id,
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

        public MascotaViewModel OjMascotaViewModel(Mascota mascota)
        {
            return new MascotaViewModel{
                Agendas = mascota.Agendas,
                FechaNacimiento = mascota.FechaNacimiento,
                HistorialMedicos = mascota.HistorialMedicos,
                UrlImagen = mascota.UrlImagen,
                Nombre = mascota.Nombre,
                Cliente = mascota.Cliente,
                TipoMascota =mascota.TipoMascota,
                Rasa =mascota.Rasa,
                Comentarios = mascota.Comentarios,
                Id = mascota.Id,
                ClienteId = mascota.Cliente.Id,
                TipoMascotaId =mascota.TipoMascota.Id,
                TipoMascotas = _combosHelper.GetComboTipoMascota() 
            };
        }
        #endregion




    }
}
