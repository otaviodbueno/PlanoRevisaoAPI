namespace PlanoRevisaoAPI.ModelView;

public class TipoRevisaoModelView
{
    public int IdTipoRevisao { get; set; }
    public string NmRevisao { get; set; }
    public DateTime? DtInclusao { get; set; }
    public int InAtivo { get; set; }
    public decimal NuRevisao { get; set; }

}
