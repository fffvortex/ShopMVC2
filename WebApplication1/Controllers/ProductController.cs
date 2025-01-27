using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopMVC2.Shared;

namespace ShopMVC2.Controllers
{
    [Authorize(Roles = nameof(Constants.Roles.Admin))]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IFileService _fileService;
        public ProductController(IProductRepository productRepository,
            IProductTypeRepository productTypeRepository, IFileService fileService)
        {
            _fileService = fileService;
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
        }

        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetProducts();
            return View(products);
        }
        public async Task<IActionResult> AddProduct()
        {
            var typeSelectList = (await _productTypeRepository.GetProductTypes())
                .Select(type => new SelectListItem
                {
                    Text = type.TypeTitle,
                    Value = type.Id.ToString()
                });
            var productToAdd = new ProductDTO
            {
                ProductTypeList = typeSelectList
            };
            return View(productToAdd);
        }

        [HttpPost]

        public async Task<IActionResult> AddProduct(ProductDTO productToAdd)
        {
            var typeSelectList = (await _productTypeRepository.GetProductTypes())
                .Select(type => new SelectListItem
                {
                    Text = type.TypeTitle,
                    Value = type.Id.ToString()
                });
            productToAdd.ProductTypeList = typeSelectList;
            if (!ModelState.IsValid)
            {
                return View(productToAdd);
            }
            try
            {
                if (productToAdd.ImageFile != null)
                {
                    if (productToAdd.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Image file can not exceed 1 MB");
                    }
                    string[] allowedExtentions = [".jpeg", ".png", ".jpg", ".webp"];
                    string imageName = await _fileService
                        .SaveFile(productToAdd.ImageFile, allowedExtentions);
                    productToAdd.ProductImage = imageName;
                }
                Product product = new()
                {
                    Id = productToAdd.Id,
                    ProductTitle = productToAdd.ProductTitle,
                    ProductImage = productToAdd.ProductImage,
                    Description = productToAdd.Description,
                    Price = productToAdd.Price,
                    ProductTypeId = productToAdd.ProductTypeId,
                    Quantity = productToAdd.Quantity,
                };
                await _productRepository.AddProduct(product);
                TempData["successMessage"] = "Product is added successfully";
                return RedirectToAction(nameof(AddProduct));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToAdd);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToAdd);
            }
            catch (Exception)
            {
                TempData["errorMessage"] = "Error on saving data";
                return View(productToAdd);
            }
        }

        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                TempData["errorMessage"] = $"Product with id: {id} does not found";
                return RedirectToAction(nameof(GetAllProducts));
            }
            var typeSelectList = (await _productTypeRepository.GetProductTypes())
                .Select(type => new SelectListItem
                {
                    Text = type.TypeTitle,
                    Value = type.Id.ToString(),
                    Selected = type.Id == product.ProductTypeId
                });
            ProductDTO productToUpdate = new()
            {
                ProductTypeList = typeSelectList,
                ProductTitle = product.ProductTitle,
                Description = product.Description,
                Price = product.Price,
                ProductTypeId = product.ProductTypeId
            };
            return View(productToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductDTO productToUpdate)
        {
            var typeSelectList = (await _productTypeRepository.GetProductTypes())
                .Select(type => new SelectListItem
                {
                    Text = type.TypeTitle,
                    Value = type.Id.ToString(),
                    Selected = type.Id == productToUpdate.ProductTypeId
                });
            productToUpdate.ProductTypeList = typeSelectList;

            if (!ModelState.IsValid)
            {
                return View(productToUpdate);
            }
            try
            {
                string oldImage = "";
                if (productToUpdate.ImageFile != null)
                {
                    if (productToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Image file can not exceed 1 MB");
                    }
                    string[] allowedExtentions = [".jpeg", ".png", ".jpg", ".webp"];
                    string imageName = await _fileService
                        .SaveFile(productToUpdate.ImageFile, allowedExtentions);
                    oldImage = productToUpdate.ProductImage;
                    productToUpdate.ProductImage = imageName;
                }
                Product product = new()
                {
                    Id = productToUpdate.Id,
                    ProductTitle = productToUpdate.ProductTitle,
                    Description = productToUpdate.Description,
                    ProductTypeId = productToUpdate.ProductTypeId,
                    Price = productToUpdate.Price,
                    ProductImage = productToUpdate.ProductImage
                };
                await _productRepository.UpdateProduct(product);
                if (!string.IsNullOrWhiteSpace(oldImage))
                {
                    _fileService.DeleteFile(oldImage);
                }
                TempData["successMessage"] = "Product is updated successfully";
                return RedirectToAction(nameof(GetAllProducts));
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToUpdate);
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(productToUpdate);
            }
            catch (Exception)
            {
                TempData["errorMessage"] = "Error on saving data";
                return View(productToUpdate);
            }
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);
                if (product == null)
                {
                    TempData["errorMessage"] = $"Product with id: {id} does not found";
                }
                else
                {
                    await _productRepository.DeleteProduct(product);
                    if (!string.IsNullOrWhiteSpace(product.ProductImage))
                    {
                        _fileService.DeleteFile(product.ProductImage);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (FileNotFoundException ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["errorMessage"] = "Error on deleting the data";
            }
            return RedirectToAction(nameof(GetAllProducts));
        }
    }
}
