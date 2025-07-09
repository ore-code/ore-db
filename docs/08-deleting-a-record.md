# Deleting a Record

Use the `Delete` method to remove a JSON record from the database by specifying its unique `guid`. All records matching the given `guid` will be deleted from memory.

**Syntax**

	public void Delete(string id)

## Example

	using (var db = new Database("database.json"))
	{
		db.Delete("f4411f8f1e824610a98f30d852783d2b");
		db.Commit();
	}

## Notes

This method deletes all records with the matching guid. Changes are made in memory and require calling Commit to persist them to disk.