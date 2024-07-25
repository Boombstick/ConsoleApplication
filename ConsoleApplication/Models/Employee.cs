namespace ConsoleApplication.Models
{
    public class Employee
    {
        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal SalaryPerHour { get; set; }
        public Employee(int id, string firstName, string lastName, decimal salaryPerHour)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            SalaryPerHour = salaryPerHour;
        }

        public override bool Equals(object? obj)
        {
            return obj is Employee employee &&
                   Id == employee.Id &&
                   FirstName == employee.FirstName &&
                   LastName == employee.LastName &&
                   SalaryPerHour == employee.SalaryPerHour;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, SalaryPerHour);
        }
    }
}