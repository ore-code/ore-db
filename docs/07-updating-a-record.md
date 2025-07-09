# Updating a Record

Use the `Update` method to replace an existing JSON record in the database by specifying its unique `guid`. The method updates the entire record with the provided data, ensuring the `guid` remains unchanged.

**Syntax**
 
	public bool Update(string id, JObject obj)

## Example

	using (var db = new Database("database.json"))
	{
		var updatedData = new JObject
		{
			["firstname"]  = "John",
			["lastname"]   = "Doe",
			["department"] = "Information Technology"
		};

		bool success = db.Update("f4411f8f1e824610a98f30d852783d2b", updatedData);

		if (success)
		{
			db.Commit();
		}
	}

## Notes

The method returns true if a record with the specified guid was found and updated; otherwise, it returns false. The provided JObject replaces the existing record except that its guid is forcibly set to the supplied id. Remember to call Commit to persist changes to disk.