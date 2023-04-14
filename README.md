# asp.net-core-mvc-6-crude-application


*************   CRUD Operation  ***************************

1.	creae project 'asp.net web model view controller'

2.	install entity framework
			- Microsoft.EntityFrameworkCore.SqlServer
			- Microsoft.EntityFrameworkCore.Tools
		
3.	create dbContext class 
			- create folder Data
			- create 'McvTestDbContext.cs' class
			- inherite it from 'DbContext' class

4.	Write constructor of dbContext class
			- click on 'McvTestDbContext' then press 'ctrl+.'
			- choose constructor with option 
			
5.	create property inside dbContext
			- 'public DbSet<Employee> Employees { get; set; }'
			- write 'prop' and double press space-bar

6.	create Employee class with property
			- create folder 'Domain' inside 'Model'
			- create 'Empployee.cs' class inside 'Domain'
			- write all property for Emplyee
					public Guid Id { get; set; }
					public string Name { get; set; }
					public string Email { get; set; }
					public int Salary { get; set; }
					public DateTime BirthDate { get; set;}
					public string Dept { get; set;}
					
7.	write connection string inside AppSetting.json
			-      "ConnectionStrings": {
							"MvcDemoConnectionString": "server=GHOST;database=MvcDemoDatabase;Trusted_connection=true;TrustServerCertificate=True"
					}

				
8.	Inject dbContext inside 'Program.cs'
			- builder.Services.AddDbContext<MvcDemoTestDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MvcDemoConnectionString")));
				
9.	run entity framework core migration
			- click on 'Tool' - click on 'NuGet package manager' - click on 'package manager console'
			- run command 'Add-Migration "Initial Migration"'
			- run command 'Update-Database'
			
10.	create new controller
			- new controller inside controller 'EmployeeController'
			
11.	create new action method to add employee 
			-	[HttpGet]
				public IActionResult Add()
				{
					return View();
				}	
			- click on view and add new view as 'Razor View-Empty'

12.	add new link in '_Layout.cshtml'
			-	<li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-controller="Employee" asp-action="AddEmployee">Add Employee</a>
                </li>
				
13.	create form in 'AddEmployee.cshtml'
			- use boostrap directly
			
14.	create class 'AddEmployeeViewModel.cs' in model
			-	public string Name { get; set; }
				public string Email { get; set; }
				public int Salary { get; set; }
				public DateTime BirthDate { get; set; }
				public string Dept { get; set; }
				
15. mapped this bootstrap with form with newly created model class
			-	@model mvcCrudeApplication.Models.AddEmployeeViewModel
				@{
				}
				<form method="post" action="AddEmployee">
					<div class="mb-3">
						<label for="" class="form-label">Name</label>
						<input type="text" class="form-control" asp-for="Name">    
					</div>
					<div class="mb-3">
						<label for="" class="form-label">Email</label>
						<input type="email" class="form-control" asp-for="Email">
					</div>
					<div class="mb-3">
						<label for="" class="form-label">Salary</label>
						<input type="number" class="form-control" asp-for="Salary">
					</div>
					<div class="mb-3">
						<label for="" class="form-label">Date of Birth</label>
						<input type="date" class="form-control" asp-for="BirthDate">
					</div>
					<div class="mb-3">
						<label for="" class="form-label">Department</label>
						<input type="text" class="form-control" asp-for="Dept">
					</div>
				   
					<button type="submit" class="btn btn-primary">Submit</button>
				</form>
				
16.	create 'EmployeeController' constructor and add instance of dbContext class
			-	private readonly MvcDemoTestDbContext mvcDemoTestDbContext;
				public EmployeeController(MvcDemoTestDbContext mvcDemoTestDbContext) 
				{
					this.mvcDemoTestDbContext = mvcDemoTestDbContext;
				}
				
17.	create post action method for newly formed form in 'EmployeeController.cs'
			- 	[HttpPost]
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

					return View("Home/Index");
				}
				
18.	to show all employee in table formate we will create action method
			- create an action method inside 'EmployeeController.cs'
			-		[HttpGet]
					public async Task<IActionResult> Index() 
					{
						var employees = await mvcDemoTestDbContext.Employees.ToListAsync();

						return View(employees);
					}
			- create 'View' from that action method
			-		@model List<mvcCrudeApplication.Models.Domain.Employee>
					@{
					}
					<h1>Employee List</h1>
					<table class="table">
						<thead>
							<tr>
								<th>Id</th>
								<th>Name</th>
								<th>Email</th>
								<th>Salary</th>
								<th>Date of Birth</th>
								<th>Department</th>
							</tr>
						</thead>
						<tbody>
							@foreach(var employee in Model)
							{
								<tr>
									<td>@employee.Id</td>
									<td>@employee.Name</td>
									<td>@employee.Email</td>
									<td>@employee.Salary</td>
									<td>@employee.BirthDate</td>
									<td>@employee.Dept</td>
								</tr>
							}
						</tbody>
					</table>
					
19.	to update employee 
			- first to create 'UpdateEmployee.cs' class inside model
			-	public Guid Id { get; set; }
				public string Name { get; set; }
				public string Email { get; set; }
				public int Salary { get; set; }
				public DateTime BirthDate { get; set; }
				public string Dept { get; set; }
			
			- create action method of name 'View'
			-	[HttpGet]
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
			- create view for this method and write form 
					@model mvcCrudeApplication.Models.UpdateEmployee
					@{
					}
					<h1>Edit Employee</h1>
					<form method="post" action="View" class="mt-5">
						<div class="mb-3">
							<label for="" class="form-label">Id</label>
							<input type="text" class="form-control" asp-for="Id" readonly>
						</div>
						<div class="mb-3">
							<label for="" class="form-label">Name</label>
							<input type="text" class="form-control" asp-for="Name">
						</div>
						<div class="mb-3">
							<label for="" class="form-label">Email</label>
							<input type="email" class="form-control" asp-for="Email">
						</div>
						<div class="mb-3">
							<label for="" class="form-label">Salary</label>
							<input type="number" class="form-control" asp-for="Salary">
						</div>
						<div class="mb-3">
							<label for="" class="form-label">Date of Birth</label>
							<input type="date" class="form-control" asp-for="BirthDate">
						</div>
						<div class="mb-3">
							<label for="" class="form-label">Department</label>
							<input type="text" class="form-control" asp-for="Dept">
						</div>

						<button type="submit" class="btn btn-primary">Submit</button>

						
					</form>
				
				- it will show all the editable info in form formate

20.	create post action method to submit changes that made
				- create post action method of same name
				-		[HttpPost]
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
						
21. to delete 
				- create post action method name as 'Delete'
				-		[HttpPost]
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
				- add new button in 'View'
						<button type="submit" class="btn btn-danger"
								asp-action="Delete" asp-controller="Employee">Delete</button>
