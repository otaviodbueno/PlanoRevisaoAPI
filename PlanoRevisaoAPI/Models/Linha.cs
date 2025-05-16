using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlanoRevisaoAPI.Models;

[Table("LINHA")] 
public class Linha
{
    [Key]
    public int ID_LINHA { get; set; }

    public string? CD_LINHA { get; set; }

    public string? NM_LINHA { get; set; }

    public bool IN_ATIVO { get; set; }

    [JsonIgnore]
    public virtual ICollection<PlanoRevisao> PlanosRevisao { get; set; }
}
