using System;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Member
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Date Of Birth is required")]
        [Required]
        public DateTime DateOfBirth { get; set; }
        [RegularExpression("^\\d{9,}$")]
        public string PhoneNumber { get; set; }
        public string BirthPlace { get; set; }

        [Required]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")]
        public string Email { get; set; } = "example@gmail.com";
        public bool IsGraduated { get; set; }
        public int Age
        {
            get { return calcAge(); }
            set { }
        }
        public int calcAge()
        {
            int surplus = 0;
            int years = DateTime.Now.Year - DateOfBirth.Year;
            int months = DateTime.Now.Month - DateOfBirth.Month;
            int days = DateTime.Now.Day - DateOfBirth.Day;

            if (((days == 0 || days > 0) && months == 0) || (months > 0))
            {
                surplus = 1;
            }

            return years + surplus;
        }


        public string FullName()
        {
            return LastName + " " + FirstName;
        }

    }
}
