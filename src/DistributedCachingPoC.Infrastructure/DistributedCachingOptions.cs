namespace DistributedCachingPoC.Infrastructure;

internal sealed class DistributedCachingOptions
{
    public static readonly string SectionName = "DistributedCaching";
    public string SchemaName { get; set; } = "dbo";
    public string TableName { get; set; } = "CachingTable";
    public string CacheKey { get; set; } = "temperatures";
}
