using EMS.ConsultaAberta.SeedWork;

namespace EMS.ConsultaAberta.QueryModel;

public class ListagemMateriaisModel
{
    public Guid Id { get; init; }
    public string Descricao { get; init; }
}

public class RecuperarMateriaisPaginadosModel : PaginationModel<ListagemMateriaisModel> { }