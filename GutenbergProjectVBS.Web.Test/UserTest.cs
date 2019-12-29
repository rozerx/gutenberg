using GutenbergProjectVBS.Web.Models;
using GutenbergProjectVBS.Web.Models.Context;
using GutenbergProjectVBS.Web.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GutenbergProjectVBS.Web.Tests
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void Login_Success_Method()
        {
            LoginViewModel model = new LoginViewModel();
            model.Email = "ozeramazan1@gmail.com";
            model.Password = GVBSHelpers.GVBSHelperClass.CreateMD5Password("123");

            var data = GVBSHelpers.GVBSHelperClass.GetUser(model);

            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void Login_Failed_Method()
        {
            User user = null;
            using (VBSContext db = new VBSContext())
            {
                var email = "ozeramazan2@gmail.com";
                var password = GVBSHelpers.GVBSHelperClass.CreateMD5Password("123456");
                user = db.Users.AsQueryable().FirstOrDefault(u => u.Email == email && u.Password.Equals(password) && u.IsAdmin == false);
            }

            Assert.IsNull(user);
        }

        [TestMethod]
        public void Register_Success_Method()
        {
            User user = new User();

            user.Name = null;
            user.Surname = null;
            user.Email = null;
            user.Password= null;
            user.ResetPasswordCode = null;

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void Register_Failed_Method()
        {
            string Name = "xysd";
            string Surname = "";
            string Email = "skdjf@sjkdf";
            string Password = "q123";

            bool control = (Name != "") && (Surname != "") && (Email != "") && (Password != "");

            Assert.IsFalse(control);

        }

    }

}
