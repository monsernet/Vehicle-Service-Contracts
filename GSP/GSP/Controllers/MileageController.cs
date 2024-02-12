using GSP.Models.Domain;
using GSP.Models.ViewModels;
using GSP.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GSP.Controllers
{
    public class MileageController : Controller
    {
        private readonly IMileageRepository mileageRepository;

        public MileageController(IMileageRepository mileageRepository)
        {
            this.mileageRepository = mileageRepository;
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Mileages";
            ViewData["navItem3"] = "New Mileage";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMileageRequest mileageAddRequest ) 
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Mileages";
            ViewData["navItem3"] = "New Mileage";
            var mileage = new Mileage
            {
                Name = mileageAddRequest.Name,
                MileageValue = mileageAddRequest.MileageValue
            };
           await  mileageRepository.AddMileageAsync(mileage);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Mileages";
            ViewData["navItem3"] = "List";
            var mileages = await mileageRepository.GetAllMileagesAsync();
            return View(mileages);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Mileages";
            ViewData["navItem3"] = "Edit Mileage";
            var mileage = await mileageRepository.GetMileageByIdAsync(id);
            if(mileage != null)
            {
                var editMileage = new EditMileageRequest
                {
                    Id = mileage.Id,
                    Name = mileage.Name,
                    MileageValue = mileage.MileageValue
                };
                return View(editMileage);
            } else
            {
                // display error message -- Mileage not found 
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit (EditMileageRequest editMileageRequest)
        {
            ViewData["navItem1"] = "Home";
            ViewData["navItem2"] = "Mileages";
            ViewData["navItem3"] = "Edit Mileage";
            var editMileage = await mileageRepository.GetMileageByIdAsync (editMileageRequest.Id);
            if(editMileage != null)
            {
                var mileage = new Mileage
                {
                    Id = editMileage.Id,
                    Name = editMileage.Name,
                    MileageValue = editMileage.MileageValue
                };
                var updatedMileage = await mileageRepository.UpdateMileageAsync(mileage);
                if(updatedMileage != null)
                {
                    //display success message -- mileage updated successfully
                    return RedirectToAction("List");
                } else
                {
                    //display error message -- mileage not updated
                    return RedirectToAction("Edit", new { id = editMileage.Id });
                }
                
                
            } else
            {
                //display error message -- Mileage nort exist 
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete (int id)
        {
            var mileageToDelete = await mileageRepository.GetMileageByIdAsync(id);
            if(mileageToDelete != null)
            {
                var deletedMileage = await mileageRepository.DeleteMileageAsync(id);
                if(deletedMileage != null)
                {
                    // display success message -- Mileage deleted 
                } else
                {
                    //display error message -- Mileage not deleted 
                }
                return RedirectToAction("list");
            } else
            {
                // display error message -- mileage not exist 
                return RedirectToAction("list");
            }
        }

        
    }
}
