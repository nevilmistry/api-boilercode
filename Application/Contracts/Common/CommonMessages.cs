namespace GenricRepository.Application.Contracts.Common;

public static class CommonMessages
{
    public const string CreatedTemplate = "{0} added successfully.";
    public const string UpdatedTemplate = "{0} updated successfully.";
    public const string DeletedTemplate = "{0} deleted successfully.";
    public const string FetchedTemplate = "{0} fetched successfully.";
    public const string ListFetchedTemplate = "{0} fetched successfully.";
    public const string NotFoundTemplate = "{0} not found.";

    public static string Created(string entity) => string.Format(CreatedTemplate, entity);
    public static string Updated(string entity) => string.Format(UpdatedTemplate, entity);
    public static string Deleted(string entity) => string.Format(DeletedTemplate, entity);
    public static string Fetched(string entity) => string.Format(FetchedTemplate, entity);
    public static string ListFetched(string entityPlural) => string.Format(ListFetchedTemplate, entityPlural);
    public static string NotFound(string entity) => string.Format(NotFoundTemplate, entity);
}
