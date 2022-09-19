using EMS.Infrastructure.Web.Extensions;
using EMS.Infrastructure.Web.InputModels;
using EMS.Infrastructure.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Infrastructure.Web.Services;

public static class LinkedPageResultService
{
    public static LinkedPageViewModel<T> GeneratePageLinks<T, TParam>(this IUrlHelper url, 
        LinkedPageViewModel<T> linkedViewModel, 
        TParam queryParameters, 
        string route) where TParam : IPaginationQueryParam
    {
        if (linkedViewModel.CurrentPage > 1)
        {
            var prevRoute = url.RouteUrl(route,  queryParameters.GetRouteParam(queryParameters.Page - 1));
            if (prevRoute != null) 
                linkedViewModel.AddResourceLink(LinkedResourceType.Prev, prevRoute);
        }

        if (linkedViewModel.CurrentPage >= linkedViewModel.TotalPages) 
            return linkedViewModel;
        
        var nextRoute = url.RouteUrl(route, queryParameters.GetRouteParam(queryParameters.Page + 1));
        if (nextRoute != null) 
            linkedViewModel.AddResourceLink(LinkedResourceType.Next, nextRoute);

        return linkedViewModel;
    }
}