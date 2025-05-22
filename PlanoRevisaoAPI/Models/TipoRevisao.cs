using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlanoRevisaoAPI.Models;

[Table("TIPOREVISAO")]
public class TipoRevisao
{
    [Key]
    public int ID_TIPO_REVISAO { get; set; }

    public string NM_REVISAO { get; set; }

    public DateTime? DT_INCLUSAO { get; set; }

    public bool IN_ATIVO { get; set; }

    public decimal NU_REVISAO { get; set; }

    [JsonIgnore]
    public virtual ICollection<PlanoRevisaoTipo>? PlanoRevisaoTipo { get; set; }
}
