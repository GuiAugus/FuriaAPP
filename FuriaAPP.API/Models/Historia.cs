namespace FuriaAPP.API.Models
{
    public class Historia
    {
        public int Id { get; set;}
        public string Titulo { get; set;} = string.Empty;
        public string Conteudo { get; set;} = string.Empty;
        public DateTime Data { get; set;}
    }
}