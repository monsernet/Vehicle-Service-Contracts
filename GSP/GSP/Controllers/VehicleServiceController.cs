using GSP.Models.Domain;
using GSP.Models.ViewModels;
using GSP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using ServiceReference1;
using System.Net.NetworkInformation;

namespace GSP.Controllers
{
    public class VehicleServiceController : Controller
    {
        private readonly IVehicleServiceRepository vehicleServiceRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IBrandRepository brandRepository;
        private readonly IMileageRepository mileageRepository;
        private readonly IGeniumPartRepository geniumPartRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IServiceContractRepository serviceContractRepository;
        private readonly IVehicleTypeRepository vehicleTypeRepository;

        //private readonly IHttpClientFactory clientFactory;

        private bool searchSubmitted = false;

        public VehicleServiceController(
            IVehicleServiceRepository vehicleServiceRepository,
            IVehicleRepository vehicleRepository,
            IBrandRepository brandRepository,
            IMileageRepository mileageRepository,
            IGeniumPartRepository geniumPartRepository,
            ICustomerRepository customerRepository,
            IServiceContractRepository serviceContractRepository,
            IVehicleTypeRepository vehicleTypeRepository
            //IHttpClientFactory clientFactory
            )
        {
            this.vehicleServiceRepository = vehicleServiceRepository;
            this.vehicleRepository = vehicleRepository;
            this.brandRepository = brandRepository;
            this.mileageRepository = mileageRepository;
            this.geniumPartRepository = geniumPartRepository;
            this.customerRepository = customerRepository;
            this.serviceContractRepository = serviceContractRepository;
            this.vehicleTypeRepository = vehicleTypeRepository;
            //this.clientFactory = clientFactory;
        }

        /**
         * 
         * Add New Service 
         * 
         */

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Rates";
            ViewData["navItem3"] = "New Service Rate";

            //Get Vehicles from Repository
            var vehicles = await vehicleRepository.GetAllVehiclesAsync();
            var vehicleList = vehicles.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.Vehicles = vehicleList;
            //Get Vehicle Types from Repository
            var vehTypes = await vehicleTypeRepository.GetVehicleTypesAsync();
            var vehicleTypeList = vehTypes.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.VehicleTypes = vehicleTypeList;
            //Get Mileages from Repository
            var mileages = await mileageRepository.GetAllMileagesAsync();
            var mileageList = mileages.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.Mileages = mileageList;

            return View();
        }

        private async Task<bool> IsNetworkConnected()
        {
            var pingTask = new Ping().SendPingAsync("192.168.16.50"); 
            var reply = await pingTask;
            return reply.Status == IPStatus.Success;
        }

