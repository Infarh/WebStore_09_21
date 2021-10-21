namespace WebStore.Domain.Models
{
    /// <summary>Информация о сотруднике</summary>
    public class Employee
    {
        /// <summary>Идентификатор сотрудника</summary>
        public int Id { get; set; }

        /// <summary>Имя</summary>
        public string FirstName { get; set; }

        /// <summary>Фамилия</summary>
        public string LastName { get; set; }

        /// <summary>Отчество</summary>
        public string Patronymic { get; set; }

        /// <summary>Возраст</summary>
        public int Age { get; set; }

        //public static bool operator ==(Employee e1, Employee e2)
        //{
        //    return e1.Id == e2.Id;
        //}
        //public static bool operator !=(Employee e1, Employee e2)
        //{
        //    return e1.Id != e2.Id;
        //}

        public override string ToString() => $"[{Id}]{LastName} {FirstName} {Patronymic} ({Age})";
    }

    //public record Employee2(int Id, string LastName, string FirstName, string Patronymic, int Age);
}
