namespace PlanoRevisaoAPI.ModelView;

public class PlanoRevisaoPrecoSemIdModelView
{
    public int IdPlanoRevisaoTipo { get; set; }

    public string? CdEmpresaRegiao { get; set; }

    public decimal? NuValor { get; set; }
    public DateTime DtVigenciaInicial { get; set; }

    public DateTime DtVigenciaFinal { get; set; }
}
