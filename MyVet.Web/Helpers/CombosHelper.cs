using Microsoft.AspNetCore.Mvc.Rendering;
using MyVet.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;
        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;

        }

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
    }
}
