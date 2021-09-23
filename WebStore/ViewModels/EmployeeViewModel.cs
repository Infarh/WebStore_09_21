using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel : IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя не указано")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длина от 2 до 200 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия не указана")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длина от 2 до 200 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Range(18, 80, ErrorMessage = "Возраст должнен быть от 18 до 80 лет")]
        public int Age { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext Context)
        {
            switch (Context.MemberName)
            {
                //default: return Enumerable.Empty<ValidationResult>();
                default: return new[] { ValidationResult.Success };

                case nameof(Age):
                    if (Age < 15 || Age > 90) 
                        return new[] { new ValidationResult("Странный возраст", new []{ nameof(Age) }) };
                    return new[] { ValidationResult.Success };
            }
        }
    }
}
