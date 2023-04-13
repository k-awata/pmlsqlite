# PMLSQLite

PMLSQLite provides ORM between AVEVA PML2 language objects and SQLite3 databases.

## Installation

1. Download the zip archive from [Releases](https://github.com/k-awata/pmlsqlite/releases).

2. Extract files and folders from the zip archive and place them in a directory defined by the `PMLLIB` environment variable.

3. Open an AVEVA product.

4. Enter the following command in the Command Window:

   ```pml2
   pml rehash all
   ```

## Usage

### Open Database

* Definitions

  ```pml2
  .PMLSQLITEORM()
  .PMLSQLITEORM(!filename is STRING)
  ```

* Arguments

  * `!filename` - Path of a SQLite3 database file

* Example

  ```sql
  using namespace 'PMLSQLite'

  -- Open an in-memory database
  !memdb = object PMLSQLITEORM()

  -- Open a database file
  !filedb = object PMLSQLITEORM('%TEMP%\example.db')
  ```

### Create Table

* Definitions

  ```pml2
  .CreateTable(!table is STRING, !schema is ANY, !force is BOOLEAN)
  ```

* Arguments

  * `!table` - Table name
  * `!schema` - Object or JSON string for the schema to define the table
  * `!force` - If true, delete the existing table before creating the new table.

* Example

  ```sql
  using namespace 'PMLSQLite'
  !db = object PMLSQLITEORM('%TEMP%\example.db')

  -- Create a table from a DIRECTION object
  $* CREATE TABLE IF NOT EXISTS "directions" (
  $*   "east" REAL,
  $*   "north" REAL,
  $*   "origin" TEXT,
  $*   "up" REAL
  $* );
  !db.CreateTable('directions', object DIRECTION(), false)

  -- Force-create a table from a JSON string
  $* DROP TABLE IF EXISTS "json";
  $* CREATE TABLE IF NOT EXISTS "json" (
  $*   "id" INTEGER PRIMARY KEY,
  $*   "name" TEXT,
  $*   "length" REAL,
  $*   "flag" INTEGER
  $* );
  !db.CreateTable('json', '{"id": 0, "name": "", "length": 0, "flag": false}', true)
  ```

### Drop Table

* Definitions

  ```pml2
  .DropTable(!table is STRING)
  ```

* Arguments

  * `!table` - Table name

* Example

  ```sql
  using namespace 'PMLSQLite'
  !db = object PMLSQLITEORM('%TEMP%\example.db')
  !db.CreateTable('directions', object DIRECTION(), true)

  -- Drop a table
  $* DROP TABLE IF EXISTS "directions";
  !db.DropTable('directions')
  ```

### Insert

* Definitions

  ```pml2
  .Insert(!table is STRING, !record is ANY)
  .Insert(!table is STRING, !records is ARRAY)
  ```

* Arguments

  * `!table` - Table name
  * `!record` - Object or JSON string for the values to insert a new record
  * `!records` - Array of objects or JSON strings for the values to bulk-insert new records

* Example

  ```sql
  using namespace 'PMLSQLite'
  !db = object PMLSQLITEORM('%TEMP%\example.db')
  !db.CreateTable('directions', object DIRECTION(), true)

  -- Insert a new record
  $* INSERT INTO "directions" ("east","north","origin","up") VALUES
  $*   (1, 0, '=?/?', 0)
  !db.Insert('directions', E)

  -- Bulk-insert new records
  !dirs[1] = E
  !dirs[2] = N
  !dirs[3] = U
  $* INSERT INTO "directions" ("east","north","origin","up") VALUES
  $*   (1, 0, '=?/?', 0),
  $*   (0, 1, '=?/?', 0),
  $*   (0, 0, '=?/?', 1)
  !db.Insert('directions', !dirs)
  ```

### Update

* Definitions

  ```pml2
  .Update(!table is STRING, !set is ANY, !where is ANY)
  ```

* Arguments

  * `!table` - Table name
  * `!set` - Object or JSON string for the values to update records
  * `!where` - Condition for specifying records to update the values

* Example

  ```sql
  using namespace 'PMLSQLite'
  !db = object PMLSQLITEORM('%TEMP%\example.db')
  !db.CreateTable('directions', object DIRECTION(), true)
  !db.Insert('directions', E)
  !db.Insert('directions', N)
  !db.Insert('directions', E)
  !db.Insert('directions', N)
  !db.Insert('directions', U)

  -- Update records
  $* UPDATE "directions" SET
  $*   "east" = 0,
  $*   "north" = 0,
  $*   "origin" = '=?/?',
  $*   "up" = -1
  $* WHERE
  $*   "east" = 1 AND
  $*   "north" = 0 AND
  $*   "up" = 0
  !db.Update('directions', D, '{"east": 1, "north": 0, "up": 0}')
  ```

### Delete

* Definitions

  ```pml2
  .Delete(!table is STRING, !where is ANY)
  ```

* Arguments

  * `!table` - Table name
  * `!where` - Condition for specifying records to delete

* Example

  ```sql
  using namespace 'PMLSQLite'
  !db = object PMLSQLITEORM('%TEMP%\example.db')
  !db.CreateTable('directions', object DIRECTION(), true)
  !db.Insert('directions', E)
  !db.Insert('directions', N)
  !db.Insert('directions', E)
  !db.Insert('directions', N)
  !db.Insert('directions', U)

  -- Delete records
  $* DELETE FROM "directions"
  $* WHERE
  $*   "east" = 1 AND
  $*   "north" = 0 AND
  $*   "up" = 0
  !db.Delete('directions', '{"east": 1, "north": 0, "up": 0}')
  ```

### Select

* Definitions

  ```pml2
  .Select(!table is STRING, !objtype is STRING) is ARRAY
  .Select(!table is STRING, !objtype is STRING, !where is ANY) is ARRAY
  .Select(!table is STRING, !columns is ARRAY) is ARRAY
  .Select(!table is STRING, !columns is ARRAY, !where is ANY) is ARRAY
  ```

* Arguments

  * `!table` - Table name
  * `!objtype` - Object type of the retrieved records
  * `!columns` - Columns in the retrieved records
  * `!where` - Condition for specifying records to extract

* Example

  ```sql
  using namespace 'PMLSQLite'
  !db = object PMLSQLITEORM('%TEMP%\example.db')
  !db.CreateTable('directions', '{"id": 0, "east": 0, "north": 0, "up": 0, "origin": ""}', true)
  !db.Insert('directions', E)
  !db.Insert('directions', N)
  !db.Insert('directions', E)
  !db.Insert('directions', N)
  !db.Insert('directions', U)

  -- Select records and read them as a DIRECTION object
  $* SELECT * FROM "directions"
  !result = !db.Select('directions', 'DIRECTION')
  do !r values !result
    q var !r
  enddo

  -- Select records with the specified columns and read them as an ARRAY object
  $* SELECT id, east, north, up FROM "directions"
  $* WHERE
  $*   "east" = 1 AND
  $*   "north" = 0 AND
  $*   "up" = 0
  !result = !db.Select('directions', Split('id east north up'), '{"east": 1, "north": 0, "up": 0}')
  do !r values !result
    q var !r
  enddo

  -- Get the number of records
  $* SELECT count(*) FROM "directions"
  q var !db.Select('directions', Split('count(*)')).First()
  ```

### Transactions

* Definitions

  ```pml2
  .BeginTransaction()
  .Commit()
  .Rollback()
  ```

* Example

  ```sql
  using namespace 'PMLSQLite'
  !db = object PMLSQLITEORM('%TEMP%\example.db')
  !db.CreateTable('directions', object DIRECTION(), true)

  -- Begin transaction
  !db.BeginTransaction()
  !db.Insert('directions', E)
  !db.Insert('directions', N)
  !db.Insert('directions', U)
  !db.Commit()
  ```

### Execute Raw SQL

* Definitions

  ```pml2
  .Execute(!sql is STRING)
  .Execute(!sql is STRING, !param is ARRAY)
  .Execute(!sql is ARRAY)
  .Execute(!sql is ARRAY, !param is ARRAY)
  .Query(!sql is STRING, !objtype is STRING) is ARRAY
  .Query(!sql is STRING, !objtype is STRING, !param is ARRAY) is ARRAY
  .Query(!sql is ARRAY, !objtype is STRING) is ARRAY
  .Query(!sql is ARRAY, !objtype is STRING, !param is ARRAY) is ARRAY
  ```

* Arguments

  * `!sql` - SQL commands
  * `!objtype` - Object type of the retrieved records
  * `!param` - Parameters to bind on the placeholders

* Example

  ```sql
  using namespace 'PMLSQLite'
  !db = object PMLSQLITEORM('%TEMP%\example.db')
  !db.CreateTable('directions', object DIRECTION(), true)

  -- Execute raw SQL
  !db.Execute('INSERT INTO directions (east, north, up) VALUES (1, 0, 0), (0, 1, 0), (0, 0, 1)')
  !result = !db.Query('SELECT * FROM directions WHERE east = ? AND north = ? AND up = ?', 'DIRECTION', Split('0 1 0'))
  do !r values !result
    q var !r
  enddo
  ```

## Tests

The test cases use [PML Unit](https://github.com/PoByBolek/PmlUnit) on Everything3D 2.1.

## License

[MIT License](LICENSE)
