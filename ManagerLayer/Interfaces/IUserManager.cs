using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        public UserEntity UserRegister(UserRegModel model);
        public string Login(LoginModel model);
        public string ForgetPassword(string email);
        public bool ResetPassword(ResetModel model, string email);
    }
}
