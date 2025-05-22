using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlanoRevisaoAPI.Models;

[Table("EMPRESAREGIAO")]
public partial class EmpresaRegiao
{
    [Key]
    public int ID_EMPRESA_REGIAO { get; set; }
    public string? NM_REGIAO { get; set; }
    public string? CD_GRUPO_REGIAO { get; set; }
    [JsonIgnore]
    public virtual ICollection<Empresa> Empresas { get; set; }
    [JsonIgnore]
    public virtual ICollection<PlanoRevisaoPreco> Planorevisaoprecos { get; set; }
}
