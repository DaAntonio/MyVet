#region Using
using Microsoft.AspNetCore.Mvc.Rendering;
using MyVet.Web.Data;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace MyVet.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        #region Variables

        private readonly DataContext _dataContext;

        #endregion

        #region Constructor

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;

        }

        #endregion

        #region Metodos

        public IEnumerable<SelectListItem> GetComboTipoMascota()
        {
            var list = _dataContext.TipoMascotas.Select(pt => new SelectListItem
            {
                Text = pt.Valor,
                Value = $"{pt.Id}"
            })
                .OrderBy(pt => pt.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Selecciona una opcion...]",
                Value = "0"
            });

            return list;

        }

        public IEnumerable<SelectListItem> GetComboTipoServicio()
        {
            var list = _dataContext.TipoServicios.Select(pt => new SelectListItem
            {
                Text = pt.Valor,
                Value = $"{pt.Id}"
            })
                            .OrderBy(pt => pt.Text)
                            .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Selecciona una opcion...]",
                Value = "0"
            });

            return list;
        }
        
        #endregion
    }
}
