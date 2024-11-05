using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using InterviewTakeawayTask2.Models;
using InterviewTakeawayTask2.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;
using Interview_TakeawayTask2.Interfaces;

namespace InterviewTakeawayTask2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        //fairly standard controller class, should probs add an endpoint for root, and also change return types to IActionResult. That way I wont care about what kind of action result im returning. 

        private readonly UserRepository _userRepository;

        //instantiates our userRepository
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Gets All Users
        //Doesnt need to be changed really
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }

        // GET: Gets Specific Users
        //Doesnt need to be changed really
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: Creates a User
        //Added some model validation and removed the check for details.
        //All fields are marked as required, so we dont need to check if they are null or not. We can just check if the model is valid or not.
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)

        {
            if(!ModelState.IsValid)
            {
                return BadRequest(CreateErrorList(ModelState));
            }
            else
            {
                //try and uphold the same ruling as earlier... if we dont supply an email, it should be "" not null. Smelly as repeated, but mentioned it could be abstracted...
                if (user.Email != "default")
                {        
                    if (user.Email == string.Empty || IsValidEmail(user.Email))
                    {
                        user.Email = "";
                    }
                    else
                    {
                        return BadRequest("The E-mail field is present in the body, but is neither empty or a valid e-mail address.");
                    }
                }
                //was trying to pass the age and email in despite the fact they could be null/missing.              
                var newUser = _userRepository.CreateUser(user.Name!, user.Password!, user.Email, user.Age);
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
            }
                        
        }


        // PUT: Updates a User

        //IMPORTANT!!!! This action method will not work with form-urlencoded typed body. I was debugging with Postman. 
        //If I wanted to also have an endpoint , would need another action method, that both share common property setting logic. Have commented it below.
        //Would really depend on the scope and requirements of the endpoint, but we could always just ensure we are sending JSON shaped requests (MIME).

        [HttpPut("UpdateUser")]
        //initially was pulling the id from the routing, should instead populate the user solely using the body, as is standard with POST and PUT requests.
        public IActionResult UpdateUser([FromBody] UserRequest requestingUser)
        {
            //We should check the model before doing any work, we can raise any errors. I wont check the ID during the model binding process however, as it offends SRP.
            //this is something I should check in the action method. I could add custom validation logic via IValidatableObject, but I always feel Model classes should soley be for modeling our business logic,
            //extensive validation should be done by commonly used components in our action methods.

            //lets first check the model to see if it is valid or not...
            if(!ModelState.IsValid)
            {
                //this wont print me a nice list on my Controller derived return types, as the MIME type is JSON, would need to inject a TextPlainFormatter or something similar to get a nice list.
                return BadRequest(CreateErrorList(ModelState));
            }
            //We need to check if the user exists
            else if (_userRepository.CheckUserExists(requestingUser.Id))
            {
                var matchedUser = _userRepository.GetUserById(requestingUser.Id);

                //We need to check if the user exists, and if the username and password match the user we are trying to update.
                if (matchedUser.Username != requestingUser.Username)
                {
                    return BadRequest($"Username supplied does not match for user {matchedUser.Username} (ID: {matchedUser.Id})");
                }
                else if (matchedUser.Password != requestingUser.Password)
                {
                    return BadRequest($"Password supplied does not match for user {matchedUser.Username} (ID: {matchedUser.Id})");
                }
                else
                {
                    //Check email first to avoid uneccessary processing time.

                    //Had an issue with the email part. May of been overthinking it. If the email can be "empty", does this mean "null"?
                    //during model binding, the password could either be "something" or "null". So really this signifies there is a
                    //difference between the two. If I didnt want to update the password, "empty" in this context was still "null",
                    //meaning even if the email field was ommited from the body, it would still update the password to nothing.
                    //I needed to say, that the email needs to be something, wether thats empty ie "" or a proper email. FOCUSED TOO MUCH ON NULLABLE!!

                    //circumvented this by ensuring the request object had a default value of "default" for the email field, and then checking if it was "default" or not.
                    //Could use an ENUM here for a hierarchy of default values....
                    if (requestingUser.Email != "default")
                    {
                        if (requestingUser.Email == string.Empty || IsValidEmail(requestingUser.Email))
                        {
                            matchedUser.Email = requestingUser.Email;
                        }
                        else
                        {
                            return BadRequest("The E-mail field is present in the body, but is neither empty or a valid e-mail address.");
                        }
                    }

                    // Update the user's properties with the new values, few ways to do this. Have set each one manually, could use another shared mechanism to do this.
                    matchedUser.Username = requestingUser.NewUsername ?? matchedUser.Username;
                    matchedUser.Password = requestingUser.NewPassword ?? matchedUser.Password;
                    matchedUser.Name = requestingUser.Name ?? matchedUser.Name;
                    matchedUser.Age = requestingUser.Age ?? matchedUser.Age;
                }              
                //perhaps need a different IActionResult?
                return Ok(matchedUser);
            }
            else
            {
                //this whole mechanism could also probably be abstracted (ie checking users)
                return NotFound("User not found.");
            }

        }
        // Add a method in the UserController class to create an error list and return it as a joined string || Would probably want to inherit from a bespoke baseController
        // that extends the normal one or similar way to duplicate this functionality across all controllers...
        private string CreateErrorList(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return string.Join(", ", errors);
        }

        //yet again, could probably move this to somewhere more common. Chose to ommit this check from model validation as it would offend my method of check if it has been supplied.
        private bool IsValidEmail(string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }

        //afformentioned comments. Main method was updated after the fact.

        //[HttpPut("{id}")]
        //public ActionResult<User> UpdateUser([FromRoute] int id, [FromBody] UserRequest user)
        //{
        //    var existingUser = _userRepository.GetUserById(id);
        //    if (existingUser == null)
        //    {
        //        return NotFound();
        //    }

        //    UpdateUserProperties(existingUser, user);

        //    return Ok(existingUser);
        //}

        //[HttpPut("{id}")]
        //[Consumes("application/x-www-form-urlencoded")]
        //public ActionResult<User> UpdateUserFormUrlEncoded([FromRoute] int id, [FromForm] UserRequest user)
        //{
        //    var existingUser = _userRepository.GetUserById(id);
        //    if (existingUser == null)
        //    {
        //        return NotFound();
        //    }

        //    UpdateUserProperties(existingUser, user);

        //    return Ok(existingUser);
        //}

        //private void UpdateUserProperties(User existingUser, UserRequest user)
        //{
        //    existingUser.Name = user.Name;
        //    existingUser.Password = user.Password;
        //    existingUser.Email = user.Email;
        //    existingUser.Age = user.Age;
        //}


    }
}