        [HttpGet]
        public IActionResult SearchVehicle()
        {
            searchSubmitted = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchVehicle(string str)
        {
            searchSubmitted=true;
            ViewBag.SearchSubmitted = searchSubmitted;
            if (!await IsNetworkConnected())
            {
                return StatusCode(503, "No network connection available."); 
            }

            try
            {
                var searchResults = new List<SearchVehicle>();
                string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";

                var client = new MyServiceClient();

                var getData = new GetDataRequest();
                str = str.ToLower();
                getData.sSqlString = "SELECT SO_21_HeaderRecords.WIPNO" +
                                                ", SO_21_HeaderRecords.CREATED" +
                                                ", SO_21_HeaderRecords.REGNO" +
                                                ", SO_21_HeaderRecords.CUSTNAME" +
                                                ", SO_21_HeaderRecords.CHASSIS" +
                                                ", GB_00_UserDetails.EMAIL " +
                                                ", SO_21_HeaderRecords.PHONE " +
                                                ", SO_21_HeaderRecords.MAGICT " +
                                                ", MK_10_VehicleRecords.MODEL " +
                                                ", MK_10_VehicleRecords.MODELYR " +
                                                ", MK_10_CustomerRecords.EMAIL " +
                                                ", MK_10_VehicleRecords.VARIANT " +
                                                ", MK_10_VehicleRecords.DESCLETT " +
                                                ", MK_10_VehicleRecords.SALEDATE " +
                                                "FROM SO_21_HeaderRecords SO_21_HeaderRecords, GB_00_UserDetails GB_00_UserDetails, MK_10_VehicleRecords MK_10_VehicleRecords, MK_10_CustomerRecords MK_10_CustomerRecords " +
                                                "where GB_00_UserDetails.POSTCODE = SO_21_HeaderRecords.CUSTCONT  " +
                                                "and MK_10_VehicleRecords.MAGIC = SO_21_HeaderRecords.MAGICV  " +
                                                "and MK_10_CustomerRecords.MAGIC = SO_21_HeaderRecords.MAGICT and " +
                                                "((SO_21_HeaderRecords.CHASSIS Like '%" + str + "%') " +
                                                "or (SO_21_HeaderRecords.REGNO Like '%" + str + "%') " +
                                                "or (lower(SO_21_HeaderRecords.CUSTNAME) Like '%" + str + "%') " +
                                                "or (SO_21_HeaderRecords.DEPT Like '%" + str + "%') " +
                                                "or (SO_21_HeaderRecords.MAGICT = " + str + ") " +
                                                "or (SO_21_HeaderRecords.WIPNO = " + str + ") " +
                                                "or (SO_21_HeaderRecords.PHONE Like '%" + str + "%')) ";

                getData.sSqlString = getData.sSqlString + " ORDER BY SO_21_HeaderRecords.CREATED DESC";
                getData.sCompanyName = "ServiceODBC";

                var result1 = client.GetDataAsync(getData).Result;

                //// Create a new list to store the search results

                foreach (string strresult in result1.GetDataResult)
                {
                    string[] tmpresult = strresult.Split(",");
                    if (tmpresult.Length > 2)
                    {
                       
                        SearchVehicle tmplist = new SearchVehicle();
                        tmplist.RepairOrder = tmpresult[0];
                        tmplist.Name = tmpresult[3];
                        tmplist.Licence = tmpresult[2];
                        tmplist.CustomerTel = tmpresult[6];
                        tmplist.Vin = tmpresult[4];
                        tmplist.RegDate = tmpresult[1];
                        tmplist.ServiceAdviser = tmpresult[5];
                        tmplist.CustomerCode = tmpresult[7].ToString();
                        //tmplist.healthCheckId = (int.Parse(tmpresult[7]) ==0 || int.Parse(tmpresult[7]) == null) ? int.Parse(tmpresult[7]) : 0;
                        tmplist.PurshYear = tmpresult[9];
                        tmplist.VehModel = tmpresult[8];
                        tmplist.YearModel = tmpresult[8].ToString() + " - " + tmpresult[9].ToString();
                        tmplist.Email = tmpresult[10].ToString();
                        tmplist.Variant = tmpresult[11].ToString();
                        tmplist.VehDescription = tmpresult[12].ToString();
                        tmplist.VehDeliveryDate = tmpresult[13].ToString();
                        searchResults.Add(tmplist);
                    }
                }
                return View("_SearchResults", searchResults);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Error occurred: "+ex.Message); 
            }





}

        [HttpPost]
        public async Task<IActionResult> Add(AddVehicleServiceRequest addVehicleServiceRequest)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Rates";
            ViewData["navItem3"] = "New Service Rate";
            var vehService = new VehicleService
            {
                Name = addVehicleServiceRequest.Name,
                Cost = addVehicleServiceRequest.Cost,
                AddedOn = DateTime.Now,
                LastUpdate = DateTime.Now,
                VehicleId = addVehicleServiceRequest.VehicleId,
                MileageId = addVehicleServiceRequest.MileageId,
                BrandId = 1,
                VehicleTypeId = addVehicleServiceRequest.VehicleTypeId,
            };
            await vehicleServiceRepository.AddVehicleServiceAsync(vehService);

            return RedirectToAction("List");
        }

        /**
         * 
         * List of Services 
         * 
         */
        [HttpGet]
        public async Task<IActionResult> List()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Rates";
            ViewData["navItem3"] = "List";

