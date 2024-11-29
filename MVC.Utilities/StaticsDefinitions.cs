using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Utilities
{
    public static class SD
    {
        public const string OK = "OK";
        public const string ERROR = "Error";

        public const string ImagenRoute = @"\images\products\";
        public const string ssShoppinCart = "Sesion carro Compras";

        public const string Role_Admin = "Admin";
        public const string Role_Client = "Cliente";
        public const string Role_Storage = "Inventario";

        // States de la Orden
        public const string StatePending = "Pendiente";
        public const string StateAppproved = "Aprobado";
        public const string StateInProcess = "Procesando";
        public const string StateSend = "Enviado";
        public const string StateCancel = "Cancelado";
        public const string StateReturned = "Devuelto";
        // State del Pago de la Orden
        public const string PayStatePending = "Pendiente";
        public const string PayStateApproved = "Aprobado";
        public const string PayStateDelayed = "Retrasado";
        public const string PayStateRefusedo = "Rechazado";
    }
}
