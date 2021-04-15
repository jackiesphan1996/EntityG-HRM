using EntityG.BusinessLogic.Services;
using EntityG.Contracts.Requests.Employees;
using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces;
using EntityG.EntityFramework.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Test.Services
{
    [TestFixture]
    public class EmployeeServiceTest
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<EmployeeService>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _loggerMock = new Mock<ILogger<EmployeeService>>();
        }

        [Test]
        public async Task GetAllAsync_Successfully()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            string search = "";
            int skip = (page - 1) * pageSize;
            int totalCount = 9999;
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    DepartmentId = 1,
                    AccountNumber = "13-12-99",
                    FirstName = "Steven",
                    LastName = "Phan",
                    Email = "steven.phan@entityg.com",
                    Phone = "(+84) 094149999"
                }
            };

            _employeeRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<Employee, bool>>>(),
                It.IsAny<Func<IQueryable<Employee>,
                IIncludableQueryable<Employee, object>>>(),
                It.IsAny<Func<IQueryable<Employee>,
                IOrderedQueryable<Employee>>>(),
                skip,
                pageSize,
                true)).ReturnsAsync(employees);

            _employeeRepositoryMock.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(totalCount);

            var employeeService = new EmployeeService(_employeeRepositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);


            // Act
            var result = await employeeService.GetAllAsync(page, pageSize, search);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(totalCount, result.TotalCount);
            Assert.AreEqual(employees.Count, result.Data.Count);
            _employeeRepositoryMock.Verify(x =>  x.CountAsync(It.IsAny<Expression<Func<Employee, bool>>>()), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_Throws_Exception()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            string search = "";

            _employeeRepositoryMock.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Employee, bool>>>())).Throws<Exception>();

            var employeeService = new EmployeeService(_employeeRepositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);

            // Act & Assert 
            var exception = Assert.ThrowsAsync<Exception>( async() => await employeeService.GetAllAsync(page, pageSize, search));

            _loggerMock.Verify(x => x.Log(LogLevel.Error,
               It.IsAny<EventId>(),
               It.IsAny<It.IsAnyType>(),
               It.IsAny<Exception>(),
               (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
               Times.Exactly(1));
        }

        [Test]
        public async Task CreateAsync_Succesfully()
        {
            // Arrange
            var request = new CreateEmployeeDto
            {
                FirstName = "Steven",
                LastName = "Phan",
                DepartmentId = 1,
                DateOfBirth = DateTime.Now,
                AccountNumber = "Account Number ",
                AccountTitle = "Mr",
                Address1 = "Address 1"
            };

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var employeeService = new EmployeeService(_employeeRepositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);

            // Act
            var result = await employeeService.CreateAsync(request);
           
            // Assert
            Assert.AreEqual(1, result);
            _employeeRepositoryMock.Verify(x => x.Add(It.IsAny<Employee>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CreateAsync_Should_Throw_Exception_With_Null_Request()
        {
            // Arrange
            CreateEmployeeDto request = null;

            var employeeService = new EmployeeService(_employeeRepositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => employeeService.CreateAsync(request));
        }


        [Test]
        public void EmployeeService_Constructors_WithThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new EmployeeService(null, _unitOfWorkMock.Object, _loggerMock.Object));
            Assert.Throws<ArgumentNullException>(() => new EmployeeService(_employeeRepositoryMock.Object, null , _loggerMock.Object));
            Assert.Throws<ArgumentNullException>(() => new EmployeeService(_employeeRepositoryMock.Object, _unitOfWorkMock.Object, null));
        }
    }
}