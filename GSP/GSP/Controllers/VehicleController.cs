using GSP.Models.Domain;
using GSP.Models.ViewModels;
using GSP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GSP.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IBrandRepository brandRepository;

        public VehicleController( IVehicleRepository vehicleRepository, IBrandRepository brandRepository)
        {
            this.vehicleRepository = vehicleRepository;
            this.brandRepository = brandRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add ()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Vehicles";
            ViewData["navItem3"] = "Add New Vehicle";
            //Get Brands from Repository
            var brands = await brandRepository.GetAllBrandsAsync();

            var brandList = brands.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.Models = brandList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddVehicleRequest addVehicleRequest)
        {
            var vehicle = new Vehicle
            {
                Name = addVehicleRequest.Name,
                BrandId = addVehicleRequest.BrandId
            };
            

            await vehicleRepository.AddVehicleAsync(vehicle);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Vehicles";
            ViewData["navItem3"] = "Change Vehicle Details";
            // Get the vehicle by id
            var vehicle = await vehicleRepository.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            // Get all brands from the repository
            var brands = await brandRepository.GetAllBrandsAsync();
            var brandList = brands.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();

            // Find the selected brand name (modelName) based on the vehicle's BrandId
            var selectedBrand = await brandRepository.GetBrandByIdAsync(vehicle.BrandId);
            var selectedBrandName = selectedBrand?.Name;

            // Create the ViewModel
            var vehicleViewModel = new EditVehicleRequest
            {
                Id = vehicle.Id,
                Name = vehicle.Name,
                BrandId = vehicle.BrandId,
                Brands = brandList // Set the Brands property with the list of brands
            };
            return View(vehicleViewModel);

            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditVehicleRequest editVehicleRequest)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Vehicles";
            ViewData["navItem3"] = "Edit Vehicle";

            var vehicle = new Vehicle
            {
                Id = editVehicleRequest.Id, 
                Name = editVehicleRequest.Name,
                BrandId = editVehicleRequest.BrandId
            };
            var updatedVehicle = await vehicleRepository.UpdateVehicleAsync(vehicle);
            if (updatedVehicle != null)
            {
                TempData["SuccessMessage"] = "Vehicle updated successfully";
                return RedirectToAction("List");
            }
            else
            {
                TempData["AlertMessage"] = "Error occurred. Vehicle not updated";
                return View(editVehicleRequest);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Vehicles";
            ViewData["navItem3"] = "List";
            var vehicles = await vehicleRepository.GetAllVehiclesAsync();
            var vehicleViewModels = await MapVehiclesToViewModelsAsync(vehicles);

            return View(vehicleViewModels);
            
        }

        private async Task<List<VehicleViewModel>> MapVehiclesToViewModelsAsync(IEnumerable<Vehicle> vehicles)
        {
            var vehicleViewModels = new List<VehicleViewModel>();

            foreach (var vehicle in vehicles)
            {
                var model = await brandRepository.GetBrandByIdAsync(vehicle.BrandId);
                var viewModel = new VehicleViewModel
                {
                    Id = vehicle.Id,
                    Name = vehicle.Name,
                    BrandId = vehicle.BrandId,
                    BrandName = model?.Name // Set the ModelName property using the fetched model name
                };
                vehicleViewModels.Add(viewModel);
            }

            return vehicleViewModels;
        }






    }
}
