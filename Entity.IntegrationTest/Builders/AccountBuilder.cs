using Entity.IntegrationTest.Constants;
using Entity.IntegrationTest.Helpers;
using Entity.IntegrationTest.Models;
using System;
using System.Collections.Generic;

namespace Entity.IntegrationTest.Builders
{
    public class AccountBuilder
    {
        private AccountCreatePrarams _accountCreatePrarams;
       
        public AccountBuilder()
        {
            _accountCreatePrarams = new AccountCreatePrarams
            {
                Username = Guid.NewGuid().ToString(),
                FirstName = System.Environment.MachineName,
                LastName = System.Environment.UserDomainName,
                Address = "Vietnam",
                Email = Guid.NewGuid().ToString() + "@gmail.com",
                Roles = new List<string>
                {
                    "Basic"
                },
                Password = DefaultConstants.DefaultPassword,
                ConfirmPassword = DefaultConstants.DefaultPassword,
                PhoneNumber = RandomHelper.GetRandomPhoneNumber()
            };
        }

        public AccountBuilder WithEmail(string email)
        {
            _accountCreatePrarams.Email = email;

            return this;
        }

        public AccountBuilder WithPhoneNumber(string phoneNumber)
        {
            _accountCreatePrarams.PhoneNumber = phoneNumber;

            return this;
        }

        public AccountBuilder WithUsername(string username)
        {
            _accountCreatePrarams.Username = username;

            return this;
        }

        public AccountBuilder WithFirstName(string firstName)
        {
            _accountCreatePrarams.FirstName = firstName;

            return this;
        }

        public AccountBuilder WithLastName(string lastName)
        {
            _accountCreatePrarams.FirstName = lastName;

            return this;
        }

        public AccountBuilder WithPassword(string password)
        {
            _accountCreatePrarams.Password = password;
            _accountCreatePrarams.ConfirmPassword = password;

            return this;
        }

        public AccountCreatePrarams Build()
        {
            return _accountCreatePrarams;
        }
    }
}
