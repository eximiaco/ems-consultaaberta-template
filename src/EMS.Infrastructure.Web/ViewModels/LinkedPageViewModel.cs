namespace EMS.Infrastructure.Web.ViewModels;

public class LinkedPageViewModel<T> : ILinkedResource
{
    public int CurrentPage { get; init; }

    public long? TotalItems { get; init; }

    public int TotalPages { get; init; }

    public List<T> Items { get; init; }
    public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
}