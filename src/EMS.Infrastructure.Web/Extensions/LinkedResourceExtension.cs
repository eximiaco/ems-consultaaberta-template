using EMS.Infrastructure.Web.ViewModels;

namespace EMS.Infrastructure.Web.Extensions;

public static class LinkedResourceExtension
{
    public static void AddResourceLink(this ILinkedResource resources, LinkedResourceType resourceType, string routeUrl)
    {
        resources.Links ??= new Dictionary<LinkedResourceType, LinkedResource>();
        resources.Links[resourceType] = new LinkedResource(routeUrl);
    }
}