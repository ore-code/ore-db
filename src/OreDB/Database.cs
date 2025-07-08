using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;
using System.Xml;

namespace OreDB
{
    public class Database : IDisposable
    {
        #region Declarations

        /**
         * Define a variable that will hold the file name for commits.
         * 
         * @type {string}
         */
        private string _filename = "";

        /**
         * Define a variable that will hold all the records in memory.
         * 
         * @type {List<JObject>}
         */
        private List<JObject> _records = new();

        /**
         * Define a variable that will be used for file locks.
         * 
         * @type {ReaderWriterLockSlim}
         */
        private ReaderWriterLockSlim _lock = new();

        /**
         * Define a variable that hold our disposed state.
         * 
         * @type {bool}
         */
        private bool _disposed = false;

        #endregion

        #region Public Members

        /**
         * The constructor that reads a no-SQL database into memory to be manipulated.
         * 
         * @param {string} filename
         *
         * @example
         * 
         * using (var _db = new Database("database.json"))
         * {
         *     var _data = new JObject
         *     {
         *         ["guid"]       = "f4411f8f1e824610a98f30d852783d2b",
         *         ["firstname"]  = "John",
         *         ["lastname"]   = "Doe",
         *         ["department"] = "Information Technology" 
         *     };
         * 
         *     _db.Insert(_data);
         * }
         */
        public Database(string filename)
        {
            _filename = filename;

            //
            // Lock the list to ensure data integrity when manipulating it.
            //
            _lock.EnterWriteLock();

            //
            // Deserialize the contents of the file into a JObject list.
            //
            try
            {
                var _json = JsonConvert.DeserializeObject<List<JObject>>(File.ReadAllText(_filename));

                if (_json != null)
                {
                    _records = _json;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /**
         * Return a snapshot of the current objects in memory.
         * 
         * @returns {<List<JObject>}
         * 
         * @example
         * 
         * using (var _db = new Database("database.json"))
         * {
         *     var _results = _db.Records.First();
         * }
         */
        public List<JObject> Records
        {
            get
            {
                //
                // Lock the list to ensure data integrity when reading it.
                //
                _lock.EnterReadLock();

                //
                // Return a snapshot of the JObject list.
                //
                try
                {
                    return _records.ToList();
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        /**
         * Queries the JObject list and returns a snapshot of the results.
         * 
         * @param {Func<JObject, bool>} predicate;
         * 
         * @example
         * 
         * using (var _db = new Database("database.json"))
         * {
         *     var _results = _db.Select(item => item["department"]?.toString() == "HR");
         * }
         */
        public List<JObject> Select(Func<JObject, bool> predicate)
        {
            //
            // Lock the list to ensure data integrity when reading it.
            //
            _lock.EnterReadLock();

            //
            // Return a snapshot of the JObject list.
            //
            try
            {
                return _records.Where(predicate).ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /**
         * Queries the JObject list and returns a snapshot of the results.
         * 
         * @param {Func<JObject, bool>} predicate;
         * 
         * @example
         * 
         * using (var _db = new Database("database.json"))
         * {
         *     var _result = _db.SelectOne(item => item["department"]?.toString() == "HR");
         * }
         */
        public JObject? SelectOne(Func<JObject, bool> predicate)
        {
            //
            // Lock the list to ensure data integrity when reading it.
            //
            _lock.EnterReadLock();

            //
            // Return a snapshot of the JObject list.
            //
            try
            {
                return _records.FirstOrDefault(predicate);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /**
         * Insert a new object in the no-SQL database. It will generate a guid if a unique ID isn't passed.
         * 
         * @param {JObject} obj
         * 
         * @example
         * 
         * using (var _db = new Database("database.json"))
         * {
         *     var _data = new JObject
         *     {
         *         ["guid"]       = "f4411f8f1e824610a98f30d852783d2b",
         *         ["firstname"]  = "John",
         *         ["lastname"]   = "Doe",
         *         ["department"] = "Information Technology" 
         *     };
         * 
         *     _db.Insert(_data);
         * }
         */
        public void Insert(JObject obj)
        {
            //
            // Lock the list to ensure data integrity when manipulating it.
            //
            _lock.EnterWriteLock();

            //
            // Insert a new object into the JObject list.
            //
            try
            {
                if (obj["guid"] == null || String.IsNullOrWhiteSpace(obj["guid"]?.ToString()) == true)
                {
                    obj["guid"] = Guid.NewGuid().ToString();

                }

                _records.Add(obj);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /**
         * Update an existing object in the no-SQL database by passing a guid or unique id.
         * 
         * @example
         * 
         * using (var _db = new Database("database.json"))
         * {
         *     var _data = new JObject
         *     {
         *         ["firstname"]  = "John",
         *         ["lastname"]   = "Doe",
         *         ["department"] = "Information Technology" 
         *     };
         * 
         *     _db.Update("f4411f8f1e824610a98f30d852783d2b", _data);
         * }
         */
        public bool Update(string id, JObject obj)
        {
            //
            // Lock the list to ensure data integrity when manipulating it.
            //
            _lock.EnterWriteLock();

            //
            // Update an existing object in the JObject list.
            //
            try
            {
                int _index = _records.FindIndex(item => item["guid"]?.ToString() == id);

                if (_index != -1)
                {
                    //
                    // Ensure the guid is the value that was passed over.
                    //
                    obj["guid"] = id;

                    //
                    // Update the record.
                    //
                    _records[_index] = obj;
                }

                return _index != -1;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /**
         * Delete an existing object in the no-SQL database by passing a guid or unique id.
         * 
         * @example
         * 
         * using (var _db = new Database("database.json"))
         * {
         *     _db.Delete("f4411f8f1e824610a98f30d852783d2b");
         * }
         */
        public void Delete(string id)
        {
            //
            // Lock the list to ensure data integrity when manipulating it.
            //
            _lock.EnterWriteLock();

            //
            // Delete an existing object from the JObject list.
            //
            try
            {
                _records.RemoveAll(item => item["guid"]?.ToString() == id);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /**
         * Find and return the object with the given guid or unique id.
         * 
         * @param {string} id
         * @returns {JObject}
         * 
         * @example
         * 
         * using (var _db = new Database("database.json"))
         * {
         *     var _data = _db.Find("f4411f8f1e824610a98f30d852783d2b");
         * }
         */
        public JObject? Find(string id)
        {
            //
            // Lock the list to ensure data integrity when reading it.
            //
            _lock.EnterReadLock();

            //
            // Find an object in the JObject list by its guid or unique id.
            //
            try
            {
                return _records.Find(item => item["guid"]?.ToString() == id);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /**
         * Checks if the object with the given guid or unique id exists.
         * 
         * @param {string} id
         * @returns {bool}
         * 
         * @example
         * 
         * using (var _db = new Database("database.json"))
         * {
         *     var exists = _db.Exists("f4411f8f1e824610a98f30d852783d2b");
         * }
         */
        public bool Exists(string id)
        {
            //
            // Lock the list to ensure data integrity when reading it.
            //
            _lock.EnterReadLock();

            //
            // Find an object in the JObject list by its guid or unique id.
            //
            try
            {
                return _records.Exists(item => item["guid"]?.ToString() == id);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /**
         * Commit the changes and save the file. This should be done after each transaction.
         * 
         * @example
         * 
         * using (var _db = new Database("database.json"))
         * {
         *     var _data = new JObject
         *     {
         *         ["guid"]       = "f4411f8f1e824610a98f30d852783d2b",
         *         ["firstname"]  = "John",
         *         ["lastname"]   = "Doe",
         *         ["department"] = "Information Technology" 
         *     };
         * 
         *     _db.Insert(_data);
         *     
         *     _db.Commit();
         * }
         */
        public void Commit()
        {
            //
            // Lock the list to ensure data integrity when manipulating it.
            //
            _lock.EnterWriteLock();

            //
            // Commit the changes to the physical file.
            //
            try
            {
                File.WriteAllText(_filename, JsonConvert.SerializeObject(_records, Newtonsoft.Json.Formatting.Indented));
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        #endregion

        #region Disposal

        /**
         * Finalizer fallback in our finalize/dispose pattern.
         */
        ~Database()
        {
            Dispose(false);
        }

        /**
         * Dispose of the lock to prevent memory leaks. 
         */
        public void Dispose()
        {
            Dispose(true);

            //
            // Prevent the finalizer from running again.
            //
            GC.SuppressFinalize(this);
        }

        /**
         * Dispose of the lock to prevent memory leaks.
         */
        public void Dispose(bool disposing)
        {
            //
            // Exit the method if the class was disposed.
            //
            if (_disposed == true)
            {
                return;
            }

            //
            // Dispose the lock to prevent memory leaks.
            //
            if (disposing == true)
            {
                _lock?.Dispose();
            }

            //
            // Set the disposed state.
            //
            _disposed = true;
        }

        #endregion
    }
}
