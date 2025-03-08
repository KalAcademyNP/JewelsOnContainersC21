using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Services;

namespace WebMVC.ViewComponents
{
    public class CartList: ViewComponent
    {
        private readonly ICartService _cartSvc;
        public CartList(ICartService cartSvc) => _cartSvc = cartSvc;

        public async Task<IViewComponentResult> InvokeAsync(ApplicationUser user)
        {
            var vm = new Models.CartModels.Cart();
            try
            {
                vm = await _cartSvc.GetCart(user);
            }
            catch (BrokenCircuitException)
            {
                ViewBag.IsBasketInoperative = true;
                TempData["BasketInoperativeMsg"] = "Basket service is inoperative, please try later on.";
            }
            return View(vm);
        }

    }
}
