﻿using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class User : IUser
    {
        
        InsurewaveContext db;
        public User()
        {
            db = new InsurewaveContext();
        }
        public bool UserDetailExists(string id)
        {
            return (db.UserDetails?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
        public string LoginUser(string userId,string password)
        {
            UserDetail temp = db.UserDetails.Where(u => u.UserId == userId && u.Password == password).FirstOrDefault();
            if (temp != null)
                return temp.Role;
            else
                return "false";

        }
        public void AddUser(UserDetail userdetail)
        {
            db.UserDetails.Add(userdetail);
            db.SaveChanges();
        }
        public void AddInsurerDetails(InsurerDetail insurerdetail)
        {
             db.InsurerDetails.Add(insurerdetail);
             db.SaveChanges();

        }
        public void AddBrokerDetails(BrokerDetail brokerdetail)
        {
            db.BrokerDetails.Add(brokerdetail);
            db.SaveChanges();
        }
        public UserDetail GetUserById(string userId)
        {
            UserDetail u = db.UserDetails.Where(a => a.UserId == userId).FirstOrDefault();
            return u;
        }
        public void EditUserDetails(UserDetail ud)
        {
            UserDetail edit = GetUserById(ud.UserId);
            edit.Password = ud.Password;
            edit.FirstName = ud.FirstName;
            edit.LastName = ud.LastName;
            edit.LicenseId = ud.LicenseId;
            edit.Gender = ud.Gender;
        }
        public void ChangePassword(string Uname, string pwd)
        {
            UserDetail temp = db.UserDetails.Where(u => u.UserId == Uname).FirstOrDefault();
            temp.Password = pwd;
            db.SaveChanges();
        }
        public List<string> GetAllUserIds()
        {
            List<string> result = db.UserDetails.Select(a => a.UserId).ToList();
            return result;
        }
    }
}
