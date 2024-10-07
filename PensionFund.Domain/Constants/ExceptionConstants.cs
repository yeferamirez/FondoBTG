namespace PensionFund.Domain.Constants
{
    public static class ExceptionConstants
    {
        public const string NOT_VALID_VALUE = "No tiene saldo disponible para vincularse al fondo";
        public const string NOT_VALID_CLIENT = "El usuario ya pertenece a ese fondo";
        public const string NOT_EXIST_CLIENT = "El usuario no pertenece a ese fondo";
        public const string NOT_VALID_AMOUNT = "El usuario no posee el monto suficiente";
        public const string NOT_EXIST_CLIENT_CITY = "La ciudad no posee clientes asociados";
        public const string NOT_EXIST_PRODUCT = "No hay poductos disponibles";
        public const string NOT_SEND_SMS = "No fue posible notificar por sns";
    }
}
