# Randal's Video Store - Example API

In this project we want to provide an API for accessing the inventory of an early 90's video store. The frontend will be designed by another party; we are only to provide the API itself.

## Usage scenarios/use cases

- Get a list of all the movies filtered by name. All properties should be returned.
- Get a single movie. All properties should be returned.
- Delete a single movie by Id.
- Create a new movie. All properties are mandatory.

## Usage

1. clone/download the code
2. open the subfolder `RandalsVideoStore.Api` in VS Code. It should ask to add resources; press `yes`.
3. open a terminal
4. `dotnet ef database update` (this will create a .sqlite file)
5. `dotnet watch run`
6. <https://localhost:5000/swagger/index.html>.
7. Behold!

## Notes

- We use a sqlite database in this project; needless to say this is not production ready. In real use case scenarios you want a decent relational database (PostgreSQL, SqlServer, ...). Using sqlite is nice for debugging purposes; if you want to reset the database you just throw away the .sqlite file and re-run the migration.
- Notice that this project tries to follow the SOLID principles. This is always best practice.
- Normally when you generate methods that are async it gets suffixed with "Async". According to me this is pretty silly; this codebase assumes async is the default and that whenever we got a database interaction that we need to be sync we add the suffix "Sync". I don't care either way, just be consistent.