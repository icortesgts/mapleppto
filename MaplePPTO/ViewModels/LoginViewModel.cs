namespace MaplePPTO.ViewModels
{
    public class LoginViewModel
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string ReturnURL { get; set; }
        public bool Recordar { get; set; }
        public int Code{get;set;}
    public string Message { get; set; }

    }
}