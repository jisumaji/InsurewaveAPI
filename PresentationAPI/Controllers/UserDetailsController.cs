﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;
using Microsoft.AspNetCore.Cors;
using LogicLayer;
using PresentationAPI.Models;

namespace PresentationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]

    public class UserDetailsController : ControllerBase
    {
        IUser user;
        public UserDetailsController( IUser obj)
        {
            user = obj; 
        }
        //Login Page calls this
        [HttpGet("{userId}+{password}")]
        public ActionResult<bool> Login(string userId, string password)
        {
            return user.LoginUser(userId, password);
        }
        //UserModel page
        [HttpPost]
        public ActionResult<string> Register(UserModel register)
        {
            if (user.GetAllUserIds().Contains(register.UserId))
                return "unavailable";
            else
            {
                UserDetail userDetail = new UserDetail()
                {
                    UserId = register.UserId,
                    Password = register.Password,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Gender = register.Gender,
                    Role = register.Role,
                    LicenseId = register.LicenseId
                };
                if (userDetail.Role.Equals("insurer"))
                {

                    InsurerDetail insert = new InsurerDetail
                    {
                        InsurerId = userDetail.UserId
                    };
                    user.AddInsurerDetails(insert);
                }
                else if (userDetail.Role.Equals("broker"))
                {
                    BrokerDetail insert = new BrokerDetail
                    {
                        BrokerId = userDetail.UserId
                    };
                    user.AddBrokerDetails(insert);
                }
                user.AddUser(userDetail);

                //return CreatedAtAction("GetUserDetail", new { id = userDetail.UserId }, userDetail);
                return "successful";
            }
        }
        //change password redirect to another page
        [HttpPut("{UserId}")]
        public ActionResult<string> ForgotPassword(string UserId, string pwd)
        {
            UserDetail ud = user.GetUserById(UserId);
            if (ud != null)
                user.ChangePassword(UserId, pwd);
            else
                return "invalidUserId";
            return "passwordChanged";
        }
        //userdetails
        [HttpGet("{userId}")]
        public ActionResult<UserDetail> Details(string userId)
        {
            if (!user.UserDetailExists(userId))
            {
                return null;
            }
            UserDetail userDetail = user.GetUserById(userId);
            userDetail.Password = null;                                                  
            return userDetail;
        }
        [HttpPut("{id}")]
        public ActionResult<string> Edit(string id, UserModel r)
        {
            if (id != r.UserId)
            {
                return "not found";
            }

            if (ModelState.IsValid)
            {
                UserDetail u = new UserDetail()
                {
                    UserId = r.UserId,
                    Password = r.Password,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Gender = r.Gender,
                    Role = r.Role,
                    LicenseId = r.LicenseId

                };
                try
                {
                    user.EditUserDetails(u);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!user.UserDetailExists(r.UserId))
                    {
                        return "not found";
                    }
                    else
                    {
                        throw;
                    }
                }
                return "success";
            }
            return "not found";
        }
    }
}
