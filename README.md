# DistributedCaching
Distributed Caching with SQL Server in ASP.NET Core
[Microsoft.Extensions.Caching.SqlServer](https://www.nuget.org/packages/Microsoft.Extensions.Caching.SqlServer/7.0.0-preview.2.22153.2)

Before running, set DistCache_ConnectionString in the ConnectionStrings section of DistributedCachingPoC.WebUI -> appsettings.json - enter the connection details for your SQL Server.

During startup, a deploy of the dacpac file is performed, which creates a database named "DistributedCaching" with a CachingTable in which the cached data is stored

> "DistCache_ConnectionString": "Data Source=ServerName;Initial Catalog=DistributedCaching;User Id=UserName;Password=UserPassword"

Caching is one of the important and commonly used performance technique for low-latency responses from APIs. A cache is a high-speed memory that persists frequently accessed but less frequently changing data. Using a cache reduces processing time for APIs as the data is readily available for it to fetch and use.

Most basic use of a cache is in-memory caching, where data is stored within the application memory.
This is a simple approach, but comes with two important limitations:

Since data is persisted within the application memory, hence it is prone to loss when application restarts
Since data is local to an application Node, which means in a multi-node environment duplication might occur (different nodes caching same data)
Distributed Caching is a concept that overcomes these two limitations with its design. A Distributed Cache is a cache that is placed external to the application nodes, with the same properties as that of an in-memory cache - high speed memory with low-latency data reads and writes.

Since a Distributed Cache is external to application Nodes, it is independent of application Node crashes and data cached by one node can be accessed by other nodes. This removes any scope for data duplication and memory wastage on the same data.

Every Distributed Cache implementation requires a data store where the data can be persisted. There are several popular providers in the market; Redis, Memcached, NCache and so on. Even Cloud providers like AWS offers managed distributed caching solutions - ElastiCache for Memcached or Redis.

In this example, we will look at a Distributed Cache implementation with SQL Server as the backing store. We will see how we can easily configure a SQL Server data store and use the IDistributedCache implementation for caching operations.

SQL Server for Caching

dotnetcore provides a simple Distributed SQL Server Cache implementation to connect to an SQL Server database and use it as a backing store for the cache. Components use IDistributedCache interface to Get or Set data into the cache, while internally the data is written onto the SQL Server database.

All the cached content is stored in a table, and the caching library internally puts and fetches records from this table configured.

