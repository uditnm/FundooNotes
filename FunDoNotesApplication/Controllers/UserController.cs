using Microsoft.AspNetCore.Mvc;
using ManagerLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;

namespace FunDoNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;
        private readonly INotesManager notesManager;
        

        public UserController(IUserManager manager, INotesManager notesManager)
        {
            this.manager = manager;
            this.notesManager = notesManager;
            
        }

        [HttpPost("Register")]

        public ActionResult UserRegister(UserRegModel model)
        {
            try
            {
                var checkReg = manager.UserRegister(model);
                if (checkReg != null)
                {
                    return Ok(new ResponseModel<UserEntity> { Status = true, Message = "Register Successful", Data = checkReg });
                }
                else
                {
                    return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Register Unsuccessful", Data = checkReg });
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
        }


        [HttpPost("Login")]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                var checkLogin = manager.Login(model);
                if (checkLogin != null)
                {
                    return Ok(new ResponseModel<string> { Status = true, Message = "Login Successfull", Data = checkLogin });

                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Login unsuccessful", Data = checkLogin });
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
        }


        [HttpPost("ForgotPassword")]
        public ActionResult ForgetPassword(string email)
        {
            try
            {
                var checkEmail = manager.ForgetPassword(email);
                if(checkEmail != null)
                {
                    return Ok(new ResponseModel<string> { Status = true, Message = "Reset Link sent to mail successfully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Reset Unsuccessfull" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPost("ResetPassword")]
        public ActionResult ResetPassword(ResetModel model)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var user = manager.ResetPassword(model,email);
                if(user)
                {
                    return Ok(new ResponseModel<UserEntity> { Status = true, Message = "Reset Successfull"});
                }
                else
                {
                    return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Reset Unsuccessfull"});
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        

    }
}
