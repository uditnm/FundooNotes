using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using System;
using System.Linq;
using System.Security.Claims;


namespace FunDoNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;

        public UserController(IUserManager manager)
        {
            this.manager = manager;
        }

        [HttpPost("Register")]

        public ActionResult UserRegister(UserRegModel model)
        {
            try
            {
                if (model.GetType().GetProperties().Select(x=>x.GetValue(model)).Any(value=>value==null))
                {
                    throw new FundoException(FundoException.ExceptionType.INVALID_INPUT);
                }
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
            catch(FundoException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception e)
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
                if (model.GetType().GetProperties().Select(x => x.GetValue(model)).Any(value => value == null))
                {
                    throw new FundoException(FundoException.ExceptionType.INVALID_INPUT);
                }
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
            catch (FundoException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception e)
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
                if (email == null)
                {
                    throw new FundoException(FundoException.ExceptionType.INVALID_INPUT);
                }
                var checkEmail = manager.ForgetPassword(email);
                if (checkEmail != null)
                {
                    return Ok(new ResponseModel<string> { Status = true, Message = "Reset Link sent to mail successfully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Reset Unsuccessfull" });
                }
            }
            catch (FundoException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
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
                if (model.GetType().GetProperties().Select(x => x.GetValue(model)).Any(value => value == null))
                {
                    throw new FundoException(FundoException.ExceptionType.INVALID_INPUT);
                }
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var user = manager.ResetPassword(model, email);
                if (user)
                {
                    return Ok(new ResponseModel<UserEntity> { Status = true, Message = "Reset Successfull" });
                }
                else
                {
                    return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Reset Unsuccessfull" });
                }
            }
            catch (FundoException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
