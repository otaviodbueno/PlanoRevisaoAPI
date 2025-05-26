namespace PlanoRevisaoAPI.ModelView;

public class TipoRevisaoRequestModelView
{
    public int IdPlanoRevisaoTipo { get; set; }
    public int IdTipoRevisao { get; set; }
    public decimal? UnidadeMaoDeObra { get; set; }
    public string? InReembolsar { get; set; }
}
