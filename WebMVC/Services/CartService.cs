using Newtonsoft.Json;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.CartModels;

namespace WebMVC.Services
{
    public class CartService : ICartService
    {
        private readonly string _baseUrl;
        private readonly IConfiguration _config;
        private readonly IHttpClient _apiClient;
        private readonly ITokenProvider _tokenProvider;
        public CartService(IConfiguration config, IHttpClient client,
            ITokenProvider tokenProvider)
        {
            _config = config;
            _apiClient = client;
            _tokenProvider = tokenProvider;
            _baseUrl = $"{config["CartUrl"]}/api/cart";
        }
        private string GetUserToken()
        {
            return _tokenProvider.GetToken();
        }
        public async Task AddItemToCart(ApplicationUser user, CartItem product)
        {
            //Get the users cart
            var cart = await GetCart(user);
            //if the item exists in the cart. if yes, increase quantity. if no, add item
            var basketItem = cart.Items.Where(p => p.ProductId == product.ProductId).FirstOrDefault();
            if (basketItem == null)
            {
                cart.Items.Add(product);
            }
            else
            {
                basketItem.Quantity++;
            }
            await UpdateCart(cart);
        }

        public async Task ClearCart(ApplicationUser user)
        {
            var token = GetUserToken();
            var cleanBasketUri = APIPaths.Basket.CleanBasket(_baseUrl, user.Email);
            await _apiClient.DeleteAsync(cleanBasketUri, token);
        }

        public async Task<Cart> GetCart(ApplicationUser user)
        {
            var token = GetUserToken();
            var getBasketUri = APIPaths.Basket.GetBasket(_baseUrl, user.Email);
            var dataString = await _apiClient.GetStringAsync(getBasketUri, token);
            var response = JsonConvert.DeserializeObject<Cart>(dataString) ??
                new Cart
                {
                    BuyerId = user.Email
                };
            return response;
        }

        public async Task<Cart> SetQuantities(ApplicationUser user, 
            Dictionary<string, int> quantities)
        {
            //Get cart
            var basket = await GetCart(user);
            basket.Items.ForEach(x =>
            {
                if (quantities.TryGetValue(x.Id, out var quantity))
                {
                    x.Quantity = quantity;
                }
            });
            return basket;
        }

        public async Task<Cart> UpdateCart(Cart cart)
        {
            var token = GetUserToken();
            var updateBasketUri = APIPaths.Basket.UpdateBasket(_baseUrl);
            var response = await _apiClient.PostAsync(updateBasketUri, cart, token);

            response.EnsureSuccessStatusCode();

            return cart;
        }
    }
}
