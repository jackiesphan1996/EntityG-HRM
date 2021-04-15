using EntityG.BusinessLogic.Helpers;
using EntityG.BusinessLogic.Services.Interfaces.Shared;
using EntityG.EntityFramework.Contexts;
using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Entities.Identity;
using EntityG.Shared.Constants.Permission;
using EntityG.Shared.Constants.Role;
using EntityG.Shared.Constants.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services.Shared
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db, ILogger<DatabaseSeeder> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = db;
            _logger = logger;
        }

        public void Initialize()
        {
            AddCustomerPermissionClaims();
            AddAdministrator();
            AddBasicUser();
            InsertDemoData();
            _context.SaveChanges();
        }

        private void AddCustomerPermissionClaims()
        {
            Task.Run(async () =>
            {
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstant.AdministratorRole);
                if (adminRoleInDb != null)
                {
                    await _roleManager.AddCustomPermissionClaim(adminRoleInDb, "Permissions.Communication.Chat");
                }
            }).GetAwaiter().GetResult();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new IdentityRole(RoleConstant.AdministratorRole);
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstant.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    _logger.LogInformation("Seeded Administrator Role.");
                }
                //Check if User Exists
                var superUser = new ApplicationUser()
                {
                    FirstName = "Nhat Hoang",
                    LastName = "Phan",
                    Email = "hoang.nhat.phan@entityg.com",
                    UserName = "hoang.nhat.phan",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, UserConstant.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RoleConstant.AdministratorRole);
                    if (result.Succeeded)
                    {
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Users);
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Roles);
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Products);
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Brands);
                    }
                    _logger.LogInformation("Seeded User with Administrator Role.");
                }
            }).GetAwaiter().GetResult();
        }

        private void AddBasicUser()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var basicRole = new IdentityRole(RoleConstant.BasicRole);
                var basicRoleInDb = await _roleManager.FindByNameAsync(RoleConstant.BasicRole);
                if (basicRoleInDb == null)
                {
                    await _roleManager.CreateAsync(basicRole);
                    _logger.LogInformation("Seeded Basic Role.");
                }
                //Check if User Exists
                var basicUser = new ApplicationUser()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@blazorhero.com",
                    UserName = "johndoe",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };
                var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
                if (basicUserInDb == null)
                {
                    await _userManager.CreateAsync(basicUser, UserConstant.DefaultPassword);
                    await _userManager.AddToRoleAsync(basicUser, RoleConstant.BasicRole);
                    _logger.LogInformation("Seeded User with Basic Role.");
                }
            }).GetAwaiter().GetResult();
        }

        private void InsertDemoData()
        {
            Task.Run(async () =>
            {
                try
                {
                    if (_context.AssetTypes.Any())
                    {
                        return;
                    }

                    //insert asset type
                    AssetType ppeAsset = new AssetType()
                    {
                        Name = "PPE",
                        Description = "(Property, Plant, and Equipment)"
                    };
                    AssetType inventoryAsset = new AssetType()
                    {
                        Name = "Inventory",
                        Description = ""
                    };
                    AssetType landAsset = new AssetType()
                    {
                        Name = "Land",
                        Description = ""
                    };
                    AssetType buildingAsset = new AssetType()
                    {
                        Name = "Buildings",
                        Description = ""
                    };
                    AssetType vehicleAsset = new AssetType()
                    {
                        Name = "Vehicles",
                        Description = ""
                    };
                    AssetType furnitureAsset = new AssetType()
                    {
                        Name = "Furniture",
                        Description = ""
                    };
                    AssetType patentAsset = new AssetType()
                    {
                        Name = "Patents",
                        Description = ""
                    };
                    AssetType machineryAsset = new AssetType()
                    {
                        Name = "Machinery",
                        Description = ""
                    };
                    AssetType equipmentAsset = new AssetType()
                    {
                        Name = "Equipment",
                        Description = ""
                    };
                    AssetType laptopAsset = new AssetType()
                    {
                        Name = "Laptop",
                        Description = ""
                    };
                    AssetType pcAsset = new AssetType()
                    {
                        Name = "PC",
                        Description = ""
                    };
                    AssetType tvAsset = new AssetType()
                    {
                        Name = "TV",
                        Description = ""
                    };
                    AssetType infocusAsset = new AssetType()
                    {
                        Name = "Infocus",
                        Description = ""
                    };

                    await _context.AssetTypes.AddAsync(ppeAsset);
                    await _context.AssetTypes.AddAsync(inventoryAsset);
                    await _context.AssetTypes.AddAsync(landAsset);
                    await _context.AssetTypes.AddAsync(buildingAsset);
                    await _context.AssetTypes.AddAsync(vehicleAsset);
                    await _context.AssetTypes.AddAsync(furnitureAsset);
                    await _context.AssetTypes.AddAsync(patentAsset);
                    await _context.AssetTypes.AddAsync(machineryAsset);
                    await _context.AssetTypes.AddAsync(equipmentAsset);
                    await _context.AssetTypes.AddAsync(laptopAsset);
                    await _context.AssetTypes.AddAsync(pcAsset);
                    await _context.AssetTypes.AddAsync(tvAsset);
                    await _context.AssetTypes.AddAsync(infocusAsset);

                    await _context.SaveChangesAsync();

                    //insert department
                    Department itDepartment = new Department()
                    {
                        Name = "IT Department",
                        Description = ""
                    };
                    Department hrDepartment = new Department()
                    {
                        Name = "HR Department",
                        Description = ""
                    };
                    Department financeDepartment = new Department()
                    {
                        Name = "Finance Department",
                        Description = ""
                    };
                    Department salesDepartment = new Department()
                    {
                        Name = "Sales Department",
                        Description = ""
                    };
                    Department warehouseDepartment = new Department()
                    {
                        Name = "Warehouse Department",
                        Description = ""
                    };

                    await _context.Departments.AddAsync(itDepartment);
                    await _context.Departments.AddAsync(hrDepartment);
                    await _context.Departments.AddAsync(financeDepartment);
                    await _context.Departments.AddAsync(salesDepartment);
                    await _context.Departments.AddAsync(warehouseDepartment);


                    await _context.SaveChangesAsync();

                    //random data source: https://www.summet.com/dmsi/html/codesamples/addresses.html

                    

                    Employee stevenPhan = new Employee()
                    {
                        FirstName = "Nhat Hoang",
                        LastName = "Phan",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1992, 1, 2),
                        PlaceOfBirth = "Vietnam",
                        MaritalStatus = "Single",
                        Email = "hoang.nhat.phan@entityg.com",
                        Phone = "(372) 587-2335",
                        Address1 = "P.O. Box 283 8562 Fusce Rd.",
                        City = "Frederick",
                        StateProvince = "Nebraska",
                        ZipCode = "20620",
                        Country = "USA",
                        EmployeeIdNumber = "hoang.nhat.phan",
                        Department = financeDepartment,
                        JoiningDate = new DateTime(2017, 1, 2),
                        BasicSalary = 20000m,
                        UnpaidLeavePerDay = 1000m,
                        AccountTitle = "Iris Watson Payroll",
                        BankName = "Bank of America",
                        AccountNumber = "1111-AMERICA"
                    };

                   
                    await _context.Employees.AddAsync(stevenPhan);
                    await _context.SaveChangesAsync();


                    //insert employee (finance)
                    Employee empCeciliaChapman = new Employee()
                    {
                        FirstName = "Cecilia",
                        LastName = "Chapman",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1992, 1, 1),
                        PlaceOfBirth = "Mankato",
                        MaritalStatus = "Single",
                        Email = "Cecilia.Chapman@entityg.com",
                        Phone = "(257) 563-7401",
                        Address1 = "711-2880 Nulla St.",
                        City = "Mankato",
                        StateProvince = "Mississippi",
                        ZipCode = "96522",
                        Country = "USA",
                        EmployeeIdNumber = "Cecilia.Chapman",
                        Department = financeDepartment,
                        //Supervisor
                        JoiningDate = new DateTime(2017, 1, 1),
                        BasicSalary = 25000m,
                        UnpaidLeavePerDay = 1250m,
                        AccountTitle = "Cecilia Chapman Payroll",
                        BankName = "JPMorgan-Chase",
                        AccountNumber = "1111-CHASE"
                    };
                    await _context.Employees.AddAsync(empCeciliaChapman);
                    await _context.SaveChangesAsync();

                    Employee empIrisWatson = new Employee()
                    {
                        FirstName = "Iris",
                        LastName = "Watson",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1992, 1, 2),
                        PlaceOfBirth = "Frederick",
                        MaritalStatus = "Single",
                        Email = "Iris.Watson@entityg.com",
                        Phone = "(372) 587-2335",
                        Address1 = "P.O. Box 283 8562 Fusce Rd.",
                        City = "Frederick",
                        StateProvince = "Nebraska",
                        ZipCode = "20620",
                        Country = "USA",
                        EmployeeIdNumber = "Iris.Watson",
                        Department = financeDepartment,
                        Supervisor = empCeciliaChapman,
                        JoiningDate = new DateTime(2017, 1, 2),
                        BasicSalary = 20000m,
                        UnpaidLeavePerDay = 1000m,
                        AccountTitle = "Iris Watson Payroll",
                        BankName = "Bank of America",
                        AccountNumber = "1111-AMERICA"
                    };
                    await _context.Employees.AddAsync(empIrisWatson);
                    await _context.SaveChangesAsync();

                    Employee empCelesteSlater = new Employee()
                    {
                        FirstName = "Celeste",
                        LastName = "Slater",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1992, 1, 3),
                        PlaceOfBirth = "Roseville",
                        MaritalStatus = "Single",
                        Email = "Celeste.Slater@entityg.com",
                        Phone = "(786) 713-8616",
                        Address1 = "606-3727 Ullamcorper. Street",
                        City = "Roseville",
                        StateProvince = "NH",
                        ZipCode = "11523",
                        Country = "USA",
                        EmployeeIdNumber = "Celeste.Slater",
                        Department = financeDepartment,
                        Supervisor = empIrisWatson,
                        JoiningDate = new DateTime(2017, 1, 3),
                        BasicSalary = 15000m,
                        UnpaidLeavePerDay = 750m,
                        AccountTitle = "Celeste Slater Payroll",
                        BankName = "Wells Fargo",
                        AccountNumber = "1111-FARGO"
                    };
                    await _context.Employees.AddAsync(empCelesteSlater);
                    await _context.SaveChangesAsync();

                    Employee empTheodoreLowe = new Employee()
                    {

                        FirstName = "Theodore",
                        LastName = "Lowe",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1992, 1, 4),
                        PlaceOfBirth = "Azusa",
                        MaritalStatus = "Single",
                        Email = "Theodore.Lowe@entityg.com",
                        Phone = "(793) 151-6230",
                        Address1 = "Ap #867-859 Sit Rd.",
                        City = "Azusa",
                        StateProvince = "New York",
                        ZipCode = "39531",
                        Country = "USA",
                        EmployeeIdNumber = "Theodore.Lowe",
                        Department = financeDepartment,
                        Supervisor = empCelesteSlater,
                        JoiningDate = new DateTime(2017, 1, 4),
                        BasicSalary = 10000m,
                        UnpaidLeavePerDay = 500m,
                        AccountTitle = "Theodore Lowe Payroll",
                        BankName = "Bank of New York Mellon",
                        AccountNumber = "1111-MELLON"
                    };
                    await _context.Employees.AddAsync(empTheodoreLowe);
                    await _context.SaveChangesAsync();

                    Employee empCalistaWise = new Employee()
                    {

                        FirstName = "Calista",
                        LastName = "Wise",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1992, 1, 5),
                        PlaceOfBirth = "San Antonio",
                        MaritalStatus = "Single",
                        Email = "Calista.Wise@entityg.com",
                        Phone = "(492) 709-6392",
                        Address1 = "7292 Dictum Av.",
                        City = "San Antonio",
                        StateProvince = "MI",
                        ZipCode = "47096",
                        Country = "USA",
                        EmployeeIdNumber = "Calista.Wise",
                        Department = financeDepartment,
                        Supervisor = empTheodoreLowe,
                        JoiningDate = new DateTime(2017, 1, 5),
                        BasicSalary = 9000m,
                        UnpaidLeavePerDay = 450m,
                        AccountTitle = "Calista Wise Payroll",
                        BankName = "Capital One",
                        AccountNumber = "1111-ONE"
                    };
                    await _context.Employees.AddAsync(empCalistaWise);
                    await _context.SaveChangesAsync();


                    //insert employee (IT)
                    Employee empKylaOlsen = new Employee()
                    {

                        FirstName = "Kyla",
                        LastName = "Olsen",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1992, 2, 1),
                        PlaceOfBirth = "Tamuning",
                        MaritalStatus = "Single",
                        Email = "Kyla.Olsen@entityg.com",
                        Phone = "(654) 393-5734",
                        Address1 = "Ap #651-8679 Sodales Av.",
                        City = "Tamuning",
                        StateProvince = "PA",
                        ZipCode = "10855",
                        Country = "USA",
                        EmployeeIdNumber = "Kyla.Olsen",
                        Department = itDepartment,
                        Supervisor = empCeciliaChapman,
                        JoiningDate = new DateTime(2017, 2, 1),
                        BasicSalary = 25000m,
                        UnpaidLeavePerDay = 1250m,
                        AccountTitle = "Kyla Olsen Payroll",
                        BankName = "JPMorgan-Chase",
                        AccountNumber = "2222-CHASE"
                    };
                    await _context.Employees.AddAsync(empKylaOlsen);
                    await _context.SaveChangesAsync();

                    //insert and connect system user with employee
                    var employees = _context.Employees.ToList();
                    foreach (var item in employees)
                    {
                        ApplicationUser appUser = new ApplicationUser();
                        if (item.Email.Equals("hoang.nhat.phan@entityg.com"))
                        {
                            appUser = await _userManager.FindByEmailAsync(item.Email);
                        }
                        else
                        {
                            appUser.Email = item.Email;
                            appUser.UserName = item.Email;
                            appUser.EmailConfirmed = true;
                            appUser.FirstName = item.FirstName;
                            appUser.LastName = item.LastName;
                            appUser.IsActive = true;
                            appUser.PhoneNumber = item.Phone;

                            //create system user
                            await _userManager.CreateAsync(appUser, "123456");
                            await _userManager.AddToRoleAsync(appUser, RoleConstant.BasicRole);
                        }
                     

                       
                        
                        _logger.LogInformation("Seeded User with Basic Role.");
                        //connect employee with their system user

                        item.SystemUser = appUser;


                        _context.Employees.Update(item);
                        await _context.SaveChangesAsync();

                        //assign role SelfService to employee
                        await _userManager.AddToRoleAsync(appUser, "Basic");

                    }

                    Project projectTSC = new Project
                    {
                        Name = "TSC Miami"
                    };

                    Project projectTopicus = new Project
                    {
                        Name = "Topicus -Finance - Mortgage"
                    };

                    _context.Projects.Add(projectTSC);
                    _context.Projects.Add(projectTopicus);

                    await _context.SaveChangesAsync();


                    //insert leave type
                    LeaveType annualLeave = new LeaveType()
                    {
                        Name = "Annual Leave",
                        Description = ""
                    };
                    LeaveType newBornChildCareLeave = new LeaveType()
                    {
                        Name = "New Born Child Care Leave",
                        Description = ""
                    };
                    LeaveType marriageLeave = new LeaveType()
                    {
                        Name = "Marriage Leave",
                        Description = ""
                    };
                    LeaveType familyFuneralLeave = new LeaveType()
                    {
                        Name = "Family funeral Leave",
                        Description = ""
                    };
                    LeaveType pregnancyCareLeave = new LeaveType()
                    {
                        Name = "Pregnancy Care Leave",
                        Description = ""
                    };

                    await _context.LeaveTypes.AddAsync(annualLeave);
                    await _context.LeaveTypes.AddAsync(newBornChildCareLeave);
                    await _context.LeaveTypes.AddAsync(marriageLeave);
                    await _context.LeaveTypes.AddAsync(familyFuneralLeave);
                    await _context.LeaveTypes.AddAsync(pregnancyCareLeave);

                    await _context.SaveChangesAsync();

                    //insert leave
                    List<LeaveType> leaveTypes = new List<LeaveType>();
                    leaveTypes = _context.LeaveTypes.ToList();

                    DateTime startLeave = new DateTime(2021, 4, 1);
                    DateTime endLeave = DateTime.Now;

                    //set random leave dates
                    List<int> leaveDates = new List<int>();
                    for (int i = 0; i < 15; i++)
                    {
                        int randomDate = new Random().Next(1, 30);
                        if (!leaveDates.Contains(randomDate))
                        {
                            leaveDates.Add(randomDate);
                        }

                    }

                    for (DateTime date = startLeave.Date; date < endLeave.Date; date = date.AddDays(1))
                    {
                        if (leaveDates.Contains(date.Day)
                            && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
                        {
                            Leave leave = new Leave();
                            leave.LeaveType = leaveTypes[new Random().Next(0, leaveTypes.Count() - 1)];
                            leave.Employee = employees[new Random().Next(0, employees.Count - 1)];
                            leave.Description = leave.Employee.FirstName + " " + leave.Employee.LastName + " on " + leave.LeaveType.Name;
                            leave.IsPaidLeave = (new Random().Next(0, 2) == 1) ? true : false;
                            leave.FromDate = date;
                            leave.ToDate = date;
                            leave.EmergencyContact = leave.Employee.Phone;
                            leave.IsApproved = new Random().Next(0, 2) == 1 ? true : false;
                            await _context.Leaves.AddAsync(leave);
                        }

                    }
                    await _context.SaveChangesAsync();

                    //insert asset
                    List<string> excludeAssets = new List<string>();
                    excludeAssets.Add("Laptop");
                    excludeAssets.Add("PPE");
                    excludeAssets.Add("Patents");
                    excludeAssets.Add("Land");
                    excludeAssets.Add("Buildings");

                    List<decimal> prices = new List<decimal>();
                    prices.Add(1500m);
                    prices.Add(1700m);
                    prices.Add(2500m);
                    prices.Add(2700m);
                    prices.Add(3200m);

                    List<AssetType> assetTypes = new List<AssetType>();
                    assetTypes = _context.AssetTypes.Where(x => !excludeAssets.Contains(x.Name))
                        .ToList();

                    DateTime startAsset = new DateTime(2021, 2, 1);
                    DateTime endAsset = DateTime.Now;

                    //set random asset dates
                    List<int> assetDates = new List<int>();
                    for (int i = 0; i < 5; i++)
                    {
                        int randomDate = new Random().Next(1, 30);
                        if (!assetDates.Contains(randomDate))
                        {
                            assetDates.Add(randomDate);
                        }

                    }

                    //laptop asset
                    AssetType assetLaptop = new AssetType();
                    assetLaptop = _context.AssetTypes.Where(x => x.Name.Equals("Laptop")).FirstOrDefault();
                    int laptop = 0;
                    foreach (var item in employees)
                    {
                        laptop++;

                        Asset asset = new Asset();
                        asset.AssetType = assetLaptop;
                        asset.PurchaseDate = new DateTime(2021, 1, new Random().Next(1, 28));
                        asset.PurchasePrice = prices[new Random().Next(0, 4)];
                        asset.AssetName = asset.PurchaseDate.ToString("yyyy-MM-dd") + " " + asset.AssetType.Name + " " + laptop.ToString();
                        asset.Description = "";
                        asset.UsedBy = item;
                        asset.IsActive = true;
                        await _context.Assets.AddAsync(asset);
                    }

                    //other asset
                    for (DateTime date = startAsset.Date; date < endAsset.Date; date = date.AddDays(1))
                    {
                        if (assetDates.Contains(date.Day)
                            && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
                        {
                            Asset asset = new Asset();
                            asset.AssetType = assetTypes[new Random().Next(0, assetTypes.Count - 1)];
                            asset.AssetName = date.ToString("yyyy-MM-dd") + " " + asset.AssetType.Name;
                            asset.Description = "";
                            asset.PurchaseDate = date;
                            asset.PurchasePrice = prices[new Random().Next(0, 4)];
                            asset.UsedBy = new Random().Next(0, 2) == 1 ? employees[new Random().Next(0, employees.Count - 1)] : null;
                            asset.IsActive = true;
                            await _context.Assets.AddAsync(asset);
                        }

                    }
                    await _context.SaveChangesAsync();

                }
                catch (Exception)
                {
                    throw;
                }
            }).GetAwaiter().GetResult();
        }
    }
}
