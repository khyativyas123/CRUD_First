using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD_First.Models;
using NuGet.Protocol.Plugins;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CRUD_First.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly KhyatiVyasContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        //hobbies
            List<SelectListItem> hobbiesList = new List<SelectListItem>()
        {
            new SelectListItem(){ Text="Singing", Value="1" },
            new SelectListItem(){ Text="Reading", Value="2" },
            new SelectListItem(){ Text="Surfing", Value="3" }


        };
        

        public EmployeesController(KhyatiVyasContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            ViewBag.itemhobbies = hobbiesList;
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewBag.itemhobbies = hobbiesList;
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Gender,MaritalStatus,BirthDate,Multihobbies,ImageFile,Salary,Address,CountrySelected,StateSelected,CitySelected,ZipCode,Password,Created")] Employee employee)
        {
            string hby = "";
            if (employee.Multihobbies != null)
            {
                var selectedItem = hobbiesList.Where(x => employee.Multihobbies.Contains(x.Value)).ToList();
                foreach (var item in selectedItem)
                {
                    item.Selected = true;
                    hby += item.Text + ", ";
                }
            }
            employee.Hobbies = hby;

            if (ModelState.IsValid)
            {
                

                if (employee.CountrySelected != null)
                {
                    var selected_country = _context.Countries.Where(x => employee.CountrySelected.Contains(x.CountryId.ToString())).First();
                    employee.Country = Convert.ToInt32(selected_country.CountryId);
                }
                if (employee.StateSelected != null)
                {
                    var selected_state = _context.States.Where(x => employee.StateSelected.Contains(x.StateId.ToString())).First();
                    employee.State = Convert.ToInt32(selected_state.StateId);
                }
                if (employee.CitySelected != null)
                {
                    var selected_city = _context.Cities.Where(x => employee.CitySelected.Contains(x.CityId.ToString())).First();
                    employee.City = Convert.ToInt32(selected_city.CityId);
                }


                //Image store into wwwroot/image
                if (employee.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                    string extension = Path.GetExtension(employee.ImageFile.FileName);
                    string storeImageFile = fileName + DateTime.Now.ToString("ddMMyyyy hhmmss") + extension;
                    employee.Photos = storeImageFile;
                    string path = Path.Combine(wwwRootPath + "/Image/", storeImageFile);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await employee.ImageFile.CopyToAsync(fileStream);
                    }
                }

                employee.Created = DateTime.Now;
                //Insert data
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
               
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            
            //Hobbies
            ViewBag.itemhobbies = hobbiesList;
            List<string> hobbyidList =new List<string>();
            if(employee.Hobbies.ToString() != null)
            {
                string s = employee.Hobbies;
                string[] hobbyvalues = s.Split(',');

                var selectedItem = hobbiesList.Where(x => employee.Hobbies.Contains(x.Text)).ToList();
                foreach (var item in selectedItem)
                {
                    item.Selected = true;
                    //hby += item.Text + ", ";
                    hobbyidList.Add(item.Value);
                }
            }
            employee.Multihobbies = hobbyidList;

            //Image
            if(employee.Photos != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image",employee.Photos);
                
                if (System.IO.File.Exists(imagePath))
                {
                    // var bytes=System.IO.File.ReadAllBytes(imagePath);
                    var stream = System.IO.File.OpenRead(imagePath);
                    employee.ImageFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                    //employee.ImageFile = imagePath;
                }
            }

            //Country
            if(employee.Country != null)
            {
                var selected_country = _context.Countries.Where(x=>x.CountryId == employee.Country).First();
                employee.CountrySelected = selected_country.CountryId.ToString();
            }
            if (employee.State != null)
            {
                var selected_state = _context.States.Where(x => x.StateId == employee.State).First();
                employee.StateSelected = selected_state.StateId.ToString();
            }
            if (employee.Country != null)
            {
                var selected_city = _context.Cities.Where(x => x.CityId == employee.City).First();
                employee.CitySelected = selected_city.CityId.ToString();
            }

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Gender,MaritalStatus,BirthDate,Multihobbies,ImageFile,Salary,Address,CountrySelected,StateSelected,CitySelected,ZipCode,Password,Created")] Employee employee)
         {
            if (id != employee.Id)
            {
                return NotFound();
            }

                try
                {
                    string hby = "";
                    if (employee.Multihobbies != null)
                    {
                        var selectedItem = hobbiesList.Where(x => employee.Multihobbies.Contains(x.Value)).ToList();
                        foreach (var item in selectedItem)
                        {
                            item.Selected = true;
                            hby += item.Text + ", ";
                        }
                    }
                    employee.Hobbies = hby;

                    if (employee.CountrySelected != null)
                    {
                        var selected_country = _context.Countries.Where(x => employee.CountrySelected.Contains(x.CountryId.ToString())).First();
                        employee.Country = Convert.ToInt32(selected_country.CountryId);
                    }
                    if (employee.StateSelected != null)
                    {
                        var selected_state = _context.States.Where(x => employee.StateSelected.Contains(x.StateId.ToString())).First();
                        employee.State = Convert.ToInt32(selected_state.StateId);
                    }
                    if (employee.CitySelected != null)
                    {
                        var selected_city = _context.Cities.Where(x => employee.CitySelected.Contains(x.CityId.ToString())).First();
                        employee.City = Convert.ToInt32(selected_city.CityId);
                    }


                    //Image store into wwwroot/image
                    if (employee.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                        string extension = Path.GetExtension(employee.ImageFile.FileName);
                        string storeImageFile = fileName + DateTime.Now.ToString("ddMMyyyy hhmmss") + extension;
                        employee.Photos = storeImageFile;
                        string path = Path.Combine(wwwRootPath + "/Image/", storeImageFile);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await employee.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'KhyatiVyasContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return _context.Employees.Any(e => e.Id == id);
        }

        [HttpGet]
        public JsonResult GetCountries()
        {
            var countries = _context.Countries.OrderBy(x => x.CountryName).ToList();
            return new JsonResult(countries);
        }
        [HttpGet]
        public JsonResult GetCountriesById(int CountryID)
        {
            var countries = _context.Countries.Where(x=>x.CountryId == CountryID).OrderBy(x => x.CountryName).ToList();
            return new JsonResult(countries);
        }
        public JsonResult GetStates(int CountryId)
        {
            var states = _context.States.Where(x=>x.CountryId == CountryId).OrderBy(x => x.StateName).ToList();
            return new JsonResult(states);
        }
        public JsonResult GetStatesById(int id)
        {
            var states = _context.States.Where(x => x.StateId == id).OrderBy(x => x.StateName).ToList();
            return new JsonResult(states);
        }
        public JsonResult GetCities(int StateId)
        {
            var cities = _context.Cities.Where(x => x.StateId == StateId).OrderBy(x => x.CityName).ToList();
            return new JsonResult(cities);
        }
        public JsonResult GetCitiesById(int CityID)
        {
            var cities = _context.Cities.Where(x => x.CityId == CityID).OrderBy(x => x.CityName).ToList();
            return new JsonResult(cities);
        }
    }
}
