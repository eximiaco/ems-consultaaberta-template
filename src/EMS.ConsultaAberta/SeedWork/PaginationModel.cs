namespace EMS.ConsultaAberta.SeedWork;

public class PaginationModel<T>
{
    public int PaginaAtual { get; init; }

    public long? TotalItens { get; init; }

    public int TotalPaginas { get; init; }

    public List<T> Itens { get; init; }
}