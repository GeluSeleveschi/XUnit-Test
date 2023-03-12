﻿using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class DemoInternalEmployeesControllerTests
    {
        [Fact]
        public async Task CreateInternalEmployee_InvalidInput_MustReturnBadRequest()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var mapperMock = new Mock<IMapper>();

            var demoInternalEmployeesController = new DemoInternalEmployeesController(employeeServiceMock.Object, mapperMock.Object);
            var internalEmployeeForCreationDto = new InternalEmployeeForCreationDto();

            demoInternalEmployeesController.ModelState.AddModelError("FirstName", "Required");

            // Act
            var result = await demoInternalEmployeesController.CreateInternalEmployee(internalEmployeeForCreationDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Models.InternalEmployeeDto>>(result);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }
    }
}
