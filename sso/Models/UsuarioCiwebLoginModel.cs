namespace sso.Models
{
    public class UsuarioCiwebLoginModel
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public bool Ativo { get; set; }
        public bool EmManutencao { get; set; }

    }
}