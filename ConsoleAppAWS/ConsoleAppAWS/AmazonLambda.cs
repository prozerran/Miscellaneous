
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace ConsoleAppAWS
{
    public class Function
    {
        public async Task<object> FunctionHandlerAsync(object input, ILambdaContext context)
        {
            // Do some async work here
            await Task.Delay(1000);

            return new
            {
                Message = "Hello from Amazon Lambda using async/await in C# .NET 7!",
                Input = input
            };
        }

        public async Task<DateTime> GetCurrentDateTimeAsync(ILambdaContext context)
        {
            return await Task.Run(() => DateTime.Now);
        }

        public async Task<int[]> QuickSortAsync(int[] inputArray, ILambdaContext context)
        {
            if (inputArray.Length <= 1)
            {
                return inputArray;
            }
            else
            {
                int pivot = inputArray[0];
                int[] leftArray = await Task.Run(() => Array.FindAll(inputArray, e => e < pivot));
                int[] rightArray = await Task.Run(() => Array.FindAll(inputArray, e => e >= pivot));
                leftArray = await QuickSortAsync(leftArray, context);
                rightArray = await QuickSortAsync(rightArray, context);
                int[] combinedArray = await Task.Run(() => leftArray.Concat(new[] { pivot }).Concat(rightArray).ToArray());
                return combinedArray;
            }
        }

        public async Task<string> CallExternalServiceAsync(ILambdaContext context)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://myexternalservice.com/api/data");
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        public async Task<List<int>> CalculatePrimesAsync(int n, ILambdaContext context)
        {
            var isPrime = new bool[n + 1];
            var primes = new List<int>();
            for (int i = 2; i <= n; i++)
            {
                isPrime[i] = true;
            }

            for (int i = 2; i <= n; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(i);
                    for (int j = i * 2; j <= n; j += i)
                    {
                        isPrime[j] = false;
                    }
                }
            }

            return primes;
        }

        public async Task<long> CalculateFactorialAsync(int n, ILambdaContext context)
        {
            if (n <= 1)
            {
                return 1;
            }
            else
            {
                return n * await CalculateFactorialAsync(n - 1, context);
            }
        }
    }
}
