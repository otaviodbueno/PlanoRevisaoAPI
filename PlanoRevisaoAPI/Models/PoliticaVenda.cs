using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlanoRevisaoAPI.Models;

[Table("POLITICAVENDA")]
public class PoliticaVenda
{
    [Key]
    public int ID_POLITICA_VENDA { get; set; }

    public string? DS_POLITICA_VENDA { get; set; }

    [JsonIgnore]
    public virtual ICollection<PlanoRevisao> Planorevisao { get; set; }
}
