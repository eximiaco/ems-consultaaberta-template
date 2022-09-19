namespace EMS.Infrastructure.Web.InputModels;

public interface IPaginationQueryParam
{
    public int Limit { get; }
    public int Page { get; }
    public object GetRouteParam(int newPage);
}