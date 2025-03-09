using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebMVC.Infrastructure;
using WebMVC.Models.OrderModels;

namespace WebMVC.Services
{
    public class OrderService: IOrderService
    {
        private readonly IHttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly ILogger _logger;
        private readonly ITokenProvider _tokenProvider;

        public OrderService(IConfiguration config,
            IHttpContextAccessor httpContextAccesor,
            IHttpClient httpClient, ILoggerFactory logger,
            ITokenProvider tokenProvider)
        {
            _remoteServiceBaseUrl = $"{config["OrderUrl"]}/api/orders";
            _config = config;
            _httpContextAccesor = httpContextAccesor;
            _apiClient = httpClient;
            _logger = logger.CreateLogger<OrderService>();
            _tokenProvider = tokenProvider;
        }

        private string GetUserToken()
        {
            return _tokenProvider.GetToken();
        }
        public async Task<int> CreateOrder(Order order)
        {
            var token = GetUserToken();
            var addNewOrderUri = APIPaths.Order.AddNewOrder(_remoteServiceBaseUrl);
            _logger.LogDebug(" OrderUri " + addNewOrderUri);

            var response = await _apiClient.PostAsync(addNewOrderUri, order, token);
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error creating order, try later.");
            }

            var jsonString = response.Content.ReadAsStringAsync();
            jsonString.Wait();
            dynamic data = JObject.Parse(jsonString.Result);
            string value = data.orderId;
            return Convert.ToInt32(value);
        }

        public async Task<Order> GetOrder(string orderId)
        {
            var token = GetUserToken();
            var getOrderUri = APIPaths.Order.GetOrder(_remoteServiceBaseUrl, orderId);

            var dataString = await _apiClient.GetStringAsync(getOrderUri, token);
            var response = JsonConvert.DeserializeObject<Order>(dataString);
            return response;
        }


    }
}
