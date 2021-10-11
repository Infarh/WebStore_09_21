namespace WebStore.Domain.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public int Age { get; set; }

        //public static bool operator ==(Employee e1, Employee e2)
        //{
        //    return e1.Id == e2.Id;
        //}
        //public static bool operator !=(Employee e1, Employee e2)
        //{
        //    return e1.Id != e2.Id;
        //}
    }

    //public record Employee2(int Id, string LastName, string FirstName, string Patronymic, int Age);
}
