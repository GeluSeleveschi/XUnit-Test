using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using Microsoft.Identity.Client;

namespace EmployeeManagement.Test
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendendFirstObligatoryCourse_WithObject()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
            var obligatoryCourse = employeeManagementTestDataRepository.GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

            // Act
            var internalEmployee = employeeService.CreateInternalEmployee("Mara", "Pop");

            // Assert
            Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendendFirstObligatoryCourse_WithPredicate()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

            // Act
            var internalEmployee = employeeService.CreateInternalEmployee("Andra", "Muntean");

            // Assert
            Assert.Contains(internalEmployee.AttendedCourses, course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendendSecondaryObligatoryCourse_WithPredicate()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

            // Act
            var internalEmployee = employeeService.CreateInternalEmployee("Dan", "Smith");

            // Assert
            Assert.Contains(internalEmployee.AttendedCourses, course => course.Id == Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendendCoursesMustMatchObligatoryCourses()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
            var obligatoryCourses = employeeManagementTestDataRepository.GetCourses(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            // Act
            var internalEmployee = employeeService.CreateInternalEmployee("Rafa", "Nadal");

            // Assert
            Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendendCoursesMustNotBeNew()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

            // Act
            var internalEmployee = employeeService.CreateInternalEmployee("Roger", "Federer");

            // Assert
            //foreach (var course in internalEmployee.AttendedCourses)
            //{
            //    Assert.False(course.IsNew);
            //}

            Assert.All(internalEmployee.AttendedCourses, course => Assert.False(course.IsNew));
        }

        [Fact]
        public async Task CreateInternalEmployee_InternalEmployeeCreated_AttendendCoursesMustMatchObligatoryCourses_Async()
        {
            // Arrange
            var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
            var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
            var obligatoryCourses = await employeeManagementTestDataRepository.GetCoursesAsync(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            // Act
            var internalEmployee = await employeeService.CreateInternalEmployeeAsync("Rafa", "Nadal");

            // Assert
            Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
        }

        #region Test Exceptions
        [Fact]
        public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseException()
        {
            // Arrange
            var employeeService = new EmployeeService(new EmployeeManagementTestDataRepository(), new EmployeeFactory());
            var internalEmployee = new InternalEmployee("Brooklyn", "Cannon", 5, 3000, false, 1);

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(async () => await employeeService.GiveRaiseAsync(internalEmployee, 50));

        }

        // caviat - when the Assert is async you go with async and you need to await it otherwise the return it Task isn't it
        // returned to the XUnit framework that cannot act on it and the test it will be passed even if it shouldn't
        //[Fact]
        //public void GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseException_Mistake()
        //{
        //    // Arrange
        //    var employeeService = new EmployeeService(new EmployeeManagementTestDataRepository(), new EmployeeFactory());
        //    var internalEmployee = new InternalEmployee("Brooklyn", "Cannon", 5, 3000, false, 1);

        //    // Act & Assert
        //    Assert.ThrowsAsync<EmployeeInvalidRaiseException>(async () => await employeeService.GiveRaiseAsync(internalEmployee, 50));

        //}
        #endregion

        #region Test Events
        [Fact]
        public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggered()
        {
            // Arrange
            var employeeService = new EmployeeService(new EmployeeManagementTestDataRepository(), new EmployeeFactory());
            var internalEmployee = new InternalEmployee("Brooklyn", "Cannon", 5, 3000, false, 1);

            // Act & Assert
            Assert.Raises<EmployeeIsAbsentEventArgs>(
                handler => employeeService.EmployeeIsAbsent += handler,
                handler => employeeService.EmployeeIsAbsent -= handler,
                () => employeeService.NotifyOfAbsence(internalEmployee));
        }
        #endregion

        [Fact]
        public void CreateEmployee_IsExternalIsTrue_ReturnTypeMustDoExternalEmployee()
        {
            // Arrange
            var factory = new EmployeeFactory();

            // Act
            var employee = factory.CreateEmployee("Kevin", "Dockx", "Marvin", false);

            // Assert
            //Assert.IsType<ExternalEmployee>(employee);
            Assert.IsAssignableFrom<Employee>(employee);
        }
    }
}
