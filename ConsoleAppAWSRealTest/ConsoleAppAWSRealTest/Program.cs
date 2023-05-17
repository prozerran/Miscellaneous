// See https://aka.ms/new-console-template for more information


using ConsoleAppAWSRealTest;

var lambda = new AmazonLambda();
await lambda.TestMyLambda().ConfigureAwait(false);

Console.WriteLine("Hello, World!");
