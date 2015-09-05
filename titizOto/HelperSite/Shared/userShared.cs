using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using titizOto.Models;

namespace HelperSite.Shared
{
    public class userShared : baseShared
    {
        public userShared(titizOtoEntities db)
        {
            this.db = db;
        }

        public registerStatu getUserRegisterStatuByEmail(string email)
        {
            var userItem = db.tbl_user.Where(a => a.email == email).FirstOrDefault();

            if (userItem == null)
            {
                return registerStatu.unregistered;
            }
            else
            {
                return (registerStatu)userItem.registerStatuId;
            }

        }

        public tbl_user getUserItemByEmailAndPassword(string email, string Md5password)
        {
            return db.tbl_user.Where(a => a.email == email && a.password == Md5password).FirstOrDefault();
        }

        public tbl_user getUserById(int userId)
        {
            return db.tbl_user.Where(a => a.userId == userId).FirstOrDefault();

        }

        public tbl_activation addActivationWithItem(string code, int userId)
        {
            tbl_activation activationItem = new tbl_activation();

            activationItem.code = code;
            activationItem.datetime = DateTime.Now;
            activationItem.userId = userId;


            db.tbl_activation.Add(activationItem);
            db.SaveChanges();


            return activationItem;

        }

        public tbl_forgetPassword addForgetPasswordWithItem(int userId)
        {

            var item = new tbl_forgetPassword();
            item.code = Guid.NewGuid().ToString();
            item.createTime = DateTime.Now;
            item.userId = userId;

            db.tbl_forgetPassword.Add(item);
            db.SaveChanges();

            return item;

        }

        public void updateUserPassword(int userId, string Md5password)
        {
            var userItem = db.tbl_user.Where(a => a.userId == userId).FirstOrDefault();
            userItem.password = Md5password;
            db.SaveChanges();

        }

        public void deleteForgetPasswordByUserId(int userId)
        {
            var itemList = db.tbl_forgetPassword.Where(a => a.userId == userId).AsEnumerable();

            foreach (var item in itemList)
            {
                db.tbl_forgetPassword.Remove(item);
            }

            db.SaveChanges();
        }

        public tbl_user getUserByGuid(string guid)
        {
          return  db.tbl_user.Where(a => a.guid == guid).FirstOrDefault();
        
        }

    }

    public enum userType
    {
        normalMember = 1,
        facebookMember = 2,
        guestUser = 3

    }

    public enum registerStatu
    {
        registered = 1,
        waitingActivation = 2,
        ban = 3,
        unregistered = 4

    }
}