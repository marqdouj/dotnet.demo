var builder = DistributedApplication.CreateBuilder(args);

var maildev = builder.AddMailDev("maildev");

var apiService = builder.AddProject<Projects.AspireDemo_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(maildev);

builder.AddProject<Projects.AspireDemo_WebApp>("demoapp")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
