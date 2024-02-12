using AuthSystem.Areas.Identity.Data;
using GSP.Models.Domain;
using GSP.Models.ViewModels;
using GSP.Repositories;
using GSP.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using NPOI.XSSF.Streaming.Values;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Numerics;
using System.Web;

namespace GSP.Controllers
{
    public class ServiceContractController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IServiceContractRepository serviceContractRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IMileageRepository mileageRepository;
        private readonly IServiceContractPartRepository serviceContractPartRepository;
        private readonly PdfService pdfService;
        private readonly IViewRenderService viewRenderService;
        private readonly IUserRepository userRepository;
        private readonly IGeniumPartRepository geniumPartRepository;
        private readonly IVehicleServiceRepository vehicleServiceRepository;
        private readonly IVehicleTypeRepository vehicleTypeRepository;

        public ServiceContractController(
            UserManager<ApplicationUser> userManager,
            IServiceContractRepository serviceContractRepository,
            ICustomerRepository customerRepository,
            IMileageRepository mileageRepository,
            IServiceContractPartRepository serviceContractPartRepository,
            PdfService pdfService,
            IViewRenderService viewRenderService,
            IUserRepository userRepository,
            IGeniumPartRepository geniumPartRepository,
            IVehicleServiceRepository vehicleServiceRepository,
            IVehicleTypeRepository vehicleTypeRepository)
        {
            this.userManager = userManager;
            this.serviceContractRepository = serviceContractRepository;
            this.customerRepository = customerRepository;
            this.mileageRepository = mileageRepository;
            this.serviceContractPartRepository = serviceContractPartRepository;
            this.pdfService = pdfService;
            this.viewRenderService = viewRenderService;
            this.userRepository = userRepository;
            this.geniumPartRepository = geniumPartRepository;
            this.vehicleServiceRepository = vehicleServiceRepository;
            this.vehicleTypeRepository = vehicleTypeRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Contracts";
            ViewData["navItem3"] = "New Service Contract";
            //Get Customers from Repository
            var customers = await customerRepository.GetAllCustomersAsync();
            var customerList = customers.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.CustomerName }).ToList();
            ViewBag.Customers = customerList;
            var mileages = await mileageRepository.GetAllMileagesAsync();
            var mileageList = mileages.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.Mileages = mileageList;
            //Get the count of service contracts
            var nbContracts = await serviceContractRepository.GetContractCountAsync();
            ViewBag.ContractCount = "SC-" + DateTime.Now.Year + "-" + (nbContracts + 1).ToString("D4");


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DisplayContractDetails(string Veh, string StMil, string EndMil, string TotCost)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Contracts";
            ViewData["navItem3"] = "New Service Contract";
            //Get Customers from Repository
            var customers = await customerRepository.GetAllCustomersAsync();
            var customerList = customers.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.CustomerName }).ToList();
            ViewBag.Customers = customerList;
            var mileages = await mileageRepository.GetAllMileagesAsync();
            var mileageList = mileages.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.Mileages = mileageList;
            // Service Package Caption 
            ViewBag.PackageCaption = $"Service Package From {StMil} To {EndMil} ";
            // Total Cost 
            ViewBag.TotalPackageCost = TotCost;
            //Get the count of service contracts
            var nbContracts = await serviceContractRepository.GetContractCountAsync();
            ViewBag.ContractCount = "SC-" + DateTime.Now.Year + "-" + (nbContracts + 1).ToString("D4");
            //Begining Date - Today Date
            ViewBag.BeginingDate = DateTime.Now;
            //Get the vehicle amd Mileage details passed
            if (string.IsNullOrEmpty(Veh))
            {
                ViewBag.VehicleName = "No Vehicle";
            }
            else
            {
                ViewBag.VehicleName = Veh;
            }
            if (string.IsNullOrEmpty(StMil))
            {
                ViewBag.StartMileage = "No Starting Mileage";
            }
            else
            {
                ViewBag.StartMileage = StMil;
            }
            if (string.IsNullOrEmpty(EndMil))
            {
                ViewBag.EndMileage = "No Ending Mileage";
            }
            else
            {
                ViewBag.EndMileage = EndMil;
            }


            return View();
        }



        [HttpPost]
        public async Task<IActionResult> SaveContractDetails(ServiceContractViewModel serviceContractViewModel, double DiscountServiceCost, double DiscountPartCost, double DiscountManualCost, double finalAmount, List<int> PartId, List<decimal> UnitCost, List<int> PartQty, List<string> checkedStatus, List<string> ManualPartNumber, List<string> ManualPartName, List<int> ManualPartQty, List<decimal> ManualUnitCost)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Contracts";
            ViewData["navItem3"] = "List";
            //User Details 
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var userId = currentUser.Id;
            var UserFullName = currentUser.FirstName + " " + currentUser.LastName;
            var customer = new Customer
            {
                CustomerName = serviceContractViewModel.CustName,
                CustomerCode = serviceContractViewModel.CustCode,
                //CivilId = addCustomerRequest.CivilId,
                Phone = serviceContractViewModel.CustPhone,
                Address = serviceContractViewModel.CustAddress,
                Email = serviceContractViewModel.CustEmail
            };
            var cust = await customerRepository.AddCustomerAsync(customer);


            var contract = new ServiceContract
            {

                Reference = serviceContractViewModel.Reference,
                VehicleCaption = serviceContractViewModel.VehicleCaption,
                VehicleVariant = serviceContractViewModel.VehicleVariant,
                VehicleId = serviceContractViewModel.VehicleId,
                VehicleTypeId = serviceContractViewModel.VehicleTypeId,
                ModelYear = serviceContractViewModel.ModelYear,
                VinNumber = serviceContractViewModel.VinNumber,
                RegistrationNumber = serviceContractViewModel.RegistrationNumber,
                DeliveryDate = serviceContractViewModel.DeliveryDate,
                StartMileage = serviceContractViewModel.StartMileage,
                EndMileage = serviceContractViewModel.EndMileage,
                ContractDuration = serviceContractViewModel.ContractDuration,
                StartDate = serviceContractViewModel.StartDate,
                ExpiryDate = serviceContractViewModel.ExpiryDate,
                ServDisc = DiscountServiceCost,
                PartsDisc = DiscountPartCost,
                AddPartsDiscount = DiscountManualCost,
                TotalCost = finalAmount,
                CustomerId = cust.Id,
                UserId = userId,
                AddedOn = @DateTime.Now
            };
            await serviceContractRepository.AddContractAsync(contract);

            // Saving Parts

            int numRows = PartId.Count;

            for (int i = 0; i < numRows; i++)
            {
                var partId = PartId[i];
                var unitCost = UnitCost[i];
                var qty = PartQty[i];
                var checkedStatusValue = checkedStatus[i];

                var part = new ServiceContractPart
                {
                    ServiceContractId = contract.Id,
                    PartId = partId,
                    Qty = qty,
                    UnitCost = unitCost
                };

                if (checkedStatusValue == "checked")
                {
                    await serviceContractPartRepository.AddContractPartAsync(part);
                }
            }

            //Saving Additional Parts
            int nbAdditionalParts = ManualUnitCost.Count();
            for (int i = 0; i < nbAdditionalParts; i++)
            {

                var manualPartNumber = ManualPartNumber[i];
                var manualPartName = ManualPartName[i];
                var manualPartQty = ManualPartQty[i];
                var manualUnitCost = ManualUnitCost[i];

                var manualPart = new ServiceContractAdditionalPart
                {
                    ServiceContractId = contract.Id,
                    PartNumber = manualPartNumber,
                    PartName = manualPartName,
                    PartQty = manualPartQty,
                    PartUnitCost = manualUnitCost
                };
                await serviceContractPartRepository.AddContractManualPartAsync(manualPart);
            }



            return RedirectToAction("List");
        }

        // Print Service Contract Details 
        public async Task<IActionResult> Print(int id)
        {
            var contract = await serviceContractRepository.GetContractWithPartsAndGeniumPartsAsync(id);
            var customerId = contract.CustomerId;
            var customer = await customerRepository.GetCustomerByIdAsync(customerId);
            var customerName = customer.CustomerName;
            var customerAddress = customer.Address;
            var customerPhone = customer.Phone;
            ViewBag.CustomerName = customerName;
            ViewBag.CustomerAddress = customerAddress;
            ViewBag.CustomerPhone = customerPhone;

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // Print Service Contract Details 
        public async Task<IActionResult> PrintContract(int id)
        {
            var contract = await serviceContractRepository.GetContractWithPartsAndGeniumPartsAsync(id);
            var customerId = contract.CustomerId;
            var customer = await customerRepository.GetCustomerByIdAsync(customerId);
            var customerName = customer.CustomerName;
            var customerAddress = customer.Address;
            var customerPhone = customer.Phone;
            ViewBag.CustomerName = customerName;
            ViewBag.CustomerAddress = customerAddress;
            ViewBag.CustomerPhone = customerPhone;

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }
        public async Task<IActionResult> PrintView(int id)
        {

            var contract = await serviceContractRepository.GetContractWithPartsAndGeniumPartsAsync(id);
            var customerId = contract.CustomerId;
            var customer = await customerRepository.GetCustomerByIdAsync(customerId);
            var customerName = customer.CustomerName;
            var customerAddress = customer.Address;
            var customerPhone = customer.Phone;
            ViewBag.CustomerName = customerName;
            ViewBag.CustomerAddress = customerAddress;
            ViewBag.CustomerPhone = customerPhone;

            if (contract == null)
            {
                return NotFound();
            }
            // Render the view to HTML string
            string htmlContent = await viewRenderService.RenderToStringAsync("ServiceContract/PrintContract", null);

            // Generate PDF
            byte[] pdfBytes = pdfService.GeneratePdf(htmlContent);

            // Return PDF as a file
            //return File(pdfBytes, "application/pdf", "output.pdf");
            return View(contract);
        }


        public async Task<IActionResult> List()
        {
            var contracts = await serviceContractRepository.GetAllContractsAsync();
            var contractViewModels = new List<ServiceContractViewModel>();
            var msrResult = Request.Query["msrResult"];
            if (!String.IsNullOrEmpty(msrResult))
            {
                msrResult = "You have updated the service contract successfully";
            }
            ViewBag.MsrResult = msrResult;
            foreach (var contract in contracts)
            {
                var customerName = (await customerRepository.GetCustomerByIdAsync(contract.CustomerId))?.CustomerName;
                var userId = contract.UserId;
                var user = await userManager.FindByIdAsync(userId);
                var userFullName = (user == null) ? "N/A" : (user.FirstName + ' ' + user.LastName);
                var contractViewModel = new ServiceContractViewModel
                {
                    Id = contract.Id,
                    Reference = contract.Reference,
                    VehicleCaption = contract.VehicleCaption,
                    ModelYear = contract.ModelYear,
                    VinNumber = contract.VinNumber,
                    RegistrationNumber = contract.RegistrationNumber,
                    DeliveryDate = contract.DeliveryDate,
                    StartMileage = contract.StartMileage,
                    EndMileage = contract.EndMileage,
                    ContractDuration = contract.ContractDuration,
                    StartDate = contract.StartDate,
                    ExpiryDate = contract.ExpiryDate,
                    CustomerId = contract.CustomerId,
                    TotalCost = contract.TotalCost,
                    VehicleVariant = contract.VehicleVariant,
                    CustomerName = customerName,
                    userFullName = userFullName,
                    AddedOn = contract.AddedOn


                };
                contractViewModels.Add(contractViewModel);
            }
            return View(contractViewModels);
        }

        /*
         * Edit Service Contract 
         */
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var contract = await serviceContractRepository.GetContractByIdAsync(id);
            if (contract == null)
            {
                //error message
                return NotFound();
            }
            var customer = await customerRepository.GetCustomerByIdAsync(contract.CustomerId);
            var customerName = customer.CustomerName;
            var user = await userRepository.GetUserById(contract.UserId);
            if (user != null)
            {
                var userFullName = user.FirstName + " " + user.LastName;
            }
            else
            {
                var userFullName = "N/A";
            }


            var startMileage = await mileageRepository.GetMileageByNameAsync(contract.StartMileage);
            var startMileageId = startMileage.Id;
            var endMileage = await mileageRepository.GetMileageByNameAsync(contract.EndMileage);
            var endMileageId = endMileage.Id;

            //Mileages
            //Get Mileages from Repository
            var mileages = await mileageRepository.GetAllMileagesAsync();
            var mileageList = mileages.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.Mileages = mileageList;

            // vehicle Types 
            var vehicleTypes = await vehicleTypeRepository.GetVehicleTypesAsync();
            var vehicleTypeList = vehicleTypes.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.VehicleTypes = vehicleTypeList;

            // retreive genium Parts
            var vehicleParts = await geniumPartRepository.VehicleParts(
                    contract.VehicleId,
                    contract.VehicleTypeId
                 );
            // retreive mileage services
            var mileageResults = await vehicleServiceRepository.SearchServices(
                    contract.VehicleId,
                    startMileageId,
                    endMileageId
                 );
            // retreive the parts that belong to the contract 
            // retreive additional parts
            var contractParts = await geniumPartRepository.ContractParts(contract.Id);
            // retreive additional parts
            var additionalParts = await serviceContractPartRepository.GetContractAdditionalPartsAsync(contract.Id);

            var contractViewModel = new ServiceContractViewModel
            {
                Id = contract.Id,
                VehicleId = contract.VehicleId,
                VehicleTypeId = contract.VehicleTypeId,
                Reference = contract.Reference,
                VehicleCaption = contract.VehicleCaption,
                VehicleVariant = contract.VehicleVariant,
                ModelYear = contract.ModelYear,
                VinNumber = contract.VinNumber,
                RegistrationNumber = contract.RegistrationNumber,
                DeliveryDate = contract.DeliveryDate,
                StartingMileageId = startMileageId,
                EndingMileageId = endMileageId,
                StartMileage = contract.StartMileage,
                EndMileage = contract.EndMileage,
                ContractDuration = contract.ContractDuration,
                StartDate = contract.StartDate,
                ExpiryDate = contract.ExpiryDate,
                ServDisc = contract.ServDisc,
                PartsDisc = contract.PartsDisc,
                AddPartsDiscount = contract.AddPartsDiscount,
                TotalCost = contract.TotalCost,
                CustomerName = customerName,
                CustCode = customer.CustomerCode,
                CustEmail = customer.Email,
                CustPhone = customer.Phone,
                CustAddress = customer.Address,

                SearchResults = mileageResults.ToList(),
                VehiclePartsResults = vehicleParts.ToList(),
                AdditionalParts = additionalParts.ToList(),
                ContractParts = contractParts.ToList(),



            };

            return View(contractViewModel);

        }

        //Get the seleted mileage services details 
        public async Task<IActionResult> GetMileageServices(int vehicleId, int startMileageId, int endMileageId)
        {
            var mileageResults = await vehicleServiceRepository.SearchServices(
                    vehicleId,
                    startMileageId,
                    endMileageId
                 );
            var mileageResult = "";
            foreach (var item in mileageResults)
            {
                mileageResult += "<tr>";
                mileageResult += "<td>" + item.Name + "</td>";
                mileageResult += "<td >" + item.Cost.ToString("F3") + " KD</td>";
                mileageResult += "</tr>";
            }
            return Json(mileageResult);
        }

        //Get the seleted vehicle parts 
        public async Task<IActionResult> GetVehicleParts(int typeId, int vehicleId)
        {
            // retreive genium Parts
            var vehicleParts = await geniumPartRepository.VehicleParts(vehicleId, typeId);
            var partsResult = "";
            var rowCount = 0;
            foreach (var part in vehicleParts)
            {

                partsResult += "<tr>";
                partsResult += "<td>" + @part.PartNumber + "<input type=\"hidden\" name=\"PartId[]\" id=\"PartId_" + @rowCount + "\" value=\"" + @part.Id + "\" /> <input type=\"hidden\" name=\"checkedStatus[]\" id =\"checkedStatus_" + @rowCount + "\" class=\"checkedStatus\" value=\"\"  /></td>";
                partsResult += "<td> " + @part.PartName + " </td>";
                partsResult += "<td>" + @part.UnitPrice + " KD <input type=\"hidden\" name=\"UnitCost[]\" id=\"UnitCost_" + @rowCount + "\" value=\"" + @part.UnitPrice + "\" /></td>";
                partsResult += "<td><input type=\"number\" width=\"50px\" name=\"PartQty[]\" id=\"PartQty_" + @rowCount + "\" class=\"quantityInput\" value=\"1\" min=\"1\" /></td>";
                partsResult += "<td class=\"totalCost\">" + @part.UnitPrice + " KD </td>";
                partsResult += " <td><input type=\"checkbox\" class=\"partCheckbox\" data-unit-price=\"" + @part.UnitPrice + "\" /></td>";
                partsResult += "</tr>";
                rowCount++;
            }
            return Json(partsResult);
        }

        /*
         * Update edited contract 
         */
        [HttpPost]
        public async Task<IActionResult> UpdateContractDetails(ServiceContractViewModel updatedModel, int EditContractId,  double DiscountServiceCost, double DiscountPartCost, double DiscountManualCost, double finalAmount, List<int> PartId, List<decimal> UnitCost, List<int> PartQty, List<string> checkedStatus, List<string> ManualPartNumber, List<string> ManualPartName, List<int> ManualPartQty, List<decimal> ManualUnitCost)
        {
            
            // 1. Retrieve the existing contract from the database
                var contractId = EditContractId;
                var existingContract = await serviceContractRepository.GetContractByIdAsync(contractId);
                
                // 3. Remove old parts and insert new checked parts
                await serviceContractRepository.RemoveOldGeniumParts(existingContract);
                // Incsert New Parts
                int numRows = PartId.Count;

                for (int i = 0; i < numRows; i++)
                {
                    var partId = PartId[i];
                    var unitCost = UnitCost[i];
                    var qty = PartQty[i];
                    var checkedStatusValue = checkedStatus[i];

                    var part = new ServiceContractPart
                    {
                        ServiceContractId = existingContract.Id,
                        PartId = partId,
                        Qty = qty,
                        UnitCost = unitCost
                    };

                    if (checkedStatusValue == "checked")
                    {
                        await serviceContractPartRepository.AddContractPartAsync(part);
                    }
                }

                //4. Remove old additional Parts
                await serviceContractRepository.RemoveOldAdditionalParts(existingContract);
                // Save new additional Parts
                int nbAdditionalParts = ManualUnitCost.Count();
                for (int i = 0; i < nbAdditionalParts; i++)
                {

                    var manualPartNumber = ManualPartNumber[i];
                    var manualPartName = ManualPartName[i];
                    var manualPartQty = ManualPartQty[i];
                    var manualUnitCost = ManualUnitCost[i];

                    var manualPart = new ServiceContractAdditionalPart
                    {
                        ServiceContractId = existingContract.Id,
                        PartNumber = manualPartNumber,
                        PartName = manualPartName,
                        PartQty = manualPartQty,
                        PartUnitCost = manualUnitCost
                    };
                    await serviceContractPartRepository.AddContractManualPartAsync(manualPart);
                }
           
            // Map view model to domain model
            var contractToUpdate = new ServiceContract
                {
                    Id = existingContract.Id,
                    VehicleTypeId = existingContract.VehicleTypeId,
                    StartMileage = updatedModel.StartMileage,
                    EndMileage = updatedModel.EndMileage,
                    ContractDuration = updatedModel.ContractDuration,
                    StartDate = updatedModel.StartDate,
                    ExpiryDate = updatedModel.ExpiryDate,
                    ServDisc = DiscountServiceCost,
                    PartsDisc = DiscountPartCost,
                    AddPartsDiscount = DiscountManualCost,
                    TotalCost = finalAmount
                    
                };
           
            try
            {
                // Call repository method to update contract
                var updatedContract = await serviceContractRepository.UpdateContractAsync(contractToUpdate, DiscountPartCost, DiscountManualCost, finalAmount);
                if (updatedContract != null)
                {
                    TempData["SuccessMessage"] = "Service Contract updated successfully";
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = "The Service Contract you are trying to edit does not exist. Please try again.";
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                TempData["ErrorMessage"] = "Error occurred while updating the contract. Please try again.";
                return RedirectToAction("List");
            }
        //// Handle invalid model state
        //return View(updatedModel);
        }

       
        public async Task<IActionResult> CalculateMileageInterval(string startMileage, string endMileage )
        {
            // Here you can call your existing C# logic
            int startMileageValue = await GetMileageValue(startMileage);
            int endMileageValue = await GetMileageValue(endMileage);
            int mileageInterval = endMileageValue - startMileageValue;
            int nbYears = GetServedYears(mileageInterval);

            // Return the result
            return Json(nbYears);
        }


        private async Task<int> GetMileageValue(string mileageName)
        {
            var mileage = await mileageRepository.GetMileageByNameAsync(mileageName);
            var mileageValue = mileage.MileageValue;
            return mileageValue;
        }

        private int GetServedYears(int mileageInterval)
        {
            if (mileageInterval > 0 && mileageInterval <= 20000)
            {
                return 1; // 1 year
            }
            else if (mileageInterval > 20000 && mileageInterval <= 40000)
            {
                return 2; // 2 years
            }
            else if (mileageInterval > 40000 && mileageInterval <= 60000)
            {
                return 3; // 3 years
            }
            else if (mileageInterval > 60000 && mileageInterval <= 80000)
            {
                return 4; // 4 years
            }
            else
            {
                return 5; // 5 years
            }
        }
    }
}


