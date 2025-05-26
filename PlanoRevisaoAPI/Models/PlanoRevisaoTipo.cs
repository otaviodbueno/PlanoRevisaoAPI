using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public virtual PlanoRevisao? PlanoRevisao { get; set; }

    [ForeignKey("ID_TIPO_REVISAO")]
    [JsonIgnore]
    public virtual TipoRevisao? TipoRevisao { get; set; }
    [JsonIgnore]
    public virtual ICollection<PlanoRevisaoPreco>? PlanoRevisaoPreco { get; set; }
}
