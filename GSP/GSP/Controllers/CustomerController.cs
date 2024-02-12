using GSP.DTO;
using GSP.Models.Domain;
using GSP.Models.ViewModels;
using GSP.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GSP.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Customers";
            ViewData["navItem3"] = "New Customer";
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Add(AddCustomerRequest addCustomerRequest)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Customers";
            ViewData["navItem3"] = "New Customer";
            var customer = new Customer
            {
                CustomerName = addCustomerRequest.CustomerName,
                CustomerCode = addCustomerRequest.CustomerCode,
                CivilId = addCustomerRequest.CivilId,
                Phone = addCustomerRequest.Phone,
                Phone2 = addCustomerRequest.Phone2,
                Phone3 = addCustomerRequest.Phone3,
                Phone4 = addCustomerRequest.Phone4,
                Address = addCustomerRequest.Address,
                Email = addCustomerRequest.Email
            };
            var cust = await customerRepository.AddCustomerAsync(customer);
            if(cust != null)
            {
                TempData["SuccessMessage"] = "Customer added successfully";
            } else
            {
                TempData["ErrorMessage"] = "An Error occurred. Customer not added.";
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Brands";
            ViewData["navItem3"] = "Edit Customer";
            // get brand based on selected id
            var customer = await customerRepository.GetCustomerByIdAsync(id);
            if (customer != null)
            {
                var editCustomerRequest = new EditCustomerRequest
                {
                    Id = customer.Id,
                    CustomerCode = customer.CustomerCode,
                    CustomerName = customer.CustomerName,
                    Phone = customer.Phone,
                    Phone2 = customer.Phone2,
                    Phone3 = customer.Phone3,
                    Phone4 = customer.Phone4,
                    Email = customer.Email,
                    CivilId = customer.CivilId,
                    Address = customer.Address,
                };
                return View(editCustomerRequest);
            }
            else
            {
                TempData["ErrorMessage"] = "Error occurred. No Customer found";
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCustomerRequest editCustomerRequest)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Customers";
            ViewData["navItem3"] = "Edit Customer";
            var customer = new Customer
            {
                Id = editCustomerRequest.Id,
                CustomerName = editCustomerRequest.CustomerName,
                CustomerCode = editCustomerRequest.CustomerCode,
                Phone = editCustomerRequest.Phone,
                Phone2 = editCustomerRequest.Phone2,
                Phone3 = editCustomerRequest.Phone3,
                Phone4 = editCustomerRequest.Phone4,
                Email = editCustomerRequest.Email,
                CivilId = editCustomerRequest.CivilId,
                Address= editCustomerRequest.Address

                
            };
            var updatedCustomer =  await customerRepository.UpdateCustomerAsync(customer);
            if (updatedCustomer != null) 
            {
                TempData["SuccessMessage"] = "Customer updated successfully";
                return RedirectToAction("List");
            } else
            {
                TempData["ErrorMessage"] = "Error occurred. Customer not updated.";
                return RedirectToAction("Edit", new { id = editCustomerRequest.Id });
            }
        }

        public async Task<IActionResult> List()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Customers";
            ViewData["navItem3"] = "List";
            var customers = await customerRepository.GetAllCustomersAsync();
            return View(customers);
        }

        
        public async Task<ActionResult> Delete(int id) 
        {
            var customerToDelete = await customerRepository.DeleteCustomerAsync(id);
            if (customerToDelete != null)
            {
                TempData["SuccessMessage"] = "Customer deleted successfully.";
            } else
            {
                TempData["ErrorMessage"] = "Error Occurred. Customer not deleted.";
            }
            return RedirectToAction("List");
        }

        //Get the customer details 
        public async Task<IActionResult> GetCustomerDetails(int customerId)
        {
            var customerDetails = await customerRepository.GetCustomerByIdAsync(customerId);
            return Json(customerDetails);
        }


        //UPLOAD BULK CUSTOMERS

        [HttpGet]
        public async Task<IActionResult> UploadExcel()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Customers";
            ViewData["navItem3"] = "Add Bulk Customers";
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                // Handle the case when no file is selected
                ModelState.AddModelError("file", "Please select a file");
                return View();
            }

            try
            {
                var excelData = await ReadExcelData(file.OpenReadStream());
                await customerRepository.AddBulkCustomersAsync(excelData);

                // Optionally, provide feedback to the user
                TempData["SuccessMessage"] = " Customers uploaded successfully";
                return RedirectToAction("List");

            }
            catch (Exception ex)
            {
                // Handle exceptions, log, and provide feedback to the user
                TempData["ErrorMessage"] = $"  An error occurred: {ex.Message}";
                return RedirectToAction("UploadExcel");
            }


        }

        private async Task<IEnumerable<CustomerExcelDto>> ReadExcelData(Stream stream)
        {
            List<CustomerExcelDto> excelData = new List<CustomerExcelDto>();

            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                //using (var fileStream = new FileStream(memoryStream, FileMode.Open, FileAccess.Read))
                using (var fileStream = new MemoryStream(memoryStream.ToArray()))
                {
                    IWorkbook workbook = new XSSFWorkbook(fileStream);
                    ISheet sheet = workbook.GetSheetAt(0); //  data here begins from the first sheet

                    for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++) // Start from 1 to skip the header row
                    {
                        IRow row = sheet.GetRow(rowIdx);
                        if (row == null) continue;

                        CustomerExcelDto dto = new()
                        {
                            CustomerCode = row.GetCell(0)?.ToString() ?? "NA",
                            CustomerName = row.GetCell(1)?.ToString() ?? "NA",
                            Phone = row.GetCell(2)?.ToString() ?? "0",
                            Phone2 = row.GetCell(3)?.ToString() ?? "0",
                            Phone3 = row.GetCell(4)?.ToString() ?? "0",
                            Phone4 = row.GetCell(5)?.ToString() ?? "0",
                            Email = row.GetCell(6)?.ToString() ?? "",
                            CivilId = row.GetCell(7)?.ToString() ?? "0",
                            Address = row.GetCell(8)?.ToString() ?? ""

                        };

                        excelData.Add(dto);
                    }
                }
            }

            return excelData;
        }


    }
}
