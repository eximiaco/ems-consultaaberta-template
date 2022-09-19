namespace EMS.Infrastructure.Web.ViewModels;

public interface ILinkedResource
{
    public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
}