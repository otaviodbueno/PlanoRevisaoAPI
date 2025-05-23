namespace PlanoRevisaoAPI.ModelView;

public class PlanoRevisaoTipoModelView
{
    public int IdPlanoRevisao { get; set; }
    public List<TipoRevisaoRequestModelView>? TiposRevisao { get; set; }
}
