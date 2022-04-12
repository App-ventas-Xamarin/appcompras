using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using AppCompras.Modelo;
using Android.Print;
using AppCompras.Datos;
using AppCompras.Vistas;
using Plugin.SharedTransitions;

namespace AppCompras.VistaModelo
{
    public class VMcompras:BaseViewModel
    {
        #region VARIABLES
        string _Texto;
        int _index;
        string _cantidadtotal;
        string _totalesc;
        List<Mproductos> _listaproductos;
        List<Mdetallecompra> _listaVistapreviaDc;
        List<Mdetallecompra> _listaDc;
        bool _IsvisiblePaneldetallecompra;
        #endregion
        #region CONSTRUCTOR
        public VMcompras(INavigation navigation, StackLayout Carrilderecha, StackLayout Carrilizquierda)
        {
            Navigation = navigation;
            Mostrarproductos(Carrilderecha, Carrilizquierda);
            IsvisiblePanelDc = false;
        }
        #endregion
        #region OBJETOS

        public string Totalesc
        {
            get { return _totalesc; }
            set { SetValue(ref _totalesc, value); }
        }
        public string Cantidadtotal
        {
            get { return _cantidadtotal; }
            set { SetValue(ref _cantidadtotal, value); }
        }
        public bool IsvisiblePanelDc
        {
            get { return _IsvisiblePaneldetallecompra; }
            set { SetValue(ref _IsvisiblePaneldetallecompra, value); }
        }
        public List<Mdetallecompra> ListaVistapreviaDc
        {
            get { return _listaVistapreviaDc; }
            set { SetValue(ref _listaVistapreviaDc, value); }
        }
        public List<Mproductos> Listaproductos
        {
            get { return _listaproductos; }
            set { SetValue(ref _listaproductos, value); }
        }

        public List<Mdetallecompra> ListaDc
        {
            get { return _listaDc; }
            set { SetValue(ref _listaDc, value); }
        }
        #endregion
        #region PROCESOS

        public async Task Mostrarproductos(StackLayout Carrilderecha, StackLayout Carrilizquierda)
        {
            var funcion = new Dproductos();
            Listaproductos = await funcion.MostrarProductos();
            var box = new BoxView
            {
                HeightRequest = 0
            };
            Carrilizquierda.Children.Clear();
            Carrilderecha.Children.Clear();
            Carrilderecha.Children.Add(box);
            foreach(var item in Listaproductos)
            {
                Dibujarproductos(item, _index, Carrilderecha, Carrilizquierda);
                _index++;
            }
        }

        public void Dibujarproductos(Mproductos item, int index, StackLayout Carrilderecha, StackLayout Carriizquierda)
        {
            var _ubicacion = Convert.ToBoolean(index % 2);
            var carril = _ubicacion ? Carrilderecha : Carriizquierda;

            var frame = new Frame
            {
                HeightRequest = 300,
                CornerRadius = 10,
                Margin = 8,
                HasShadow = false,
                BackgroundColor = Color.White,
                Padding = 22
            };
            var stack = new StackLayout
            {

            };
            var image = new Image
            {
                Source = item.Icono,
                HeightRequest = 150,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0.10)
        };

            var labelprecio = new Label
            {
                Text = "$" + item.Precio,
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                Margin = new Thickness(0,10),
                TextColor = Color.FromHex("#333333")
            };

            var labeldescripcion = new Label
            {
                Text = item.Descripcion,
                FontSize = 16,
                TextColor = Color.Black,
                CharacterSpacing = 1
            };

            var labelpeso = new Label
            {
                Text = item.Peso,
                FontSize = 13,
                TextColor = Color.FromHex("#cccccc"),
                CharacterSpacing = 1
            };

            stack.Children.Add(image);
            stack.Children.Add(labelprecio);
            stack.Children.Add(labeldescripcion);
            stack.Children.Add(labelpeso);
            // todas esta area se encarga de traer basicamente los items
            // e irlos renderizandolos en la pantalla del celular...
            frame.Content = stack;
            var tap = new TapGestureRecognizer();


            tap.Tapped += async (object sender, EventArgs e) =>
            {
                var page = (App.Current.MainPage as SharedTransitionNavigationPage).CurrentPage;
                SharedTransitionNavigationPage.SetBackgroundAnimation(page, BackgroundAnimation.SlideFromBottom);
                SharedTransitionNavigationPage.SetTransitionDuration(page, 1000);
                SharedTransitionNavigationPage.SetTransitionSelectedGroup(page, item.Idproducto);
                await Navigation.PushAsync(new Agregarcompra(item));
            };


            carril.Children.Add(frame);
            stack.GestureRecognizers.Add(tap); // al hacer clic a cualquier imagen se va accionar

        }


        public async Task ProcesoAsyncrono()
        {

        }
        public void ProcesoSimple()
        {

        }

        public async Task MostrarVistapreviaDc()
        {
            var funcion = new Ddetallecompras();
            ListaVistapreviaDc = await funcion.MostrarVistapreviaDc();
        }
        public async Task MostrarpanelDc(Grid gridproductos, StackLayout paneldetalleC,
           StackLayout panelcontador)
        {
            uint duracion = 700; // la duracion de la animacion
            await Task.WhenAll(
                panelcontador.FadeTo(0, 500),
                gridproductos.TranslateTo(0, -200, duracion + 200, Easing.CubicIn),
                paneldetalleC.TranslateTo(0, -200, duracion, Easing.CubicIn)
                );
            IsvisiblePanelDc = true;
        }
        //  segunda animacion

        public async Task MostrargridProductos(Grid gridproductos, StackLayout paneldetalleC,
          StackLayout panelcontador)
        {
            uint duracion = 700; // la duracion de la animacion
            await Task.WhenAll(
                panelcontador.FadeTo(1, 500),
                gridproductos.TranslateTo(0, 0, duracion + 200, Easing.CubicIn),
                paneldetalleC.TranslateTo(0, 1000, duracion, Easing.CubicIn)
                );
            IsvisiblePanelDc = false;
        }

        public async Task MostrarDetalleC()
        {
            var funcion = new Ddetallecompras();
            ListaDc = await funcion.MostrarDc();
        }
        public async Task Sumarcantidades()
        {
            var funcion = new Ddetallecompras();
            Cantidadtotal = await funcion.Sumarcantidad();
                
        }
        public async Task MostrarTotales()
        {
            var funcion = new Ddetallecompras();
            Totalesc = await funcion.MostrarTotales();
        }
        #endregion
        #region COMANDOS
        public ICommand ProcesoAsyncommand => new Command(async () => await ProcesoAsyncrono());
        public ICommand ProcesoSimpcommand => new Command(ProcesoSimple);
        #endregion
    }
}
