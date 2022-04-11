using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database.Query;
using System.Linq;
using System.Threading.Tasks;
using AppCompras.Modelo;
using AppCompras.Conexiones;

namespace AppCompras.Datos
{
   public class Ddetallecompras
    {
        public async Task InsertarDc(Mdetallecompra  parametros)
        {
            await Cconexion.firebase
                .Child("Detallecompra") // la base de datos de firebase es esta
                .PostAsync(new Mdetallecompra()
                {
                    Cantidad = parametros.Cantidad,
                    Idproducto = parametros.Idproducto,
                    Preciocompra = parametros.Preciocompra,
                    Total = parametros.Total
                });
        }
    }
}
