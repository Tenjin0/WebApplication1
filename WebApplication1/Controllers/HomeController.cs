﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LanguageFeatures.Models;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {

            List<string> results = new List<string>();

            Dictionary<string, Product> products = new Dictionary<string, Product> {
                //{ "Kayak", new Product { Name = "Kayak", Price = 275M } },
                //{ "Lifejacket", new Product { Name = "LifeJacket", Price = 48.95M } }
                ["Kayak"] =  new Product { Name = "Kayak", Price = 275M },
                ["Lifejacket"] = new Product { Name = "LifeJacket", Price = 48.95M }
            };

            foreach (Product p in Product.GetProducts()) {
                string name = p?.Name ?? "No Name";
                decimal? price = p?.Price ?? 0;
                string relatedName = p?.Related?.Name ?? "<None>";

                results.Add($"{nameof(p.Name)}: {name}, {nameof(p.Price)}: {price}, Related: {relatedName}");
                //results.Add(string.Format("Name: {0}, Price: {1}, Related: {2}", name, price, relatedName));
            }

            return View(products.Keys);
        }

        public IActionResult Index2() {

            object[] data = new object[] {
            275M, 29.95M,"apple","orange", 100, 10
            };
            decimal total = 0;
            for (int i = 0; i < data.Length; i++) {

                switch (data[i]) {
                    case decimal decimalValue:
                        total += decimalValue;
                        break;
                    case int intValue when intValue > 50:
                        total += intValue;
                        break;
                }
            }
            return View("Index2", new String[] { $"Total: {total:C2}" });
        }

        public IActionResult Index3() {

            ShoppingCart cart = new ShoppingCart { Products = Product.GetProducts() };

            Product[] productArray = {
                new Product { Name = "Kayak", Price = 275M },
                new Product { Name = "LifeJacket", Price = 48.95M },
                new Product { Name = "Soccer Ball", Price = 19.50M },
                new Product { Name = "Corner flag", Price = 34.95M }
            };

            decimal cartTotal = cart.TotalPrices();
            decimal priceFilterTotal = productArray.FilterByPrice(20).TotalPrices();
            decimal nameFilterTotal = productArray.FilterByName('S').TotalPrices();

            return View("Index2", new String[] {
                $"Cart Total: { cartTotal:C2}",
                $"Price Total: { priceFilterTotal:C2}",
                $"Name Total: { priceFilterTotal:C2}",
            });
        }

        bool FilterByPrice(Product p) {
            return (p?.Price ?? 0) >= 20;
        }

        public bool EvaluateProduct(Product p, int count) {
            return (p?.Price ?? 0) >= count;
        }

        public IActionResult Index4() {

            ShoppingCart cart = new ShoppingCart { Products = Product.GetProducts() };

            Product[] productArray = {
                new Product { Name = "Kayak", Price = 275M },
                new Product { Name = "LifeJacket", Price = 48.95M },
                new Product { Name = "Soccer Ball", Price = 19.50M },
                new Product { Name = "Corner flag", Price = 34.95M }
            };


            Func<Product, bool> FilterByName = delegate (Product prod) {
                return prod?.Name?[0] == 'S';
            };
            
            
            decimal cartTotal = cart.TotalPrices();
            //decimal priceFilterTotal = productArray.Filter(p => (p?.Price ?? 0) >= 20).TotalPrices();
            decimal priceFilterTotal = productArray.Filter(p => EvaluateProduct(p, 20)).TotalPrices();
            //decimal nameFilterTotal = productArray.Filter(p => p?.Name?[0] == 'S').TotalPrices();
            decimal nameFilterTotal = productArray.Filter(p => p?.Name?[0] == 'S').TotalPrices();

            return View("Index2", new String[] {
                $"Cart Total: { cartTotal:C2}",
                $"Price Total: { priceFilterTotal:C2}",
                $"Name Total: { priceFilterTotal:C2}",
            });
        }

        public async Task<ViewResult> Index5() {
            long? length = await MyAsyncMethods.GetPageLenghtAwait();
            return View(new string[] { $"Length: {length}"});
        }
    }
}