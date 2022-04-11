using Android.Content.Res;
using AppCompras.Datos;
using AppCompras.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppCompras.VistaModelo
{
    public class VMagregarcompra : BaseViewModel
    {
        #region VARIABLES
        string _Texto;
        int _Cantidad;
        public Mproductos Parametrosrecibe { get; set; }
        #endregion
        #region CONSTRUCTOR
        public VMagregarcompra(INavigation navigation, Mproductos parametrosTrae)
        {
            Navigation = navigation;
            Parametrosrecibe = parametrosTrae;
        }
        #endregion
        #region OBJETOS
        public string Texto
        {
            get { return _Texto; }
            set { SetValue(ref _Texto, value); }
        }
        public int Cantidad
        {
            get { return _Cantidad; }
            set { SetValue(ref _Cantidad, value); }
        }
        #endregion
        #region PROCESOS
        public async Task InsertarDc()
        {
            if (Cantidad == 0)
            {
                Cantidad = 1;
            }

            var funcion = new Ddetallecompras();
            var parametros = new Mdetallecompra();
            parametros.Cantidad = Cantidad.ToString();
            parametros.Idproducto = Parametrosrecibe.Idproducto;
            parametros.Preciocompra = Parametrosrecibe.Precio;
            double total = 0;
            double preciocompra = Convert.ToDouble(Parametrosrecibe.Precio);
            double cantidad = Convert.ToDouble(Cantidad);
            total = cantidad * preciocompra;
            parametros.Total = total.ToString();
            await funcion.InsertarDc(parametros);
            await Volver();
        }
        public async Task Volver()
        {
            await Navigation.PopAsync();
        }
        public void Aumentar()
        {
            Cantidad += 1;
        }
        public void Disminuir()
        {
            if (Cantidad > 0)
            {
                Cantidad -= 1;
            }
        }
        public async Task ProcesoAsyncrono()
        {

        }
        public void ProcesoSimple()
        {

        }
        #endregion
        #region COMANDOS
        public ICommand Volvercommand => new Command(async () => await Volver());

        public ICommand Aumentarcommand => new Command(Aumentar);

        public ICommand Disminuircommand => new Command(Disminuir);
        public ICommand Insertarcommand => new Command(async () => await InsertarDc());
        #endregion
    }
}
