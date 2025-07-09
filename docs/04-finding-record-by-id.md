# Finding a Single Record By ID

Use the `FindByID` method to locate and retrieve a JSON record from the database by its unique `guid`. This method returns the first matching record or `null` if none is found.

**Syntax**

	public JObject? FindByID(string id)

## Example
	
	using (var db = new Database("database.json"))
	{
		var employee = db.FindByID("f4411f8f1e824610a98f30d852783d2b");

		if (employee != null)
		{
			Console.WriteLine(employee["firstname"]);
		}
	}

## Notes
 
If no record matches the specified `guid`, the method returns `null`. Use the `SelectOne` method for more flexibility.
