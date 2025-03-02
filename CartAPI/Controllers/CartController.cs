using CartAPI.Data;
using CartAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CartAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;
        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            var basket = await _repository.GetCartAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] Cart basket)
        {
            var updatedCart = await _repository.UpdateCartAsync(basket);
            return Ok(updatedCart);
        }

        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _repository.DeleteCartAsync(id);
        }
    }
}
