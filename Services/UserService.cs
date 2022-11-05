using DS2022_30442_Presecan_Alexandru_Assignment_1.Data;
using DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1.Services
{
    public class UserService
    {
        private readonly DataContext _db;

        private readonly string _key = "This is a sample secret key - please don't use in production environment.'";
        private readonly string _emailPattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

        public UserService(DataContext db)
        {
            _db = db;
        }

        public IEnumerable<UserDisplayDTO> GetUsers() => 
            _db.Users
            .Include(user => user.Devices)
            .Select(user => new UserDisplayDTO(user));

        public UserDisplayDTO GetUserById(int id) => 
            GetUsers()
            .First(user => user.Id == id);

        public UserDisplayDTO GetUserByName(string userName) => 
            GetUsers()
            .First(user => user.UserName == userName);

        public UserAuthenticationDTO AuthenicateUser(UserDTO userDTO)
        {
            if (userDTO.UserName == null || userDTO.Password == null)
                throw new Exception("Username and password cannot be empty");

            User user = _db.Users.First(user => user.UserName == userDTO.UserName);

            if (user == null)
                throw new Exception("Invalid username");

            if (user.PasswordHash != Encoding.ASCII.GetString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(userDTO.Password))))
                throw new Exception("Invalid password");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                (   
                    new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return new UserAuthenticationDTO(user, stringToken);
        }

        public UserDisplayDTO CreateUser(UserDTO userDTO)
        {
            if (userDTO.Email == null || !Regex.Match(userDTO.Email.Trim(), _emailPattern, RegexOptions.IgnoreCase).Success)
                throw new Exception("Invalid email address");

            if (userDTO.UserName == null || GetUserByName(userDTO.UserName) != null)
                throw new Exception("Invalid username");

            if (userDTO.Password == null)
                throw new Exception("Password cannot be empty");

            if (userDTO.Password != userDTO.ConfirmPassword)
                throw new Exception("Passwords do not match");

            User user = new User()
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                PasswordHash = Encoding.ASCII.GetString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(userDTO.Password))),
                Role = userDTO.Role
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return GetUserById(user.Id);
        }

        public UserDisplayDTO UpdateUser(int id, UserDTO userDTO)
        {
            User user = _db.Users.First(user => user.Id == id);

            if (user == null)
                throw new Exception("User not found");

            if (userDTO.Email == null || !Regex.Match(userDTO.Email.Trim(), _emailPattern, RegexOptions.IgnoreCase).Success)
                throw new Exception("Invalid email address");

            if (userDTO.UserName == null || GetUserByName(userDTO.UserName) != null)
                throw new Exception("Invalid username");

            if (userDTO.Password == null || user.PasswordHash != Encoding.ASCII.GetString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(userDTO.Password))))
                throw new Exception("Invalid password");

            if (userDTO.NewPassword == null)
                throw new Exception("Password cannot be empty");

            if (userDTO.NewPassword != userDTO.ConfirmPassword)
                throw new Exception("Passwords do not match");

            user.UserName = userDTO.UserName;
            user.Email = userDTO.Email;
            user.PasswordHash = Encoding.ASCII.GetString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(userDTO.NewPassword)));
            user.Role = userDTO.Role;

            _db.Users.Update(user);
            _db.SaveChanges();

            return GetUserById(user.Id);
        }

        public string DeleteUser(int id)
        {
            User user = _db.Users.First(user => user.Id == id);

            if (user == null)
                throw new Exception("User not found");

            _db.Users.Remove(user);
            _db.SaveChanges();

            return "User deleted";
        }
    }
}
