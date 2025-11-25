using FoodEatsBlazor.Data;
using FoodEatsBlazor.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace FoodEatsBlazor.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductRepository(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Product> CreateAsync(Product obj)
        {
            await _db.Product.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var obj = await _db.Product.FirstOrDefaultAsync(u => u.Id == id);
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('/'));

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            if(obj != null)
            {
                _db.Product.Remove(obj);
                // Return if more than 0 changes were successfully made
                return await _db.SaveChangesAsync() > 0;
            }
            // No categories were deleted
            return false;
        }

        public async Task<Product> GetAsync(int id)
        {
            var obj = await _db.Product.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                // no product with given id was found
                return obj;
            }
            return new Product();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Product.Include(u=>u.Category).ToListAsync();
        }

        public async Task<Product> UpdateAsync(Product obj)
        {
            var objFromDb = await _db.Product.FirstOrDefaultAsync(u => u.Id == obj.Id);
            if(objFromDb is not null)
            {
                // update
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.ImageUrl = obj.ImageUrl;
                objFromDb.CategoryId= obj.CategoryId;
                objFromDb.Price = obj.Price;
                _db.Product.Update(objFromDb);
                await _db.SaveChangesAsync();
                return objFromDb;
            }
            // unsuccessful change - return old obj
            return obj;
        }
    }
}
