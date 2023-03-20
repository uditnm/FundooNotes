using RepositoryLayer.Entity;
using RepositoryLayer.FundoContext;
using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace RepositoryLayer.Services
{
    public class UserRepository: IUserRepository
    {
        private readonly FundoAppContext context;
        private IConfiguration _config;
        public UserRepository(FundoAppContext context, IConfiguration config)
        {
            this.context = context;
            _config = config;
        }

        public UserEntity UserRegister(UserRegModel model)
        {
            try
            {
                UserEntity entity = new UserEntity();
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Email = model.Email;
                entity.Password = EncryptPassword(model.Password);
                var check = context.Users.Add(entity);
                context.SaveChanges();
                if (check != null)
                {
                    return entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public string EncryptPassword(string password)
        {
            try
            {
                var PlainPassword = Encoding.UTF8.GetBytes(password);
                var encodedPassword = Convert.ToBase64String(PlainPassword);
                return encodedPassword;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string Login( LoginModel model)
        {
            try
            {
                var CheckDetails = context.Users.FirstOrDefault(v => v.Email == model.Email && v.Password == EncryptPassword(model.Password));
                if (CheckDetails != null)
                {
                    var token = GenerateToken(CheckDetails.Email, CheckDetails.UserId);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        

        private string GenerateToken(string email,long id)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim(ClaimTypes.Email, email),
                new Claim("UserId",id.ToString())
            };

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Audience"],
                  claims,
                  expires: DateTime.Now.AddMinutes(15),
                  signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public string ForgetPassword(string email)
        {
            try
            {
                var checkEmail = context.Users.Where(x => x.Email == email).FirstOrDefault();
                if (checkEmail != null)
                {
                    var token = GenerateToken(checkEmail.Email, checkEmail.UserId);
                    MSMQ mSMQ= new MSMQ();
                    mSMQ.sendData2Queue(token);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ResetPassword(ResetModel model, string email)
        {
            try
            {
                if(model.NewPassword.Equals(model.ConfirmPassword))
                {
                    var checkEmail = context.Users.FirstOrDefault(x => x.Email == email);
                    if (checkEmail != null)
                    {
                        checkEmail.Password = EncryptPassword(model.NewPassword);
                        context.SaveChanges();
                        return true;
                    }
                }
                return false;
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        

    }
}
