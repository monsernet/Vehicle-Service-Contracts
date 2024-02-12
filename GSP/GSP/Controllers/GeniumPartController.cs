using GSP.DTO;
using GSP.Models.Domain;
using GSP.Models.ViewModels;
using GSP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GSP.Controllers
{
    public class GeniumPartController : Controller
    {
        private readonly IGeniumPartRepository geniumPartRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IVehicleTypeRepository vehicleTypeRepository;

        public GeniumPartController(
            IGeniumPartRepository geniumPartRepository, 
            IVehicleRepository vehicleRepository,
            IVehicleTypeRepository vehicleTypeRepository)        
        {
            this.geniumPartRepository = geniumPartRepository;
            this.vehicleRepository = vehicleRepository;
            this.vehicleTypeRepository = vehicleTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Parts";
            ViewData["navItem3"] = "Add New Part";
            //Get Vehicles from Repository
            var vehicles = await vehicleRepository.GetAllVehiclesAsync();
            var vehicleList = vehicles.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.Vehicles = vehicleList;
            //Get Vehicle Types from Repository
            var vehTypes = await vehicleTypeRepository.GetVehicleTypesAsync();
            var vehicleTypeList = vehTypes.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.VehicleTypes = vehicleTypeList;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddGeniumPartRequest addGeniumPartRequest)
        {
            var part = new GeniumPart
            {
                PartNumber = addGeniumPartRequest.PartNumber,
                PartName = addGeniumPartRequest.PartName,
                UnitPrice = addGeniumPartRequest.UnitPrice,
                QtyPerSet = addGeniumPartRequest.QtyPerSet,
                VehicleId = addGeniumPartRequest.VehicleId,
                VehicleTypeId = addGeniumPartRequest.VehicleTypeId

            };


            await geniumPartRepository.AddPartAsync(part);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Parts";
            ViewData["navItem3"] = "Edit Part Details";
            // Get the vehicle by id
            var part = await geniumPartRepository.GetPartByIdAsync(id);
            if (part == null)
            {
                return NotFound();
            }

            // Get all vehicle from the repository
            var vehicles = await vehicleRepository.GetAllVehiclesAsync();
            var vehicleList = vehicles.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            // ********* Find the selected vehicle name  based on the part's VehicleId
            var selectedVehicle = await vehicleRepository.GetVehicleByIdAsync(part.VehicleId);
            var selectedVehicleName = selectedVehicle?.Name;

            // Get all vehicle types from the repository
            var vehicleTypes = await vehicleTypeRepository.GetVehicleTypesAsync();
            var vehicleTypeList = vehicleTypes.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            // ********* Find the selected vehicle name  based on the part's VehicleId
            var selectedVehicleType = await vehicleTypeRepository.GetVehicleTypeByIdAsync(part.VehicleTypeId);
            var selectedVehicleTypeName = selectedVehicleType?.Name;

            // Create the ViewModel
            var partViewModel = new EditGeniumPartRequest
            {
                Id = part.Id,
                PartNumber = part.PartNumber,
                PartName = part.PartName,
                UnitPrice = part.UnitPrice,
                QtyPerSet = part.QtyPerSet,
                VehicleId = part.VehicleId,
                VehicleTypeId = part.VehicleTypeId,
                Vehicles = vehicleList,
                VehicleTypes = vehicleTypeList
            };
            return View(partViewModel);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditGeniumPartRequest editGeniumPartRequest)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Parts";
            ViewData["navItem3"] = "Edit Part Details";

            var part = new GeniumPart
            {
                Id = editGeniumPartRequest.Id,
                PartNumber = editGeniumPartRequest.PartNumber,
                PartName = editGeniumPartRequest.PartName, 
                UnitPrice = editGeniumPartRequest.UnitPrice,
                QtyPerSet= editGeniumPartRequest.QtyPerSet,
                VehicleId = editGeniumPartRequest.VehicleId,
                VehicleTypeId= editGeniumPartRequest.VehicleTypeId,
            };
            var updatedPart = await geniumPartRepository.UpdatePartAsync(part);
            if (updatedPart != null)
            {
                TempData["SuccessMessage"] = "Service Part updated successfully";
                return RedirectToAction("List");
            }
            else
            {
                TempData["AlertMessage"] = "Error occurred. Service Part not updated";
                return View(editGeniumPartRequest);
            }

        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Parts";
            ViewData["navItem3"] = "List";
            var parts = await geniumPartRepository.GetAllPartsAsync();
            var partViewModels = await MapPartsToViewModelsAsync(parts);

            return View(partViewModels);

        }

        private async Task<List<GeniumPartViewModel>> MapPartsToViewModelsAsync(IEnumerable<GeniumPart> parts)
        {
            var partViewModels = new List<GeniumPartViewModel>();

            foreach (var part in parts)
            {
                var vehicle = await vehicleRepository.GetVehicleByIdAsync(part.VehicleId);
                var vehicleType = await vehicleTypeRepository.GetVehicleTypeByIdAsync(part.VehicleTypeId);
                var viewModel = new GeniumPartViewModel
                {
                    Id = part.Id,
                    PartNumber = part.PartNumber,
                    PartName = part.PartName,
                    UnitPrice = part.UnitPrice,
                    QtyPerSet = part.QtyPerSet,
                    VehicleTypeId = part.VehicleTypeId,
                    VehicleName = vehicle?.Name, 
                    VehicleTypeName = vehicleType?.Name
                };
                partViewModels.Add(viewModel);
            }

            return partViewModels;
        }

        [HttpGet]
        public async Task<IActionResult> UploadExcel()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Parts";
            ViewData["navItem3"] = "Add Bulk Parts";
            //Get Vehicles from Repository
            var vehicles = await vehicleRepository.GetAllVehiclesAsync();
            var vehicleList = vehicles.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.Vehicles = vehicleList;
            //Get Vehicle Types from Repository
            var vehicleTypes = await vehicleTypeRepository.GetVehicleTypesAsync();
            var vehicleTypeList = vehicleTypes.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.VehicleTypes = vehicleTypeList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file, int vehicleId, int vehicleTypeId)
        {
            if (file == null || file.Length == 0)
            {
                // Handle the case when no file is selected
                ModelState.AddModelError("file", "Please select a file");
                return View();
            }

            try
            {
                var excelData = await ReadExcelData(file.OpenReadStream(), vehicleId, vehicleTypeId);
                await geniumPartRepository.AddBulkPartsAsync(excelData);

                // Optionally, provide feedback to the user
                TempData["SuccessMessage"] = " Parts uploaded successfully";
                return RedirectToAction("List");

            }
            catch (Exception ex)
            {
                // Handle exceptions, log, and provide feedback to the user
                TempData["ErrorMessage"] = $"  An error occurred: {ex.Message}";
                return RedirectToAction("UploadExcel");
            }

            
        }

        private async Task<IEnumerable<GeniumPartExcelDto>> ReadExcelData(Stream stream, int vehicleId, int vehicleTypeId)
        {
            List<GeniumPartExcelDto> excelData = new List<GeniumPartExcelDto>();

            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                //using (var fileStream = new FileStream(memoryStream, FileMode.Open, FileAccess.Read))
                using (var fileStream = new MemoryStream(memoryStream.ToArray()))
                {
                    IWorkbook workbook = new XSSFWorkbook(fileStream);
                    ISheet sheet = workbook.GetSheetAt(0); // Assuming data is in the first sheet

                    for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++) // Start from 1 to skip the header row
                    {
                        IRow row = sheet.GetRow(rowIdx);
                        if (row == null) continue;

                        GeniumPartExcelDto dto = new GeniumPartExcelDto
                        {
                            PartNumber = row.GetCell(0).ToString(),
                            PartName = row.GetCell(1).ToString(),
                            QtyPerSet = Convert.ToInt32(row.GetCell(2).ToString()),
                            UnitPrice = Convert.ToSingle(row.GetCell(3).ToString()),
                            VehicleId = vehicleId,  // Set the VehicleId from the dropdown
                            VehicleTypeId = vehicleTypeId,  // Set the VehicleId from the dropdown

                        };

                        excelData.Add(dto);
                    }
                }
            }

            return excelData;
        }

        

    }
}
