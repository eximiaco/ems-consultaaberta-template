using EMS.Infrastructure.Web.InputModels;

namespace EMS.ConsultaAberta.HttpServelessApi.InputModels;

public record RecuperarMateriaisInputModel(string CodigoAnvisa = "", int Limit = 50, int Page = 1) : IPaginationQueryParam
{
    public object GetRouteParam(int newPage) => new
    {
        codigoAnvisa = CodigoAnvisa,
        limit = Limit,
        page = newPage
    };
}