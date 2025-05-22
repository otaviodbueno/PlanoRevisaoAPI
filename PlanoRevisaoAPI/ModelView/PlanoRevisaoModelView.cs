namespace PlanoRevisaoAPI.ModelView;

public class PlanoRevisaoModelView
{
    public int IdPlanoRevisao { get; set; }
    public string? DsPlanoRevisao { get; set; }
    public int IdLinha { get; set; }
    public DateTime DtInclusao { get; set; }
    public DateTime? DtVigenciaInicial { get; set; }
    public DateTime? DtVigenciaFinal { get; set; }
    public int? NuMesesGarantia { get; set; }
    public int IdPoliticaVenda { get; set; }
}

