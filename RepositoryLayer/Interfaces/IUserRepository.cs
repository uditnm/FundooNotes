using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        public UserEntity UserRegister(UserRegModel model);
        public string Login(LoginModel model);
        public string ForgetPassword(string email);
        public bool ResetPassword(ResetModel model, string email);
    }
}
