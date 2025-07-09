# Inserting a Record

Use the `Insert` method to add a new JSON record to the in-memory database. If a `guid` field is not provided, the method will automatically generate one to uniquely identify the entry.

**Syntax**
 
	public void Insert(JObject obj)

## Example

	using (var db = new Database("database.json"))
	{
		var record = new JObject
		{
			["firstname"]  = "Jane",
			["lastname"]   = "Smith",
			["department"] = "Marketing"
		};

		db.Insert(record);
		db.Commit();
	}

## Notes

If the input object does not contain a guid field or it is empty, the method will generate a new GUID and assign it automatically. The inserted record is stored in memory until explicitly persisted using the Commit method.
