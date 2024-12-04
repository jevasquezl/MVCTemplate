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
        public const string Role_Storage = "Inventory";

        // States de la Orden
        public const string StateOrderPending = "Pending";
        public const string StateOrderApproved = "Approved";
        public const string StateOrderInProcess = "InProcess";
        public const string StateOrderSend = "Sended";
        public const string StateOrderCancel = "Canceled";
        public const string StateOrderReturned = "Returned";
        // State del Pago de la Orden
        public const string StatePayedPending = "Pending";
        public const string StatePayedApproved = "Approved";
        public const string StatePayedDelayed = "Delayed";
        public const string StatePayedCancel = "Canceled";
        public const string StatePayedRejected = "Rejected";
    }
}
