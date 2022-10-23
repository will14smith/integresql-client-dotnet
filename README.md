# Toxon.Integresql.Client

A .NET client for [IntegreSQL](https://github.com/allaboutapps/integresql), a management tool for isolated PostgreSQL databases for integration testing

## Usage

- Install the [`Toxon.Integresql.Client`](https://www.nuget.org/packages/Toxon.Integresql.Client) nuget package
- Create a client for communicating with the service
  ```csharp
     var client = new IntegresqlClient("http://localhost:5000/api");
  ```
- Create a hash of the schema & fixtures, this could vary a lot across applications so imagine something like feeding your schema setup files into a SHA256 hash
  ```csharp
    var hashBytes = SHA256.HashData(File.ReadAllBytes("schema.sql"));
    var sb = new StringBuilder();
    for (int i = 0; i < hashBytes.Length; i++) { sb.Append(hashBytes[i].ToString("x2")); }
    var hash = sb.ToString();
  ```
- Setup the template database, you can do this with `client.InitializeTemplate` and `client.FinalizeTemplate`, or use the setup helper method. Either way you want to try and only do this once per test session, so consider putting it in a `Lazy` initialized static field
  ```csharp
    await client.SetupTemplate(hash, connectionString => {
        // connect to the database and set it up 
    });
  ```
- In each test get a connection to a unique isolated database using `client.GetDatabase` and then return it at the end with `client.ReturnDatabase`, alternatively use the disposable extension method
  ```csharp
    await using var database = client.AcquireDatabase(hash);
    var connectionString = database.GetConnectionString();
    // run your test, the database will be automatically returned and recycled at the end
  ```