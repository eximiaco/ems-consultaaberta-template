using System.Runtime;
using EMS.ConsultaAberta.SeedWork;

namespace EMS.ConsultaAberta.QueryModel;

public sealed class Material : Entity<Guid>
{
    public Material(Guid id, string codigoAnvisa, string nomeTecnico) : base(id)
    {
        CodigoAnvisa = codigoAnvisa;
        NomeTecnico = nomeTecnico;
    }

    public string CodigoAnvisa { get; set; }
    public string NomeTecnico { get; set; }
}