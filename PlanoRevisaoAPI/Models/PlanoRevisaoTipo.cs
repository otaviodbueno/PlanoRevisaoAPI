using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanoRevisaoAPI.Models;

[Table("PLANOREVISAOTIPO")]

public class PlanoRevisaoTipo
{
    [Key]
    public int IdPlanoRevisaoTipo { get; set; }

    public int IdTipoRevisao { get; set; }

    public int IdPlanoRevisao { get; set; }

    public decimal? UnidadeMaoDeObra { get; set; }

    public string? InReembolsar { get; set; }

    [ForeignKey("IdPlanoRevisao")]
    public virtual PlanoRevisao PlanoRevisao { get; set; }

    [ForeignKey("IdTipoRevisao")]
    public virtual TipoRevisao TipoRevisao { get; set; }

    public virtual ICollection<PlanoRevisaoPreco> PlanoRevisaoPreco { get; set; }
}
