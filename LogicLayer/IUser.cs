﻿
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IUser
    {
        
        //public bool LoginUser(UserDetail userdetail);
        public bool LoginUser(string userId, string password);
        public void AddUser(UserDetail userdetail);
        public void AddBrokerDetails(BrokerDetail brokerdetail);
        public void AddInsurerDetails(InsurerDetail insurerdetail);
        public UserDetail GetUserById(string userId);
        public void EditUserDetails(UserDetail ud);
        public void ChangePassword(string Uname, string pwd);
        public List<string> GetAllUserIds();
        public bool UserDetailExists(string id);
    }
}
