using Interview_TakeawayTask2.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace InterviewTakeawayTask2.Models
{
    //same again, we could use some interfaces here to introduce some SOLID principles, and also add some model validations.
    //could also "describe" different kinds of requests with interfaces, and endpoints could expect a IRequest interface, and then we could have different implementations of IRequest for different kinds of requests.
    //Really this would just mean we could have a more flexible system, and we could add more functionality to the system without having to change the existing code, but would have to be careful to not break SRP.

    //Change the required fields to instead leverage model validation using attributes.
    public class UserRequest : IUpdateUser
    {
        [Required(ErrorMessage = "An ID must be passed to change any details.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        public string? Password { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "The password must contain at least 8 characters, including an uppercase letter, a lowercase letter, a number, and a symbol.")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Please enter a username.")]
        public string? Username { get; set; }

        public string? NewUsername { get; set; }

        public string? Name { get; set; }

        //could use the email attribute here, but seeming it can be "" or something, need to rethink.
        public string Email { get; set; } = "default";

        [Range(0, 150, ErrorMessage = "Please enter a valid age.")]
        public int? Age { get; set; }
    }
}
