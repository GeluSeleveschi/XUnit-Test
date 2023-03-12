using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class PromotionsControllerTests
    {
        [Fact]
        public void GetProtectedInternalEmployees_GetActionForUserInAdminRole_MustRedirectToGetInternalEmployeesOnProtectedInternalEmployees_WithMoq()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            var mapperMock = new Mock<IMapper>();

            var demoInternalEmployeesController = new DemoInternalEmployeesController(employeeServiceMock.Object, mapperMock.Object);
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.IsInRole(It.Is<string>(s => s == "Admin"))).Returns(true);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(mockPrincipal.Object);

            demoInternalEmployeesController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = httpContextMock.Object
            };

            // Act
            var result = demoInternalEmployeesController.GetProtectedInternalEmployees();

            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("GetInternalEmployees", redirectToActionResult.ActionName);
            Assert.Equal("ProtectedInternalEmployees", redirectToActionResult.ControllerName);
        }
    }
}
