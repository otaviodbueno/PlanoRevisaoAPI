using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanoRevisaoAPI.Models;

[Table("POLITICAVENDA")]
public class PoliticaVenda
{
    [Key]
    public int IdPoliticaVenda { get; set; }

    public string? DsPoliticaVenda { get; set; }

    public virtual ICollection<PlanoRevisao> Planorevisaos { get; set; }
}
