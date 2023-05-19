
using Amazon.EventBridge;
using Amazon.EventBridge.Model;

namespace ConsoleAppAWS
{
    public class EventBridgeService
    {
        private readonly IAmazonEventBridge _eventBridge;

        private readonly IAmazonEventBridge eventBridgeClient;
        private readonly string eventBusName;
        private readonly string source;
        private readonly string detailType;
        private readonly string detail;

        public EventBridgeService(IAmazonEventBridge eventBridge)
        {
            _eventBridge = eventBridge;
        }

        public async Task<PutEventsResponse> PutEventsAsync(PutEventsRequest request)
        {
            return await _eventBridge.PutEventsAsync(request);
        }

        public async Task<ListRulesResponse> ListRulesAsync(ListRulesRequest request)
        {
            return await _eventBridge.ListRulesAsync(request);
        }

        public async Task<PutRuleResponse> PutRuleAsync(PutRuleRequest request)
        {
            return await _eventBridge.PutRuleAsync(request);
        }

        public async Task<DeleteRuleResponse> DeleteRuleAsync(DeleteRuleRequest request)
        {
            return await _eventBridge.DeleteRuleAsync(request);
        }

        public async Task PublishEventAsync()
        {
            var request = new PutEventsRequest
            {
                Entries = new List<PutEventsRequestEntry>
                {
                    new PutEventsRequestEntry
                    {
                        Source = source,
                        DetailType = detailType,
                        Detail = detail,
                    }
                },
                //EventBusName = eventBusName
            };

            await eventBridgeClient.PutEventsAsync(request);
        }

        public async Task SubscribeToEventAsync()
        {
            var request = new PutTargetsRequest
            {
                //Rules = new List<PutTargetsRequestEntry>
                //{
                //    new PutTargetsRequestEntry
                //    {
                //        Arn = "ARN of the target lambda function to invoke",
                //        Id = "id-1",
                //        Input = "{ \"input\": \"some data\" }"
                //    }
                //},
                EventBusName = eventBusName
            };

            await eventBridgeClient.PutTargetsAsync(request);
        }
    }
}
