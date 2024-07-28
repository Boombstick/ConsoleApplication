using ConsoleApplication.Utils;
using ConsoleApplication.Models;
using ConsoleApplication.Commands;
using ConsoleApplication.Abstractions;

namespace ConsoleApplication.UnitTests
{
    public class AddEmployeeCommandTests
    {
        private List<Employee> _employees;
        private Command _addCommand;

        [SetUp]
        public void Setup()
        {
            _addCommand = new AddEmployeeCommand();
            _employees = new()
            {
                new Employee(1,"John","Domond",100),
                new Employee(2,"Artur","Morgan",100.100M),
            };
        }

        [TestCase("FirstName:Gena", "LastName:Bukin", "SalaryPerHour:10123,112312")]
        [TestCase("FirstName:Gena", "LastName:Bukin", "SalaryPerHour:12312")]
        [TestCase("FirstName:Maxim", "LastName:Brasov", "SalaryPerHour:100,12")]
        public void Should_Apply_When_ParametersIsValid(params string[] parameters)
        {
            _addCommand.Execute(_employees, new[] { parameters[0], parameters[1], parameters[2] });
            Assert.That(_employees.First(x => x.Id == _employees.Count()), Is.EqualTo(new Employee(_employees.Count(),
                ParameterUtils.TryGetParamValue(parameters[0]).Value,
                ParameterUtils.TryGetParamValue(parameters[1]).Value,
                decimal.Parse(ParameterUtils.TryGetParamValue(parameters[2]).Value))));
        }

        [TestCase( "FirstName:Maxim", "Brasov" )]
        [TestCase("Gena", "LastName:Bukin", "SalaryPerHour:101230,112312")]
        [TestCase("FirtName:Gena", "LastName:Bukin", "SalaryPerHour:101230,112312")]
        [TestCase("FirstName:Gena", "LastName Bukin", "SalaryPerHour:101230,112312")]
        [TestCase("FirstName:Gena", "LastName:Bukin", ":101230,112312")]
        public void Should_ThrowException_When_ParameterNameIsInvalid(params string[] parameters)
        {
            Assert.Throws<ArgumentException>(() => _addCommand.Execute(_employees,parameters));
        }

        [TestCase("FirstName:Max12im", "Brasov")]
        [TestCase("FirstName:Gena", "LastName:Bukin", "SalaryPerHour:101230אפû112312")]
        [TestCase("FirstName:Gena", "LastName:Bukin", "SalaryPerHour:101230sgas112312")]
        [TestCase("FirstName:Gena", "LastName Buk22in", "SalaryPerHour:101230,112312")]
        [TestCase("FirstName: ", "LastName:Bukin", "SalaryPerHour:101230,112312")]
        [TestCase("FisrtName:Gena", "LastName:Bukin", "SalaryPerHour:10112312312312310.112312")]
        [TestCase("FirstName:Gena", "LastName:Bukin", "SalaryPerHour:101123,1231231,212230,112312")]
        public void Should_ThrowException_When_ParameterValueIsInvalid(params string[] parameters)
        {
            Assert.Throws<ArgumentException>(() => _addCommand.Execute(_employees, parameters));
        }
        [TestCase("FirstName:Gena", "LastName:Bukin", "SalaryPerHour:1011252346235331231231231231231212230,112312")]
        public void Should_ThrowException_When_SalaryValueIsTooLarge(params string[] parameters)
        {
            Assert.Throws<InvalidCastException>(() => _addCommand.Execute(_employees, parameters));
        }
    }
}