            var services = await vehicleServiceRepository.GetVehicleServicesAsync();
            var vehicleViewModels = await MapVehiclesToViewModelsAsync(services);
            return View(vehicleViewModels);

        }
        private async Task<List<VehicleServiceViewModel>> MapVehiclesToViewModelsAsync(IEnumerable<VehicleService> vehicleServices)
        {
            var vehicleServiceViewModels = new List<VehicleServiceViewModel>();

            foreach (var service in vehicleServices)
            {
                var brand = await brandRepository.GetBrandByIdAsync(service.BrandId);
                var mileage = await mileageRepository.GetMileageByIdAsync(service.MileageId);
                var vehicle = await vehicleRepository.GetVehicleByIdAsync(service.VehicleId);
                var vehicleType = await vehicleTypeRepository.GetVehicleTypeByIdAsync(service.VehicleTypeId);
                var viewModel = new VehicleServiceViewModel
                {
                    Id = service.Id,
                    Name = service.Name,
                    Cost = service.Cost,
                    BrandId = service.BrandId,
                    VehicleId = service.VehicleId,
                    MileageId = service.MileageId,
                    VehicleTypeId = service.VehicleTypeId,
                    BrandName = brand?.Name, 
                    VehicleName = vehicle?.Name, 
                    MileageValue = mileage?.MileageValue.ToString(),
                    VehicleTypeName = vehicleType?.Name
                };
                vehicleServiceViewModels.Add(viewModel);
            }

            return vehicleServiceViewModels;
        }


        /**
         * 
         * Get data of service by Id
         * 
         */

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Rates";
            ViewData["navItem3"] = "Edit Service Rate";

            var vehservice = await vehicleServiceRepository.GetVehicleServiceByIdAsync(id);
            if (vehservice != null)
            {
                // Get all brands from the repository
                var brands = await brandRepository.GetAllBrandsAsync();
                var brandList = brands.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
                // Get all vehicles from the repository
                var vehicles = await vehicleRepository.GetAllVehiclesAsync();
                var vehicleList = vehicles.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
                // Get all mileages from the repository
                var mileages = await mileageRepository.GetAllMileagesAsync();
                var mileageList = mileages.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();

                // Find the selected brand name based on the service's BrandId
                var selectedBrand = await brandRepository.GetBrandByIdAsync(vehservice.BrandId);
                var selectedBrandName = selectedBrand?.Name;
                // Find the selected vehicle name based on the service's VehicleId
                var selectedVehicle = await vehicleRepository.GetVehicleByIdAsync(vehservice.VehicleId);
                var selectedVehicleName = selectedVehicle?.Name;
                // Find the selected mileage name based on the service's MileageId
                var selectedMileage = await mileageRepository.GetMileageByIdAsync(vehservice.MileageId);
                var selectedMileageName = selectedMileage?.Name;

                // Get all brands from the repository
                var vehicleTypess = await vehicleTypeRepository.GetVehicleTypesAsync();
                var vehicleTypeList = vehicleTypess.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();

                var editServiceRequest = new EditVehicleServiceRequest
                {
                    Name = vehservice.Name,
                    Cost = vehservice.Cost,
                    AddedOn = vehservice.AddedOn,
                    LastUpdate = DateTime.Now,
                    VehicleId = vehservice.VehicleId,
                    Vehicles = vehicleList,
                    BrandId = vehservice.BrandId,
                    VehicleTypeId = vehservice.VehicleTypeId,
                    Brands = brandList,
                    MileageId = vehservice.MileageId,
                    Mileages = mileageList,
                    VehicleTypes = vehicleTypeList
                };
                return View(editServiceRequest);
            }
            else
            {
                TempData["AlertMessage"] = "Error occurred. Unable to find the requested Service Reate.";
                return RedirectToAction("List");
            }
        }

        /**
         * 
         * Update changed service 
         * 
         */

        [HttpPost]
        public async Task<IActionResult> Edit(EditVehicleServiceRequest editVehicleServiceRequest)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Rates";
            ViewData["navItem3"] = "Edit Service Rate";
            var vehicleService = new VehicleService
            {
                Id = editVehicleServiceRequest.Id,
                Name = editVehicleServiceRequest.Name,
                Cost = editVehicleServiceRequest.Cost,
                LastUpdate = DateTime.Now,
            };
            var updatedService = await vehicleServiceRepository.UpdateVehicleServiceAsync(vehicleService);
            if (updatedService != null)
            {
                TempData["SuccessMessage"] = "Service Rate updated successfully";
                return RedirectToAction("list");
            }
            else
            {
                TempData["AlertMessage"] = "Error occurred. Service rate not updated";
                return RedirectToAction("Edit", new { id = editVehicleServiceRequest.Id });
            }


        }

        /**
         * 
         * Delete Service 
         * 
         */

        public async Task<IActionResult> Delete(int id)
        {
            var serviceToDelete = await vehicleServiceRepository.GetVehicleServiceByIdAsync(id);
            if (serviceToDelete != null)
            {
                var deletedService = await vehicleServiceRepository.DeleteVehicleServiceAsync(id);
                if (deletedService != null)
                {
                    //dispaly  success message  -- service deleted successfully
                    return RedirectToAction("List");
                }
                else
                {
                    //display error message -- service not deleted
                    return RedirectToAction("List");
                }

            }
            else
            {
                //display error message -- no service found to delete
                return RedirectToAction("List");
            }
        }

        /**
         * Seach for Vehicle Services 
         * 
         */
        [HttpGet]
        public async Task<IActionResult> Search(int CustCode, string CustName, string VehModel, string VehVariant, string VehDescription, string VehDelivDate, string PurYear, string VehVin, string VehReg, string CustTel, string CustEmail)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Quotations";
            ViewData["navItem3"] = "New Quotation";
            //Get Vehicles from Repository
            var vehicles = await vehicleRepository.GetAllVehiclesAsync();
            var vehicleList = vehicles.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.Cars = vehicleList;
            //Get Vehicle Types from Repository
            var vehicleTypes = await vehicleTypeRepository.GetVehicleTypesAsync();
            var vehicleTypeList = vehicleTypes.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.VehicleTypes = vehicleTypeList;
            //Get Mileages from Repository
            var mileages = await mileageRepository.GetAllMileagesAsync();
            var mileageList = mileages.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.Mileages = mileageList;
            ViewBag.CustCode = CustCode;
            ViewBag.CustName = CustName;
            ViewBag.VehModel = VehModel;
            ViewBag.VehVariant = VehVariant;
            ViewBag.VehDescription = VehDescription;
            ViewBag.VehDelivDate = VehDelivDate;
            ViewBag.PurYear = PurYear;
            ViewBag.VehReg = VehReg;
            ViewBag.VehVin = VehVin;
            ViewBag.CustTel = CustTel;
            ViewBag.CustEmail = CustEmail;
            string normalizedName = VehModel.ToUpperInvariant(); // Or ToLowerInvariant()
            var vehicle = await vehicleRepository.GetVehicleByNameAsync(normalizedName);
            //var vehicle = vehicleRepository.GetVehicleByNameAsync(VehModel);
            if (vehicle!= null)
            {
                ViewBag.CarId = vehicle.Id;
            } else
            {
                ViewBag.CarId = 0;
            }
           

            var submitStatus = false;
            ViewBag.SubmitStatus = submitStatus;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Search(string CustCode, string CustName, string CustTel, string CustEmail, string PurYear, string VehReg, string VehVin, string VehicleVariant, string VehDescription, string VehDelivDate, ServiceContractViewModel searchViewModel)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Service Rates";
            ViewData["navItem3"] = "Search";
            var searchResults = await vehicleServiceRepository.SearchServices(
                    searchViewModel.VehicleId,
                    searchViewModel.StartingMileageId,   
                    searchViewModel.EndingMileageId
                 );
            var vehicleParts = await geniumPartRepository.VehicleParts(
                    searchViewModel.VehicleId,
                    searchViewModel.VehicleTypeId
                 );

            //string VehName = await GetVehicleName(searchViewModel.VehicleId);
            string VehName = (await vehicleRepository.GetVehicleByIdAsync(searchViewModel.VehicleId))?.Name;
            ViewBag.VehicleName = VehName;
            ViewBag.VehicleId = searchViewModel.VehicleId;
            string typeName = await GetVehicleTypeName(searchViewModel.VehicleTypeId);
            ViewBag.TypeId = searchViewModel.VehicleTypeId;
            ViewBag.VehicleTypeName = typeName;
            string StMil = await GetMileageIndex(searchViewModel.StartingMileageId);
            
            ViewBag.StartingMileageValue = StMil;
            string EndMil = await GetMileageIndex(searchViewModel.EndingMileageId);
            ViewBag.EndingMileageValue = EndMil;
            // Calculate the number of KM that will be served
            int stMilValue = await GetMileageValue(searchViewModel.StartingMileageId);
            int endMilValue = await GetMileageValue(searchViewModel.EndingMileageId);
            int servedMileage = endMilValue - stMilValue;
            int nbMileages = await mileageRepository.GetMileageCountAsync(searchViewModel.StartingMileageId, searchViewModel.EndingMileageId);
            int servedYears = GetServedYears(nbMileages);
            ViewBag.BegMileage = stMilValue;
            ViewBag.EdMileage = endMilValue;
            ViewBag.ServedMileage = servedMileage;
            // Other Vehicle Details 
            ViewBag.VehVariant = VehicleVariant;
            ViewBag.VehDescription = VehDescription;
            ViewBag.VehDeliveryDate = VehDelivDate;
            ViewBag.VehVin = VehVin;
            ViewBag.VehReg = VehReg;
            ViewBag.PurchaseYear = PurYear;
            //Customer Details
            ViewBag.CustCode = CustCode;
            ViewBag.CustName = CustName;
            ViewBag.CustPhone = CustTel;
            ViewBag.CustEmail = CustEmail;
            // Number of service years
            ViewBag.ServedYears = servedYears;

            //Get the count of service contracts
            var nbContracts = await serviceContractRepository.GetContractCountAsync();
            ViewBag.ContractCount = "SC-" + DateTime.Now.Year + "-" + (nbContracts + 1).ToString("D4");

            // Get all brands from the repository
            var vehicleTypes = await vehicleTypeRepository.GetVehicleTypesAsync();
            var vehicleTypeList = vehicleTypes.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            ViewBag.VehicleTypes = vehicleTypeList;

            //Get Customers from Repository
            var customers = await customerRepository.GetAllCustomersAsync();
            var customerList = customers.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.CustomerName }).ToList();
            ViewBag.Customers = customerList;
            // Service Package Caption 
            ViewBag.PackageCaption = $"Service Package From {StMil} To {EndMil} ";
            // Current Date 
            ViewBag.CurrentDate = DateTime.Now.Date;
            //Expiry Date 
            DateTime expiryDate = DateTime.Now.AddYears(servedYears);
            ViewBag.ExpiryDate = expiryDate;

            searchViewModel.SearchResults = searchResults.ToList();
            searchViewModel.VehiclePartsResults = vehicleParts.ToList();
            var submitStatus = true;
            ViewBag.SubmitStatus = submitStatus;



           



            return View(searchViewModel);
        }
        private async Task<string> GetVehicleName(int vehicleId)
        {
            var vehservice = await vehicleServiceRepository.GetVehicleServiceByIdAsync(vehicleId);
            if(vehservice != null)
            {
                var selectedVehicle = await vehicleRepository.GetVehicleByIdAsync(vehservice.VehicleId);
                var selectedVehicleName = selectedVehicle?.Name;
                return selectedVehicleName; // Replace with the actual vehicle name
            } else
            {
                return null;
            }
            
        }

        private async Task<string> GetVehicleTypeName(int vehicleTypeId)
        {
            //var vehservice = await vehicleServiceRepository.GetVehicleServiceByIdAsync(vehicleId);
            var selectedVehicleType = await vehicleTypeRepository.GetVehicleTypeByIdAsync(vehicleTypeId);
            var selectedVehicleTypeName = selectedVehicleType?.Name;
            return selectedVehicleTypeName; // Replace with the actual vehicle name
        }

        private async Task<string> GetMileageIndex(int mileageId)
        {
            //var vehservice = await vehicleServiceRepository.GetVehicleServiceByIdAsync(vehicleId);
            var selectedMileage = await mileageRepository.GetMileageByIdAsync(mileageId);
            var selectedMileageValue = selectedMileage?.Name;
            return selectedMileageValue; // Replace with the actual vehicle name
        }

        private async Task<int> GetMileageValue(int mileageId)
        {
           
            var selectedMileage = await mileageRepository.GetMileageByIdAsync(mileageId);
            var selectedMileageValue = selectedMileage?.MileageValue ?? 0;
            return selectedMileageValue; 
        }

        private int GetServedYears(int nbMileages)
        {
            if (nbMileages > 0 && nbMileages <= 2)
            {
                return 1; // 1 year
            }
            else if (nbMileages > 2 && nbMileages <= 4)
            {
                return 2; // 2 years
            }
            else if (nbMileages > 4 && nbMileages <= 6)
            {
                return 3; // 3 years
            }
            else if (nbMileages > 6 && nbMileages <= 8)
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
