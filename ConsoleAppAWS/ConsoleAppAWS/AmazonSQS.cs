
using Amazon.SQS;
using Amazon.SQS.Model;

namespace ConsoleAppAWS
{
    public class AwsSqsClient : IDisposable
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;

        public AwsSqsClient(string queueUrl)
        {
            _queueUrl = queueUrl;
            _sqsClient = new AmazonSQSClient();
        }

        public async Task SendMessageAsync(string messageBody)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = messageBody
            };

            var response = await _sqsClient.SendMessageAsync(sendMessageRequest);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Error sending message with body: {messageBody}");
            }
        }

        public async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
        {
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                WaitTimeSeconds = 20,
                MaxNumberOfMessages = 10
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                var response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest, cancellationToken);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"Error receiving messages from queue");
                }

                foreach (var message in response.Messages)
                {
                    Notify(message.Body);
                }
            }
        }

        // Observer Pattern implementation
        List<IObserver<string>> observers = new List<IObserver<string>>();

        public void Subscribe(IObserver<string> observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver<string> observer)
        {
            observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(message);
            }
        }

        public void Dispose()
        {
            _sqsClient.Dispose();
        }
    }

    public class SqsMessageObserver : IObserver<string>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Observer completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(string message)
        {
        }
    }
}