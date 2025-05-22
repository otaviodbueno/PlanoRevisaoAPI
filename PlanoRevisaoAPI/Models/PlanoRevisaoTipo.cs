using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanoRevisaoAPI.Models;

[Table("PLANOREVISAOTIPO")]

public class PlanoRevisaoTipo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_PLANO_REVISAO_TIPO { get; set; }

    public int ID_TIPO_REVISAO { get; set; }

    public int ID_PLANO_REVISAO { get; set; }

    public decimal? UNIDADE_MAO_DE_OBRA { get; set; }

    public string? IN_REEMBOLSAR { get; set; }

    [ForeignKey("ID_PLANO_REVISAO")]
    public virtual PlanoRevisao? PlanoRevisao { get; set; }

    [ForeignKey("ID_TIPO_REVISAO")]
    public virtual TipoRevisao? TipoRevisao { get; set; }

    public virtual ICollection<PlanoRevisaoPreco>? PlanoRevisaoPreco { get; set; }
}
