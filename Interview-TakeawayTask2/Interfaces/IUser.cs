using System.ComponentModel.DataAnnotations;

namespace Interview_TakeawayTask2.Interfaces
{
    public interface IUser
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Name { get; set; }

        public string Email { get; set; } 

    }
}
