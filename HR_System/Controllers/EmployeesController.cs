using HR_System.Data;
using HR_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HR_System.Controllers
{
    public class EmployeesController : Controller
    {
        public ApplicationDbContxt _context;
        public EmployeesController(ApplicationDbContxt context)
        {
            _context = context;
        }

       

        public IActionResult Index()
        {
            var result = _context.Employees.Include(x=>x.Department)
                .OrderBy(x=>x.EmployeeName).ToList();
            return View(result);
        }

        public IActionResult Create()
        {
            ViewBag.Departments= _context.Departments.OrderBy(x=>x.DepartmentName).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee model)
        {
            //Upload Image:
            UploadImage(model);
            if (ModelState.IsValid)
            {
                _context.Employees.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = _context.Departments.OrderBy(x => x.DepartmentName).ToList();
            return View();
        }


        public IActionResult Edit(int? Id)
        {
            ViewBag.Departments = _context.Departments.OrderBy(x => x.DepartmentName).ToList();
            var result = _context.Employees.Find(Id);
            return View("Create",result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee model)
        {
            UploadImage(model);
            if(ModelState.IsValid)
            {
                _context.Employees.Update(model);  
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments = _context.Departments.OrderBy(x => x.DepartmentName).ToList();
            return View(model);
        }

        public IActionResult Delete(int? Id)
        {
            var result = _context.Employees.Find(Id);
            if(result !=null)
            {
                _context.Employees.Remove(result);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        private void UploadImage(Employee model)
        {
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                var fileStream = new FileStream(Path.Combine(@"wwwroot/Images", imageName), FileMode.Create);
                file[0].CopyTo(fileStream);
                model.UserImage = imageName;
            }
            else if (model.UserImage == null && model.EmployeeId == null)
            {
                model.UserImage = "DefaultImage.png";
            }
            else
            {
                model.UserImage = model.UserImage;
            }
        }

    }


}
