
var builder = DistributedApplication.CreateBuilder(args);
  

var sql = builder.AddSqlServer("sql")
                 .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("AspireDb")
            .WithCreationScript(File.ReadAllText("init-db.sql"));

var apiService = builder.AddProject<Projects.AspireApp1_ApiService>("apiservice")
    .WithReference(db)
    .WaitFor(db)
    .WithHttpHealthCheck("/health");

builder.Build().Run();
