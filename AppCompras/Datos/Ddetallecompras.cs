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
                    Iddetallecompra = item.Key,
                    Cantidad = item.Object.Cantidad,
                    Total = item.Object.Total,
                    
                })
                ; //  esto trae la conexion con la bd y trae la data asigna a este


            foreach (var hobit in data)
            {
                var parametros = new Mdetallecompra();
                parametros.Idproducto = hobit.Idproducto;
                parametrosProductos.Idproducto = hobit.Idproducto;
                var listaproductos = await funcionproductos.MostrarProductosXid(parametrosProductos);

                parametros.Imagen = listaproductos[0].Icono;
                parametros.Descripcion = listaproductos[0].Descripcion;
                parametros.Cantidad = hobit.Cantidad;
                parametros.Total = hobit.Total;
                ListaDc.Add(parametros);
            }
            return ListaDc;
        }

        public async Task<List<Mdetallecompra>> MostrarDcXidproducto(string idproducto)
        {
            return (await Cconexion.firebase
                .Child("Detallecompra")
                .OnceAsync<Mdetallecompra>()
                ).Where(a=> a.Object.Idproducto == idproducto).Select(item => new Mdetallecompra
                {
                    Total = item.Object.Total
                }).ToList();
        }
        public async Task Editardetalle(Mdetallecompra parametros)
        {
            var data = (await Cconexion.firebase
                .Child("Detallecompra")
                .OnceAsync<Mdetallecompra>())
                .Where(a => a.Object.Idproducto == parametros.Idproducto)
                .FirstOrDefault();
            double cantidadInicial = Convert.ToDouble(data.Object.Cantidad);
            data.Object.Cantidad = (cantidadInicial + Convert.ToDouble(parametros.Cantidad)).ToString();
            double cantidad = Convert.ToDouble(data.Object.Cantidad);
            double preciocompra = Convert.ToDouble(parametros.Preciocompra);
            double total = 0;
            total = cantidad * preciocompra;
            data.Object.Total = total.ToString();

            await Cconexion.firebase
                .Child("Detallecompra")
                .Child(data.Key)
                .PutAsync(data.Object);
        }

        public async Task<string> MostrarTotales()
        {
            var funcion = new Ddetallecompras();
            var lista = await funcion.Mostrarcantidades();
            double totales = 0;
            foreach(var hobit in lista)
            {
                totales += Convert.ToDouble(hobit.Totales);
            }
            return totales.ToString();
        }
        public async Task<List<Mdetallecompra>> Mostrarcantidades()
        {
            return (await Cconexion.firebase
                .Child("Detallecompra")
                .OnceAsync<Mdetallecompra>()
                ).Where(a=> a.Key != "Modelo").Select(item => new Mdetallecompra
                {
                    Cantidad = item.Object.Cantidad
                }).ToList();
        }
        public async Task <string> Sumarcantidad()
        {
            var funcion = new Ddetallecompras();
            var lista = await funcion.Mostrarcantidades();
            double cantidad = 0;
            foreach(var hobit in lista)
            {
                cantidad += Convert.ToDouble(hobit.Cantidad);                
            }
            return cantidad.ToString();
        }
    }
}
