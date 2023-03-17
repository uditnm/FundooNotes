using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class UserManager: IUserManager
    {
        private readonly IUserRepository repository;

        public UserManager(IUserRepository repository)
        {
            this.repository = repository;
        }

        public UserEntity UserRegister(UserRegModel model)
        {
            return repository.UserRegister(model);
        }

        public string Login(LoginModel model)
        {
            return repository.Login(model);
        }

        public string ForgetPassword(string email)
        {
            return repository.ForgetPassword(email);
        }
        public bool ResetPassword(ResetModel model, string email)
        {
            return repository.ResetPassword(model, email);
        }
    }
}
