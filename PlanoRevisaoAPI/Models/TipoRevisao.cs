using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanoRevisaoAPI.Models;

[Table("TIPOREVISAO")]
public class TipoRevisao
{
    [Key]
    public int IdTipoRevisao { get; set; }

    public string NmRevisao { get; set; }

    public DateTime? DtInclusao { get; set; }

    public bool InAtivo { get; set; }

    public decimal NuRevisao { get; set; }

    public virtual ICollection<PlanoRevisaoTipo> PlanoRevisaoTipo { get; set; }
}
