using System.Collections.Generic;
using System.Linq;
using InterviewTakeawayTask2.Models;

namespace InterviewTakeawayTask2.Repositories
{

    //singleton repo class for all users, guess database connection stuffs would go here in a real service, would use methods on a IDatabase connection property to interact with the database.
    //worth mentioning that whilst this class is used as a singleton, we are leveraging the framework and injecting it as a service. Typically we would expect singleton logic to be coupled with its own class. (Private instance that is controlled via the constructor)

    public class UserRepository
    {
        private readonly List<User> _users;
        private int _nextId = 1;

        public UserRepository()
        {
            // Initializing with some dummy users
            _users = new List<User>
            {
                new User { Id = _nextId++, Username = "alice@example.com", Password="Password", Name = "Alice", Email = "alice@example.com", Age = 25 },
                new User { Id = _nextId++, Username = "bob@example.com", Password="Password", Name = "Bob", Email = "bob@example.com", Age = 30 }
            };
        }

        // Get all users
        public List<User> GetAllUsers() => _users;

        //Get a user by ID.
        //Using a simple linq expression to get the user by ID, although, there is no mitigating action if the user is not found, would be good to return a 404 or something or "User not found" etc.
        public User GetUserById(int id) => _users.FirstOrDefault(u => u.Id == id);

        //checks if the user is in the collection or not....
        public bool CheckUserExists(int id) => _users.Any(u => u.Id == id);

        // Create a new user
        //Straightforward method to create a new user, would be good to add some validation here, like checking if the email is unique, password is strong enough, etc. Can at least validate fields during model binding in the controller.
        //Id is just incremented by 1 each time a new user is created, would be better to use a GUID or something unique, I know this example is simple, but in a real service, this could cause issues and would need to verify.
        public User CreateUser(string name, string password, string email, int age)
        {
            var user = new User
            {
                Id = _nextId++,
                Username = email,
                Password = password,
                Name = name,
                Email = email,
                Age = age
            };

            _users.Add(user);
            return user;
        }

    }
}
