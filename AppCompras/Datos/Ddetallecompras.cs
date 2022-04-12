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
        public async Task<List<Mdetallecompra>> MostrarVistapreviaDc()
        {
            var ListaDc = new List<Mdetallecompra>(); // en esta lista se van a ir añadiendo cadauno de los elementos que recorra la funcion anterior...

            var parametrosProductos = new Mproductos();

            // funcion para traer la imagen a traves del id
            var funcionproductos = new Dproductos();


            var data = (await Cconexion.firebase
                .Child("Detallecompra")
                .OnceAsync<Mdetallecompra>())
                .Where(a=> a.Key != "Modelo")
                .Select(item=>new Mdetallecompra
                { 
                    Idproducto = item.Object.Idproducto,
                    Iddetallecompra = item.Key
                })
                ; //  esto trae la conexion con la bd y trae la data asigna a este
            
              
            foreach(var hobit in data)
            {
                var parametros = new Mdetallecompra();
                parametros.Idproducto = hobit.Idproducto;
                parametrosProductos.Idproducto = hobit.Idproducto;
                var listaproductos = await funcionproductos.MostrarProductosXid(parametrosProductos);

                parametros.Imagen = listaproductos[0].Icono;
                ListaDc.Add(parametros);
            }
            return ListaDc;
        }

        public async Task<List<Mdetallecompra>> MostrarDc()
        {
            var ListaDc = new List<Mdetallecompra>(); // en esta lista se van a ir añadiendo cadauno de los elementos que recorra la funcion anterior...

            var parametrosProductos = new Mproductos();

            // funcion para traer la imagen a traves del id
            var funcionproductos = new Dproductos();


            var data = (await Cconexion.firebase
                .Child("Detallecompra")
                .OnceAsync<Mdetallecompra>())
                .Where(a => a.Key != "Modelo")
                .Select(item => new Mdetallecompra
                {
                    Idproducto = item.Object.Idproducto,
                    Iddetallecompra = item.Key
                })
                ; //  esto trae la conexion con la bd y trae la data asigna a este


            foreach (var hobit in data)
            {
                var parametros = new Mdetallecompra();
                parametros.Idproducto = hobit.Idproducto;
                parametrosProductos.Idproducto = hobit.Idproducto;
                var listaproductos = await funcionproductos.MostrarProductosXid(parametrosProductos);

                parametros.Imagen = listaproductos[0].Icono;
                parametros.Cantidad = hobit.Cantidad;
                parametros.Total = hobit.Total;
                ListaDc.Add(parametros);
            }
            return ListaDc;
        }
    }
}
