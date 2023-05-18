using Microsoft.AspNetCore.Mvc;

namespace MinimalLambda.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(ILogger<CalculatorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Perform x + y
        /// https://xozx7yvmqc3tdsgeskiwtuyjxi0ltlbf.lambda-url.ap-southeast-1.on.aws/calculator/add/8/9
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Sum of x and y.</returns>
        [HttpGet("add/{x}/{y}")]
        public int Add(int x, int y)
        {
            _logger.LogInformation($"{x} plus {y} is {x + y}");
            return x + y;
        }

        /// <summary>
        /// Perform x - y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>x subtract y</returns>
        [HttpGet("subtract/{x}/{y}")]
        public int Subtract(int x, int y)
        {
            _logger.LogInformation($"{x} subtract {y} is {x - y}");
            return x - y;
        }

        /// <summary>
        /// Perform x * y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>x multiply y</returns>
        [HttpGet("multiply/{x}/{y}")]
        public int Multiply(int x, int y)
        {
            _logger.LogInformation($"{x} multiply {y} is {x * y}");
            return x * y;
        }

        /// <summary>
        /// Perform x / y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>x divide y</returns>
        [HttpGet("divide/{x}/{y}")]
        public int Divide(int x, int y)
        {
            _logger.LogInformation($"{x} divide {y} is {x / y}");
            return x / y;
        }

        /// <summary>
        /// Perform Factorial N
        /// https://xozx7yvmqc3tdsgeskiwtuyjxi0ltlbf.lambda-url.ap-southeast-1.on.aws/calculator/fact/8
        /// </summary>
        /// <param name="n"></param>
        [HttpGet("fact/{n}")]
        public int Factorial(int n)
        {
            if (n == 0)
            {
                return 1;
            }
            else
            {
                return n * Factorial(n - 1);
            }
        }
    }
}