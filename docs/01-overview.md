# Overview

OreDB is a lightweight, in-memory no-SQL database for .NET that stores structured data as JSON using `JObject` from Newtonsoft.Json. It’s designed for simplicity, portability, and performance in small applications, tools, or prototypes where a full database engine would be unnecessary.

**In-memory storage**  
Data is loaded into memory on startup and kept there for fast access. All operations (insert, update, delete, query) work against in-memory data until explicitly saved.

**Thread-safe access**  
Uses `ReaderWriterLockSlim` to ensure safe concurrent reads and writes, making it suitable for multi-threaded environments.

**Flexible JSON structure**  
Records are stored as `JObject` instances, allowing for dynamic, schema-less data that adapts to your needs.

**Predicate-based querying**  
Supports LINQ-style filtering with methods like `Select` and `SelectOne`, giving you full control over how data is queried.

**Automatic GUID assignment**  
If a record is inserted without a `guid`, one is automatically generated to uniquely iden
