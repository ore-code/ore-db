# Selecting Multiple Records

Use the `Select` method to retrieve a list of JSON records from the in-memory database that match a given condition. The method accepts a predicate (a function that returns `true` or `false`) and returns a filtered list of matching `JObject` entries.

**Syntax**

	public List<JObject> Select(Func<JObject, bool> predicate)

## Example

	using (var db = new Database("database.json"))
	{
		var results = db.Select(record => record["department"]?.ToString() == "HR");

		foreach (var employee in results)
		{
			Console.WriteLine(employee["firstname"]);
		}
	}
 


