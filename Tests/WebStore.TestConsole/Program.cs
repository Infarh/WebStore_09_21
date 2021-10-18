using System;

namespace WebStore.TestConsole
{
    public record Student1(string LastName, string FirstName, int Age)
    {
        //public Student1()
        //{
        //    LastName = "";
        //    FirstName = "";
        //    Age = 5;
        //}
    }

    class Program
    {
        private record Student
        {
            public string LastName { get; init; }
            public string FirstName { get; init; }
            public int Age { get; init; }
        }

        static void Main(string[] args)
        {
            var s1 = new Student
            {
                LastName = "Last1",
                FirstName = "First1",
                Age = 1,
            };

            var s3 = s1 with { Age = 31, LastName = "LL1" };

            var s2 = new Student
            {
                LastName = "Last2",
                FirstName = "First2",
                Age = 2,
            };

            var s4 = new Student1("123", "321", 123);

            var (last, first, age) = s4;

            if (s1 == s2)
            {
                Console.WriteLine("equals");
            }
        }
    }
}
