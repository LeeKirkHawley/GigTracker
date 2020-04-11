using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using GigTracker.Entities;
using GigTracker.Helpers;

namespace GigTracker.Services {
    public interface IUserService {
        GigTrackerUser Authenticate(string username, string password);
        IEnumerable<GigTrackerUser> GetAll();
        GigTrackerUser GetById(int id);
    }

    public class UserService : IUserService {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        //private List<User> _users = new List<User>
        //{
        //    new User { Id = 1, FirstName = "Admin", LastName = "User", UserName = "admin", Password = "admin", Role = Role.Admin },
        //    new User { Id = 2, FirstName = "Normal", LastName = "User", UserName = "user", Password = "user", Role = Role.User }
        //};

        private readonly AppSettings _appSettings;
        private readonly AccountService _accountService;

        public UserService(IOptions<AppSettings> appSettings, AccountService accountService) {
            _appSettings = appSettings.Value;
            _accountService = accountService;
        }

        public GigTrackerUser Authenticate(string username, string password) {
            var user = this.GetAll().Where(u => u.UserName == username && u.Password == password).FirstOrDefault();

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public IEnumerable<GigTrackerUser> GetAll() {
            IQueryable<GigTrackerUser> Users = new List<GigTrackerUser> {
                new GigTrackerUser { UserName = "kirkhawley", Password = _accountService.HashPwd("password"), FirstName = "Kirk", LastName = "Hawley", Email = "leekirkhawley@gmail.com"},
                new GigTrackerUser { UserName = "leonredbone", Password = _accountService.HashPwd("password"), FirstName = "Leon", LastName = "Redbone", Email = "leekirkhawley@gmail.com"}
            }.AsQueryable<GigTrackerUser>();
            return Users;

//            return this.GetAll().WithoutPasswords();
        }

        public GigTrackerUser GetById(int id) {
            var user = this.GetAll().FirstOrDefault(x => x.Id == id);
            return user.WithoutPassword();
        }
    }
}