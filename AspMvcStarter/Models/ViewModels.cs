using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AspMvcStarter.Models
{
    public class ViewModels
    {
        public string encryptPassword(string value)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(value));
                return Convert.ToBase64String(data);
            }
        }

        public string generateActivationCode()
        {
            Random rnd = new Random();
            return rnd.Next(10000, 99999).ToString();
        }

    }

    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [Display(Name = "Compare Password")]
        public string Compare_Password { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Phone { get; set; }

        public bool Tailor { get; set; }



        FashionContext database = new FashionContext();
        ViewModels vm = new ViewModels();

        public int AddUser(RegisterViewModel Register)
        {
            var UserEmailExist = database.Users.Where(a => a.Email.Equals(Register.Email)).FirstOrDefault();
            var UserPhoneExist = database.Users.Where(a => a.Phone.Equals(Register.Phone)).FirstOrDefault();
            var UsernameExist = database.Users.Where(a => a.Username.Equals(Register.Username)).FirstOrDefault();
            if (UsernameExist == null)
            {
                if (UserEmailExist == null)
                {
                    if (UserPhoneExist == null)
                    {
                        User newUser = new User();
                        newUser.Username = Register.Username;
                        newUser.Email = Register.Email;
                        newUser.Phone = Register.Phone;
                        newUser.Password = vm.encryptPassword(Register.Password);
                        if (Register.Tailor)
                        {
                            newUser.Tailor = 1;
                        }
                        database.Users.Add(newUser);
                        database.SaveChanges();
                        return 1; //successful
                    }
                    return 2;//phone number already exist
                }
                return 3;//email already exist
            }
            return 4; //username already exist
        }
    }

    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Email / Username")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        FashionContext database = new FashionContext();
        ViewModels vm = new ViewModels();

        public bool LoginUser(LoginViewModel ValidModel)
        {
            var password = vm.encryptPassword(ValidModel.Password);
            var User = database.Users.Where(a => (a.Email == ValidModel.Email || a.Username == ValidModel.Email) && a.Password == password).FirstOrDefault();
            if (User != null)
            {
                return true;

            }
            return false;
        }
    }

    public class EditProfileViewModel
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Username { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string instagram { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

    }

    public class BigProfileViewModel
    {
        public User User { get; set; }
        public EditProfileViewModel EPVM { get; set; }
        public List<List> List { get; set; }
    }

    public class BigTimelineViewModel
    {
        public User User { get; set; }
        public List<Photo> Photo { get; set; }
    }

    public class changeProfilePictureViewModel
    {
        public HttpPostedFileWrapper ProfilePicture { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class  NewPasswordViewModel
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required][Display(Name ="Confirm Password")][Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
