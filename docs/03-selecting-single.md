# Selecting a Single Record

Use the `SelectOne` method to retrieve the first JSON record from the in-memory database that matches a given condition. This is useful when you expect only one result or only need the first match.

**Syntax**
 
	public JObject? SelectOne(Func<JObject, bool> predicate)

## Example

	using (var db = new Database("database.json"))
	{
		var employee = db.SelectOne(record => record["department"]?.ToString() == "HR");

		if (employee != null)
		{
			Console.WriteLine(employee["firstname"]);
		}
	}
 
## Notes


