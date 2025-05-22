using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace PlanoRevisaoAPI.Models;


[Table("PLANOREVISAO")]
public class PlanoRevisao
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID_PLANO_REVISAO { get; set; }

    public string? DS_PLANO_REVISAO { get; set; }

    public int ID_LINHA { get; set; }

    public DateTime DT_INCLUSAO { get; set; }

    public DateTime? DT_VIGENCIA_INICIAL { get; set; }

    public DateTime? DT_VIGENCIA_FINAL { get; set; }

    public int? NU_MESES_GARANTIA { get; set; }

    public int ID_POLITICA_VENDA { get; set; }

    [ForeignKey("ID_PLANO_REVISAO")]
    [JsonIgnore]
    public virtual Linha? Linha { get; set; }

    [ForeignKey("ID_POLITICA_VENDA")]
    [JsonIgnore]
    public virtual PoliticaVenda? PoliticaVenda { get; set; }

    [JsonIgnore]
    public virtual ICollection<PlanoRevisaoTipo>? PlanoRevisaoTipo { get; set; }
}
