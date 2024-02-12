using GSP.Models.Domain;
using GSP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GSP.Controllers
{
    public class VehicleTypeController : Controller
    {
        private readonly IVehicleTypeRepository _repo;
        private readonly IVehicleRepository vehicleRepository;

        public VehicleTypeController(
            IVehicleTypeRepository repo,
            IVehicleRepository vehicleRepository)
        {
            _repo = repo;
            this.vehicleRepository = vehicleRepository;
        }

        public async Task<IActionResult> Index()
        {
            var vehicleTypes = await _repo.GetVehicleTypesAsync();
            return View(vehicleTypes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vehicleType = await _repo.GetVehicleTypeByIdAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }
            return View(vehicleType);
        }

        public async Task<IActionResult> Create()
        {
            var vehicles = await vehicleRepository.GetAllVehiclesAsync();
            var vehicleOptions = vehicles.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Name
            }).ToList();
            ViewData["VehicleOptions"] = vehicleOptions;
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleType vehicleType)
        {
            if (!ModelState.IsValid)
            {
                return View(vehicleType);
            }

            try
            {
                await _repo.AddVehicleTypeAsync(vehicleType);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, log error, etc.
                ModelState.AddModelError("", "An error occurred while creating the vehicle type.");
                return View(vehicleType);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vehicleType = await _repo.GetVehicleTypeByIdAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }
            return View(vehicleType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleType vehicleType)
        {
            if (id != vehicleType.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(vehicleType);
            }

            try
            {
                await _repo.UpdateVehicleTypeAsync(vehicleType);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, log error, etc.
                ModelState.AddModelError("", "An error occurred while updating the vehicle type.");
                return View(vehicleType);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vehicleType = await _repo.GetVehicleTypeByIdAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }
            return View(vehicleType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _repo.DeleteVehicleTypeAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle exception appropriately, log error, etc.
                return View("Delete", new { id = id, error = "An error occurred while deleting the vehicle type." });
            }
        }
    }
}
