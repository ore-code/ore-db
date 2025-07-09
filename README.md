# OreDB

OreDB is a lightweight, no-SQL JSON database engine for .NET. It uses in-memory storage with thread-safe access and simple commit-based persistence, making it ideal for small tools, prototypes, and applications that need structured data without a full database engine.

## Example

	using (var db = new Database("database.json"))
	{
		var results = db.Select(record => record["department"]?.ToString() == "HR");

		foreach (var employee in results)
		{
			Console.WriteLine(employee["firstname"]);
		}
	}

## Documentation

- [Overview](/docs/01-overview.md)
- [Selecting Multiple Records](/docs/02-selecting-multiple.md)
- [Selecting a Single Record](/docs/03-selecting-single.md)
- [Finding a Single Record By ID](/docs/04-finding-record-by-id.md)
- [Checking if a Record Exists](/docs/05-checking-if-exists.md)
- [Inserting a Record](/docs/06-inserting-a-record.md)
- [Updating a Record](/docs/07-updating-a-record.md)
- [Deleting a Record](/docs/08-deleting-a-record.md)

## Contributing

We welcome contributions from the community! To get started, please review our [Contributing Guidelines](https://github.com/ore-code/ore-db/blob/main/CONTRIBUTING.md)


