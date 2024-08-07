﻿using Fiorello_MVC.Data;
using Fiorello_MVC.Models;
using Fiorello_MVC.Services.Interfaces;
using Fiorello_MVC.ViewModels.Products;
using Microsoft.EntityFrameworkCore;

namespace Fiorello_MVC.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(m=>m.Category).Include(m=>m.Price).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllPaginateAsync(int page, int take)
        {
            return await _context.Products.Include(m=>m.Category)
                                          .Include(m=>m.ProductImages)
                                          .Skip((page - 1)* take)
                                          .Take(take)
                                          .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllWithImagesAsync()
        {
            return await _context.Products.Include(m=>m.ProductImages).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int? id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> GetByIdWithAllDatasAsync(int? id)
		{
			return await _context.Products.Where(m=>m.Id == id)
                                          .Include(m=>m.ProductImages)
                                          .Include(m=>m.Category)
                                          .FirstOrDefaultAsync();
		}

        public async Task<int> GetCountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public IEnumerable<ProductVM> GetMappedDatas(IEnumerable<Product> products)
        {
            return products.Select(m => new ProductVM()
            {
                Id = m.Id,
                Name = m.Name,
                CategoryName = m.Category.Name,
                Price = m.Price,
                Description = m.Description,
                MainImage = m.ProductImages?.FirstOrDefault(m => m.isMain)?.Name
            });
        }
    }
}
