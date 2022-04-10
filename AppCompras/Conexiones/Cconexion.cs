using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;

namespace AppCompras.Conexiones
{
    public class Cconexion
    {
        // aqui se conecta directamente a firebase
        public static FirebaseClient firebase = new FirebaseClient("https://appcompras-2752e-default-rtdb.firebaseio.com/");
    }
}
