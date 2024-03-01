using System.ComponentModel.DataAnnotations;

public class Log
{
    [Key]
    public int IdLog { get; set; }
    public string CodRob { get; set; }
    public string UsuRob { get; set; }
    public DateTime DateLog { get; set; }
    public string Processo { get; set; }
    public string InfLog { get; set; }
    public int IdProd { get; set; }
}
