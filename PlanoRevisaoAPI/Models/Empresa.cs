using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanoRevisaoAPI.Models;

[Table("EMPRESA")]
public class Empresa
{
    [Key]
    public int IdEmpresa { get; set; }

    public string? NuCnpj { get; set; }

    public string NmEmpresa { get; set; }

    public int IdEmpresaRegiao { get; set; }

    public DateTime? DtInclusao { get; set; }

    public bool InAtivo { get; set; }

    public virtual EmpresaRegiao IdEmpresaRegiaoNavigation { get; set; } = null!;
}
