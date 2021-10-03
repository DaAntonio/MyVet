#region Using
using MyVet.Web.Data.Entidades;
using MyVet.Web.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace MyVet.Web.Data
{
    public class AlimentadorDB
    {
        private readonly DataContext _dataContext;
        private readonly IUsuarioHelper _usuarioHelper;

        public AlimentadorDB(
            DataContext context,
            IUsuarioHelper usuarioHelper)
        {
            _dataContext = context;
            _usuarioHelper = usuarioHelper;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckRoles();
            var manager = await CheckUserAsync("1010", "Juan", "Zuluaga", "jzuluaga55@gmail.com", "350 634 2747", "Calle Luna Calle Sol", "Admin");
            var customer = await CheckUserAsync("2020", "Juan", "Zuluaga", "jzuluaga55@hotmail.com", "350 634 2747", "Calle Luna Calle Sol", "Customer");
            await CheckPetTypesAsync();
            await CheckTipoServiciosAsync();
            await CheckOwnerAsync(customer);
            await CheckManagerAsync(manager);
            await CheckPetsAsync();
            //await CheckAgendasAsync();
        }

        private async Task CheckRoles()
        {
            await _usuarioHelper.CheckRolAsync("Admin");
            await _usuarioHelper.CheckRolAsync("Customer");
        }

        private async Task<Usuario> CheckUserAsync(
            string documento,
            string nombre,
            string apellidos,
            string email,
            string phone,
            string dieccion,
            string rol)
        {
            var user = await _usuarioHelper.GetUsuarioByEmailAsync(email);
            if (user == null)
            {
                user = new Usuario
                {
                    Nombre = nombre,
                    Apellidos = apellidos,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Direccion = dieccion,
                    Documento = documento
                };

                await _usuarioHelper.AddUsuarioAsync(user, "123456");
                await _usuarioHelper.AddUsuarioToRolAsync(user, rol);
            }
            return user;
        }

        private async Task CheckPetsAsync()
        {
            if (!_dataContext.Mascotas.Any())
            {
                var cliente = _dataContext.Clientes.FirstOrDefault();
                var tipoMascota = _dataContext.TipoMascotas.FirstOrDefault();
                AddPet("Otto", cliente, tipoMascota, "Shih tzu");
                AddPet("Killer", cliente, tipoMascota, "Dobermann");
                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckTipoServiciosAsync()
        {
            if (!_dataContext.TipoServicios.Any())
            {
                _dataContext.TipoServicios.Add(new TipoServicio { Valor = "Consulta" });
                _dataContext.TipoServicios.Add(new TipoServicio { Valor = "Urgencia" });
                _dataContext.TipoServicios.Add(new TipoServicio { Valor = "Vacunación" });
                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckPetTypesAsync()
        {
            if (!_dataContext.TipoMascotas.Any())
            {
                _dataContext.TipoMascotas.Add(new TipoMascota { Valor = "Perro" });
                _dataContext.TipoMascotas.Add(new TipoMascota { Valor = "Gato" });
                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckOwnerAsync(Usuario user)
        {
            if (!_dataContext.Clientes.Any())
            {
                _dataContext.Clientes.Add(new Cliente { Usuario = user });
                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckManagerAsync(Usuario user)
        {
            if (!_dataContext.Managers.Any())
            {
                _dataContext.Managers.Add(new Manager { Usuario = user });
                await _dataContext.SaveChangesAsync();
            }
        }

        private void AddPet(string nombre, Cliente cliente, TipoMascota tipoMascota, string rasa)
        {
            _dataContext.Mascotas.Add(new Mascota
            {
                FechaNacimiento = DateTime.Now.AddYears(-2),
                Nombre = nombre,
                Cliente = cliente,
                TipoMascota = tipoMascota,
                Rasa = rasa
            });
        }

        private async Task CheckAgendasAsync()
        {
            if (!_dataContext.Agendas.Any())
            {
                var initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                var finalDate = initialDate.AddYears(1);
                while (initialDate < finalDate)
                {
                    if (initialDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var finalDate2 = initialDate.AddHours(10);
                        while (initialDate < finalDate2)
                        {
                            _dataContext.Agendas.Add(new Agenda
                            {
                                Fecha = initialDate,
                                Disponibilidad = true
                            });

                            initialDate = initialDate.AddMinutes(30);
                        }

                        initialDate = initialDate.AddHours(14);
                    }
                    else
                    {
                        initialDate = initialDate.AddDays(1);
                    }
                }
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}
