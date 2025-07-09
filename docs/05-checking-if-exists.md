# Checking if a Record Exists

Use the `Exists` method to determine whether a record with a specific `guid` exists in the database. This is useful when you need to validate presence without retrieving the full record.

**Syntax**

	public bool Exists(string id)

## Example

	using (var db = new Database("database.json"))
	{
		bool exists = db.Exists("f4411f8f1e824610a98f30d852783d2b");

		if (exists)
		{
			Console.WriteLine("Record found.");
		}
		else
		{
			Console.WriteLine("Record not found.");
		}
	}
