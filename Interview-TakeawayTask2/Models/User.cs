using Interview_TakeawayTask2.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace InterviewTakeawayTask2.Models
{
    //Model for a stanard user. Will require model validations. Could also add a interface hierarchy to begin introducing some SOLID principles. (LSP, ISP, etc)
    public class User : ICreateUser
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username not supplied.")]
        public string? Username {get; set;}

        [Required(ErrorMessage = "Password not supplied.")]
        public  string? Password {get; set;}

        [Required(ErrorMessage = "Name not supplied.")]
        public string? Name { get; set; }

        //looks like usernames are normally emails...
        public string Email { get; set; } = "default";

        [Range(0, 150, ErrorMessage = "Please enter a valid age.")]
        //method put in by the example was passing age in as a non null int, so I will do the same via interfaces.
        public int Age { get; set; }
    }
}
