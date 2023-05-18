
// https://www.youtube.com/watch?v=rImaNyfKhZk

// dotnet lambda list-functions
// dotnet lambda deploy-function

/*
invoke-function - invokes a Lambda function with a specified payload
create-function - creates a new Lambda function based on a deployment package
delete-function - deletes a Lambda function
get-function-configuration - retrieves the configuration settings for a specified Lambda function
update-function-configuration - updates the configuration settings for a specified Lambda function
 */

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);    // default is RestApi

var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();
