namespace mvcCrudeApplication.Models.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Salary { get; set; }
        public DateTime BirthDate { get; set;}
        public string Dept { get; set;}

    }
}
