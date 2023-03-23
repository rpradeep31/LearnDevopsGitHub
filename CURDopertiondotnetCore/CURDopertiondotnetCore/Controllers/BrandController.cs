using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CURDopertiondotnetCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace CURDopertiondotnetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandContext _brandContext;
        public BrandController(BrandContext brandContext)
        {
            _brandContext = brandContext;
        }
        [HttpGet]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            if (_brandContext.Brands == null)
            {
                return NotFound();
            }
            return await _brandContext.Brands.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrands(int id)
        {
            if (_brandContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await _brandContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return brand;
        }
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _brandContext.Brands.Add(brand);
            await _brandContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrands), new { id = brand.ID }, brand);
        }

        [HttpPut]
        public async Task<ActionResult<Brand>> putBrand(int id, Brand brand)
        {
            if (id!= brand.ID)
            {
                return BadRequest();
            }
            _brandContext.Entry(brand).State = EntityState.Modified;
            try
            {
                await _brandContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!Brandavailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                
            }
            return Ok();
        }
        private bool Brandavailable (int id)
        {
            return (_brandContext.Brands?.Any(x => x.ID == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            if(_brandContext.Brands==null)
            {
                return NotFound();
            }
            var brand = await _brandContext.Brands.FindAsync(id);
            if (brand==null)
            {
                return NotFound();
            }
            _brandContext.Brands.Remove(brand);
            await _brandContext.SaveChangesAsync();
            return Ok();
        }
    }
}
