using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcCrudeApplication.Data;
using mvcCrudeApplication.Models;
using mvcCrudeApplication.Models.Domain;

namespace mvcCrudeApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MvcDemoTestDbContext mvcDemoTestDbContext;

        public EmployeeController(MvcDemoTestDbContext mvcDemoTestDbContext) 
        {
            this.mvcDemoTestDbContext = mvcDemoTestDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            var employees = await mvcDemoTestDbContext.Employees.ToListAsync();

            return View(employees);
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeViewModel addEmployeeReq)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeReq.Name,
                Email = addEmployeeReq.Email,
                Salary = addEmployeeReq.Salary,
                BirthDate = addEmployeeReq.BirthDate,
                Dept = addEmployeeReq.Dept,
            };
            await mvcDemoTestDbContext.Employees.AddAsync(employee);
            await mvcDemoTestDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id) 
        {
            var employee = await mvcDemoTestDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if(employee != null)
            {
                var viewModel = new UpdateEmployee()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    BirthDate = employee.BirthDate,
                    Dept = employee.Dept,
                };
                return await Task.Run(() => View("View", viewModel));
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployee update)
        {
            var employee = await mvcDemoTestDbContext.Employees.FindAsync(update.Id);

            if(employee != null)
            {
                employee.Name = update.Name;
                employee.Email = update.Email;
                employee.Salary = update.Salary;
                employee.BirthDate = update.BirthDate;
                employee.Dept = update.Dept;

                await mvcDemoTestDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployee delete)
        {
            var employee = await mvcDemoTestDbContext.Employees.FindAsync(delete.Id);
            
            if(employee != null)
            {
                mvcDemoTestDbContext.Employees.Remove(employee);
                await mvcDemoTestDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
