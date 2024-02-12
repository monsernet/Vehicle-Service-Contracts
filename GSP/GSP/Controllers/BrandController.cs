using GSP.Data;
using GSP.Models.Domain;
using GSP.Models.ViewModels;
using GSP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace GSP.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandRepository brandRepository;

        public BrandController(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Brands";
            ViewData["navItem3"] = "New Brand";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBrandRequest addBrandRequest)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Brands";
            ViewData["navItem3"] = "New Brand";
            //Mapping addBrandRequest to Brand Model
            var brand = new Brand
            { Name = addBrandRequest.Name };
            await brandRepository.AddBrandAsync(brand);
            return RedirectToAction("List");

        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Brands";
            ViewData["navItem3"] = "List";
            var brands = await brandRepository.GetAllBrandsAsync();
            
            return View(brands);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Brands";
            ViewData["navItem3"] = "Edit Brand";
            // get brand based on selected id
            var brand = await brandRepository.GetBrandByIdAsync(id);
            if (brand != null)
            {
                var editBrandRequest = new EditBrandRequest
                {
                    Id = brand.Id,
                    Name = brand.Name
                };
                return View(editBrandRequest);
            } else
            {
                //show error message -- Brand not found 
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult>Edit(EditBrandRequest editBrandRequest)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Brands";
            ViewData["navItem3"] = "Edit Brand";
            var brand = new Brand 
            {
                Id = editBrandRequest.Id,
                Name = editBrandRequest.Name 
            };
            var updatedBrand = await brandRepository.UpdateBrandAsync(brand);
            if (updatedBrand == null)
            {
                //show error message -- brand not updated
                return RedirectToAction("Edit", new { id = editBrandRequest.Id });
            } else
            {
                //show success message -- brand updated successfully 
                return RedirectToAction("List");
            }
           
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brandToDelete = await brandRepository.DeleteBrandAsync(id);
            if (brandToDelete == null)
            {
                //show error message -- brand not deleted 
                return RedirectToAction("List");
            } else
            {
                // show success message --- brand deleted successfully
                return RedirectToAction("list");
            }
        }
    }
}
