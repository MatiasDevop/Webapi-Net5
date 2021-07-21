using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class CartController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult GetCart()
        {
            var cart = new List<Item>();
            var total = cart.Sum(item => item.Price * item.Quantity);
            return Ok(total);
        }

        // public IActionResult Buy(string id){
        //      List<Item> cart = new List<Item>();
        //     if (id != null)
        //     {
        //         cart.Add(new Item{ Id = new Guid(id) , Quantity = 1});
        //     }

        //     return Ok(cart);
        // }
    }
}