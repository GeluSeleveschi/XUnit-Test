using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test
{
    public class EmployeeFactoryTests
    {

        [Fact]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500()
        {
            // Arrange
            var employeeFactory = new EmployeeFactory();

            // Act
            var employee = (InternalEmployee)employeeFactory.CreateEmployee("Kevin", "Dockx");

            // Assert
            Assert.Equal(2500, employee.Salary);
        }

        [Fact]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
        {
            // Arrange
            var employeeFactory = new EmployeeFactory();

            // Act
            var employee = (InternalEmployee)employeeFactory.CreateEmployee("Kevin", "Dockx");

            // Assert
            Assert.True(employee.Salary >= 2500 && employee.Salary <= 3500, "Salary not in the acceptable range.");
        }

        [Fact]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500AlternativeWay()
        {
            // Arrange
            var employeeFactory = new EmployeeFactory();

            // Act
            var employee = (InternalEmployee)employeeFactory.CreateEmployee("Kevin", "Dockx");

            // Assert
            Assert.True(employee.Salary >= 2500);
            Assert.True(employee.Salary <= 3500);
        }

        [Fact]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500UsingInRange()
        {
            // Arrange
            var employeeFactory = new EmployeeFactory();

            // Act
            var employee = (InternalEmployee)employeeFactory.CreateEmployee("Kevin", "Dockx");

            // Assert
            Assert.InRange(employee.Salary, 2500, 3500);
        }

        [Fact]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500Precision()
        {
            // Arrange
            var employeeFactory = new EmployeeFactory();

            // Act
            var employee = (InternalEmployee)employeeFactory.CreateEmployee("Kevin", "Dockx");
            employee.Salary = 2500.132m;
            // Assert
            Assert.Equal(2500, employee.Salary, 0); // number of digits that will be taken into consideration after comma
        }
    }
}