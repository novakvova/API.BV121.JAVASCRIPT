﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Data.Entities;
using WebShop.Models;

namespace WebShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppEFContext _context;
        public ProductsController(AppEFContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var list = _context.Products
                .Include(x=>x.Category)
                .Include(x=>x.Images)
                .Select(x => new ProductItemView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Category=x.Category.Name,
                    Images=x.Images.Select(x=>x.Name).ToList()
                })
                .ToList();
            return Ok(list);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] ProductSearchViewModel search)
        {
            var query = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Images)
                .AsQueryable();

            if(!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(search.Description))
            {
                query = query.Where(x => x.Description.ToLower().Contains(search.Description.ToLower()));
            }
            if (!string.IsNullOrEmpty(search.CategoryId))
            {
                int catid=int.Parse(search.CategoryId);
                query = query.Where(x => x.CategoryId==catid);
            }

            int page = search.Page;
            int pageSize = 5;

            var list = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductItemView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Category = x.Category.Name,
                    Images = x.Images.Select(x => x.Name).ToList()
                })
                .ToList();

            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)pageSize);

            return Ok(new ProductSearchResultViewModel
            {
                Products = list,
                Total = total,
                CurrentPage = page,
                Pages = pages

            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateView model)
        {
            var entity = new ProductEntity
            {
                Name = model.Name,
                Price=model.Price,
                CategoryId = model.CategoryId,
                Description = model.Description
            };
            _context.Products.Add(entity);
            _context.SaveChanges();

            string imageName = String.Empty;
            if(model.Files!=null)
            {
                short priority = 1;
                foreach(var image in model.Files)
                {
                    if (image != null)
                    {
                        imageName = await SaveImage(image);
                        ProductImageEntity pi = new ProductImageEntity
                        {
                            Name = imageName,
                            Priority = priority,
                            ProductId = entity.Id
                        };
                        _context.ProductImages.Add(pi);
                        _context.SaveChanges();
                    }
                }
            }
            return Ok();
        }

        private async Task<string> SaveImage(IFormFile image)
        {

            string exp = Path.GetExtension(image.FileName);
            var imageName = Path.GetRandomFileName() + exp;
            string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
            using (var stream = System.IO.File.Create(dirSaveImage))
            {
                await image.CopyToAsync(stream);
            }
            return imageName;
        }
    }
}
