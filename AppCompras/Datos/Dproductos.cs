using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Database;
using AppCompras.Modelo;
using AppCompras.Conexiones;

namespace AppCompras.Datos
{
    public class Dproductos
    {
        public async Task<List<Mproductos>> MostrarProductos()
        {
            return (await Cconexion.firebase
                .Child("Productos")
                .OnceAsync<Mproductos>()).Select(item => new Mproductos
                {
                    Descripcion = item.Object.Descripcion,
                    Icono = item.Object.Icono,
                    Precio = item.Object.Precio,
                    Peso = item.Object.Peso,
                    Idproducto = item.Key
                }).ToList();
                
        }
        // replicando el metodo, pero poir Id

        public async Task<List<Mproductos>> MostrarProductosXid(Mproductos parametro)
        {
            return (await Cconexion.firebase
                .Child("Productos")

                .OnceAsync<Mproductos>()
                ).Where(a=>a.Key==parametro.Idproducto).Select(item => new Mproductos
                {
                    Descripcion = item.Object.Descripcion,
                    Icono = item.Object.Icono,
                    Precio = item.Object.Precio,
                    Peso = item.Object.Peso,
                    Idproducto = item.Key
                }).ToList();

        }
    }
}
