using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopMVC2.Controllers
{
    [Authorize(Roles = nameof(Constants.Roles.Admin))]
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeRepository _productTypeRepository;

        public ProductTypeController(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
        }
        public async Task<IActionResult> GetAllProductTypes()
        {
            var types = await _productTypeRepository.GetProductTypes();
            return View(types);
        }
        public IActionResult AddProductType()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProductType(ProductTypeDTO productType)
        {
            if (!ModelState.IsValid)
            {
                return View(productType);
            }
            try
            {
                var productTypeToAdd = new ProductType
                {
                    Id = productType.Id,
                    TypeTitle = productType.ProductTypeTitle
                };
                await _productTypeRepository.AddProductType(productTypeToAdd);
                TempData["successMessage"] = "Type added successfully";
                return RedirectToAction(nameof(AddProductType));
            }
            catch (Exception)
            {
                TempData["errorMessage"] = "Type could not added";
                return View(productType);
            }
        }

        public async Task<IActionResult> UpdateProductType(int id)
        {
            var type = await _productTypeRepository.GetProductTypeById(id);
            if (type == null)
            {
                throw new InvalidOperationException($"Type with id: {id} does not found");
            }
            var productTypeToUpdate = new ProductTypeDTO
            {
                Id = id,
                ProductTypeTitle = type.TypeTitle
            };
            return View(productTypeToUpdate);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProductType(ProductTypeDTO productToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(productToUpdate);
            }
            try
            {
                var type = new ProductType
                {
                    Id = productToUpdate.Id,
                    TypeTitle = productToUpdate.ProductTypeTitle
                };
                await _productTypeRepository.UpdateProductType(type);
                TempData["successMessage"] = "Type is updated successfully";
                return RedirectToAction(nameof(GetAllProductTypes));
            }
            catch (Exception)
            {
                TempData["errorMessage"] = "Type could not updated";
                return View(productToUpdate);
            }
        }

        public async Task<IActionResult> DeleteProductType(int id)
        {
            var type = await _productTypeRepository.GetProductTypeById(id);
            if (type == null)
            {
                throw new InvalidOperationException($"Type with id: {id} does not found");
            }
            await _productTypeRepository.DeleteProductType(type);
            return RedirectToAction(nameof(GetAllProductTypes));
        }
    }
}
