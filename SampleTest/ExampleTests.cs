using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using InterviewTakeawayTask2.Repositories;
using Interview_TakeawayTask2.Interfaces;
using InterviewTakeawayTask2.Models;
using InterviewTakeawayTask2.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace SampleTest
{
    public class ExampleTests
    {
        private readonly ITestOutputHelper output;
        private UserRepository _userRepo;
        private UserController UserController;

        public ExampleTests(ITestOutputHelper output)
        {
            this.output = output;
            _userRepo = new UserRepository();

            //pop concrete user. This is just cheap and quick, wanted to show some unit tests.
      
            //satisfy controller constructor
            UserController = new UserController(_userRepo);

        }

        [Fact]
        public void UpdateUserEndpoint_WorksAsExpectedUpdatingToBlankEmailAndChangePassword()
        {
            // Arrange
            var user = new UserRequest
            {
                Id = 1,
                Password = "Password",
                NewPassword = "SneakyPanda89!",
                Username = "alice@example.com",
                Name = "Alice",
                Email = ""
            };

            // Act
            var result = UserController.UpdateUser(user);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var updatedUser = okResult.Value as User;
            Assert.NotNull(updatedUser);
            Assert.Equal("SneakyPanda89!", updatedUser.Password);
            Assert.Equal("Alice", updatedUser.Name);
            Assert.Equal("alice@example.com", updatedUser.Username);
            Assert.Equal("", updatedUser.Email); // Email should remain unchanged
        }

        [Fact]
        public void UpdateUserEndpoint_WorksAsExpectedWithoutChangingEmailAndNewUserName()
        {
            // Arrange
            var user = new UserRequest
            {
                Id = 1,
                Password = "Password",
                Username = "alice@example.com",
                NewUsername = "Alan"

            };

            // Act
            var result = UserController.UpdateUser(user);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var updatedUser = okResult.Value as User;
            Assert.NotNull(updatedUser);
            Assert.Equal("Password", updatedUser.Password);
            Assert.Equal("Alan", updatedUser.Username);
            Assert.Equal("alice@example.com", updatedUser.Email); // Email should remain unchanged
        }




    }

}
