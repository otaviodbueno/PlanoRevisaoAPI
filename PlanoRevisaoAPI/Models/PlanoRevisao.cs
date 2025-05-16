using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PlanoRevisaoAPI.Models;


[Table("PLANOREVISAO")]
public class PlanoRevisao
{
    [Key]
    public int IdPlanoRevisao { get; set; }

    public string DsPlanoRevisao { get; set; } = null!;

    public int IdLinha { get; set; }

    public DateTime DtInclusao { get; set; }

    public DateTime? DtVigenciaInicial { get; set; }

    public DateTime? DtVigenciaFinal { get; set; }

    public int? NuMesesGarantia { get; set; }

    public int IdPoliticaVenda { get; set; }

    [ForeignKey("IdLinha")]
    public virtual Linha Linha { get; set; }

    [ForeignKey("IdPoliticaVenda")]
    public virtual PoliticaVenda PoliticaVenda { get; set; } = null!;

    public virtual ICollection<Planorevisaotipo> Planorevisaotipos { get; set; } = new List<Planorevisaotipo>();
}
