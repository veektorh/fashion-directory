using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspMvcStarter.Models;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;


namespace AspMvcStarter.Controllers
{
    public class AccountController : Controller
    {
        FashionContext database = new FashionContext();
        cloudinaryService cloudinaryService = new cloudinaryService("318747541398173", "qNJqtPYJxQcVJguPmxPqWTSKC90", "bolum-victor");

        public ActionResult TestCloudinary(HttpPostedFileBase file)
        {
            var result = cloudinaryService.UploadImage(file);

            return Json(result);
        }
        public class likemodel
        {
            public int id { get; set; }
            public int no { get; set; }

        }
        public ActionResult unLikePicture(int id)
        {
            var photo = database.Photos.Where(a => a.Id == id).FirstOrDefault();
            var loggedInUserId = getLoggedInUser().Id;
            var unlike = database.Likes.Where(a => a.Photo.Id == id && a.Sender.Id == loggedInUserId).FirstOrDefault();
            database.Likes.Remove(unlike);
            database.SaveChanges();
            var noOfLikes = database.Likes.Where(a => a.Photo.Id == id).Count();
            var newlikemodel = new likemodel() { id = id, no = noOfLikes };
            return PartialView("_unLikePage", newlikemodel);
        }

        public ActionResult LikePicture(int id)
        {
            var photo = database.Photos.Where(a => a.Id == id).FirstOrDefault();
            var newlikes = new Like();
            newlikes.Photo = photo;
            newlikes.Sender = getLoggedInUser();
            newlikes.Receiver = photo.User;
            database.Likes.Add(newlikes);
            database.SaveChanges();
            var noOfLikes = database.Likes.Where(a => a.Photo.Id == id).Count();
            var newlikemodel = new likemodel() { id = id , no = noOfLikes };
            return PartialView("_LikePage",newlikemodel);
        }
        // GET: Account
        public ActionResult Index()
        {
            if (Request.Cookies["user"] != null)
            {
                var username = Request.Cookies["username"].Value;
                var LoggedInUser = database.Users.Where(a => a.Username == username).FirstOrDefault();
                
                if (LoggedInUser != null)
                {
                    var photoLists =
                       from photos in database.Photos
                       join lists in database.Lists on photos.User.Id equals lists.FollowedUser.Id
                       where lists.FollowingUser.Id == LoggedInUser.Id
                       orderby photos.Id descending
                       select photos;

                    ViewBag.Timeline = photoLists;
                    ViewBag.LoggedInUser = LoggedInUser;
                    var bigmodel = new BigTimelineViewModel()
                    {
                        User = LoggedInUser,
                        Photo = photoLists.ToList(),
                    };
                    return View(bigmodel);
                }

            }

            return HttpNotFound();
        }

        public User getLoggedInUser()
        {
            if (Request.Cookies["user"] != null)
            {
                string email = Request.Cookies["user"].Value;
                var LoggedInUser = database.Users.Where(a => a.Email == email).FirstOrDefault();
                return LoggedInUser;
            }
            return null;
        }
        public class picAndComments
        {
            public Photo Photo { get; set; }
            public List<Comment> Comment { get; set; }

        }
        public ActionResult InsertComments(int id , string body)
        {
            var photo = database.Photos.Where(a => a.Id == id).FirstOrDefault();
            var newcomment = new Comment();
            newcomment.TimePosted = DateTime.Now;
            newcomment.Photo = photo;
            newcomment.Receiver = photo.User;
            newcomment.Sender = getLoggedInUser();
            newcomment.Body = body;
            database.Comments.Add(newcomment);
            database.SaveChanges();
            return Json(true);
        }

        public ActionResult GetPicAndComments(int id)
        {
            var pic = database.Photos.Where(a => a.Id == id).FirstOrDefault();
            var comments = database.Comments.Where(a => a.Photo.Id == pic.Id).ToList();
            var picAndComments = new picAndComments() { Photo = pic, Comment = comments };
            return PartialView(picAndComments);
        }

        public ActionResult Login()
        {
            ViewBag.Status = TempData["newPasswordstatus"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel ValidModel)
        {
            var Lvm = new LoginViewModel();
            if (ModelState.IsValid)
            {
                //send user input to login view model for authentication
                var result = Lvm.LoginUser(ValidModel);

                if (result)
                {
                    var User = database.Users.Where(a => a.Email == ValidModel.Email || a.Username == ValidModel.Email).FirstOrDefault();
                    HttpCookie userCookie = new HttpCookie("user", User.Email);
                    var usernamecookie = new HttpCookie("username", User.Username);
                    userCookie.Expires = DateTime.MaxValue;
                    usernamecookie.Expires = DateTime.MaxValue;
                    HttpContext.Response.SetCookie(userCookie);
                    HttpContext.Response.SetCookie(usernamecookie);

                    return RedirectToAction("Profile", new { username = User.Username });
                }
                else
                {
                    ViewBag.Status = "error";

                }
            }
            return View();
        }

        public ActionResult register()
        {
            ViewBag.Status = TempData["status"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult register(RegisterViewModel validmodel)
        {
            var Rvm = new RegisterViewModel();
            if (ModelState.IsValid)
            {
                var result = Rvm.AddUser(validmodel);
                if (result == 1)
                {
                    TempData["status"] = "success";
                    return RedirectToAction("register");
                }
                else if (result == 2)
                {
                    ViewBag.Status = "Phone Number";
                }
                else if (result == 3)
                {
                    ViewBag.Status = "Email Address";
                }
                else
                {
                    ViewBag.Status = "Username";
                }
            }
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel ValidModel)
        {
            if (Request.Cookies["user"] != null)
            {
                if (ModelState.IsValid)
                {
                    ViewModels vm = new ViewModels();
                    var oldPassword = vm.encryptPassword(ValidModel.CurrentPassword);
                    var username = Request.Cookies["username"].Value;
                    var LoggedInUser = database.Users.Where(a => a.Username == username && a.Password == oldPassword).FirstOrDefault();
                    if (LoggedInUser != null)
                    {
                        var newUser = new User();
                        newUser.Password = ValidModel.ConfirmNewPassword;
                        database.SaveChanges();
                        ViewBag.Status = "success";
                        return View();
                    }
                    ViewBag.Status = "error";
                }
                return View();
            }
            return RedirectToAction("login");
        }

        public ActionResult ForgotPassword()
        {
            ViewBag.status = TempData["forgotPasswordStatus"];
            ViewBag.email = TempData["email"];
            return View();
        }

        public ActionResult send_activation_code(string email)
        {
            var vm = new ViewModels();
            var user = database.Users.Where(a => a.Email == email).FirstOrDefault();
            if (user != null)
            {
                var forgotPassword = database.ForgotPasswords.Where(a => a.User.Id == user.Id).FirstOrDefault();
                if (forgotPassword != null)
                {
                    //delete already existing entry
                    database.ForgotPasswords.Remove(forgotPassword);
                    database.SaveChanges();
                }
                var newEntry = new ForgotPassword();
                newEntry.User = user;
                newEntry.Code = vm.generateActivationCode();
                newEntry.Time = DateTime.Now;
                database.ForgotPasswords.Add(newEntry);
                database.SaveChanges();
                //send email to user containing the activation code
                TempData["forgotPasswordStatus"] = "newconfirmation";
                return RedirectToAction("forgotpassword");
            }
            TempData["forgotPasswordStatus"] = "ForgotPasswordError";
            return RedirectToAction("forgotpassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel validmodel)
        {
            var vm = new ViewModels();
            if (ModelState.IsValid)
            {
                var user = database.Users.Where(a => a.Email == validmodel.Email).FirstOrDefault();
                if (user != null)
                {
                    var forgotPassword = database.ForgotPasswords.Where(a => a.User.Id == user.Id).FirstOrDefault();
                    if (forgotPassword != null)
                    {
                        //delete already existing entry
                        database.ForgotPasswords.Remove(forgotPassword);
                        database.SaveChanges();
                    }
                    var newEntry = new ForgotPassword();
                    newEntry.User = user;
                    newEntry.Code = vm.generateActivationCode();
                    newEntry.Time = DateTime.Now;
                    database.ForgotPasswords.Add(newEntry);
                    database.SaveChanges();
                    //------------------------------
                    //send email to user containing the activation code and username
                    //------------------------------
                    TempData["email"] = validmodel.Email;
                    TempData["forgotPasswordStatus"] = "success";
                    ViewBag.email = validmodel.Email;
                    return RedirectToAction("forgotpassword");

                }
                ViewBag.status = "error";
            }
            return View();
        }

        public ActionResult AuthenticateAccount()
        {
            ViewBag.email = TempData["email"];
            return View();
        }

        public ActionResult ResetEmail()
        {
            return View();
        }

        public ActionResult NewPassword(string username, string code)
        {
            var user = database.Users.Where(a => a.Username == username).FirstOrDefault();
            var forgotPassword = database.ForgotPasswords.Where(a => a.User.Id == user.Id).FirstOrDefault();
            if (forgotPassword != null)
            {
                var datediff = Convert.ToInt16((DateTime.Now - forgotPassword.Time).ToString().Substring(0, 2)); //get the date difference
                if (code == forgotPassword.Code)
                {
                    if (datediff < 25)
                    {
                        database.ForgotPasswords.Remove(forgotPassword);
                        database.SaveChanges();
                        ViewBag.username = user.Username;
                        return View();
                    }
                    TempData["forgotPasswordStatus"] = "expired";
                    TempData["email"] = user.Email;
                    return RedirectToAction("ForgotPassword");
                }
                TempData["forgotPasswordStatus"] = "invalid";
                return RedirectToAction("ForgotPassword");
            }
            TempData["forgotPasswordStatus"] = "ForgotPasswordError";
            return RedirectToAction("ForgotPassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPassword(NewPasswordViewModel validmodel)
        {
            if (ModelState.IsValid)
            {
                var vm = new ViewModels();
                var username = validmodel.username;
                var user = database.Users.Where(a => a.Username == username).FirstOrDefault();
                user.Password = vm.encryptPassword(validmodel.ConfirmPassword);
                database.SaveChanges();
                TempData["newPasswordstatus"] = "success";
            }
            return RedirectToAction("login");
        }


        public JsonResult JsonIsUserFollowing(string username)
        {
            if (IsUserFollowing(username))
            {
                return Json(true);
            }
            return Json(false);
        }

        [ChildActionOnly]
        public bool IsUserFollowing(string username)
        {
            if (Request.Cookies["user"] != null)
            {
                var userProfile = database.Users.Where(a => a.Username == username).FirstOrDefault();
                string email = Request.Cookies["user"].Value;
                User LoggedInUser = new User();
                LoggedInUser = database.Users.Where(a => a.Email == email).FirstOrDefault();
                var CheckIfIsFollowing = database.Lists.Where(a => a.FollowingUser.Id == LoggedInUser.Id && a.FollowedUser.Id == userProfile.Id).FirstOrDefault();
                if (CheckIfIsFollowing != null)
                {
                    ViewBag.isFollowing = true;
                    return true;
                }
            }
            return false;
        }

        public ActionResult profile(string username)
        {
            if (username != null)
            {
                TempData["username"] = username;
                ViewBag.ProfileOwner = false;
                ViewBag.isLoggedIn = false;
                User LoggedInUser = new User();
                var userProfile = database.Users.Where(a => a.Username == username).FirstOrDefault();

                if (userProfile != null)
                {
                    if (Request.Cookies["user"] != null)
                    {
                        string email = Request.Cookies["user"].Value;
                        LoggedInUser = database.Users.Where(a => a.Email == email).FirstOrDefault();
                        if (LoggedInUser != null)
                        {
                            ViewBag.isLoggedIn = true;
                            var CheckIfIsFollowing = database.Lists.Where(a => a.FollowingUser.Id == LoggedInUser.Id && a.FollowedUser.Id == userProfile.Id).FirstOrDefault();
                            if (IsUserFollowing(username))
                            {
                                ViewBag.isFollowing = true;
                            }
                            else
                            {
                                ViewBag.isFollowing = false;
                            }

                            if (!IsUserFollowing(LoggedInUser.Username))
                            {
                                addToList(LoggedInUser.Username);
                            }
                        }
                    }

                    //get list of people the logged in user is following
                    var FollowingLists = database.Lists.Include("FollowedUser").Where(a => a.FollowingUser.Id == LoggedInUser.Id).ToList();

                    if (LoggedInUser == userProfile)
                    {
                        ViewBag.ProfileOwner = true;
                    }

                    //get the photos of the logged in user
                    ViewBag.PictureUploads = database.Photos.Where(a => a.User.Id == userProfile.Id).ToList();


                    ViewBag.Following = database.Lists.Include("FollowedUser").Where(a => a.FollowingUser.Id == userProfile.Id).ToList().Count;
                    ViewBag.Followers = database.Lists.Include("FollowedUser").Where(a => a.FollowedUser.Id == userProfile.Id).ToList().Count - 1;
                    ViewBag.Posts = database.Photos.Where(a => a.User.Id == userProfile.Id).ToList().Count;

                    if (userProfile.Picture != null)
                    {
                        ViewBag.PicturePublicId = userProfile.Picture;
                        ViewBag.Version = userProfile.Version;
                        ViewBag.Format = userProfile.Format;
                    }
                    else
                    {
                        ViewBag.PictureUrl = "../images/blank-profile-picture.png";
                    }

                    var EditProfile = new EditProfileViewModel();

                    var bigmodel = new BigProfileViewModel()
                    {
                        User = userProfile,
                        EPVM = EditProfile,
                        List = FollowingLists
                    };
                    return View(bigmodel);
                }

            }


            return HttpNotFound();
        }

        public ActionResult Edit()
        {
            if (Request.Cookies["user"] != null)
            {
                string email = Request.Cookies["user"].Value;
                var currentUser = database.Users.Where(a => a.Email == email).FirstOrDefault();
                var EditUser = new EditProfileViewModel();
                EditUser.Bio = currentUser.Bio;
                EditUser.Location = currentUser.Location;
                EditUser.Name = currentUser.Name;
                EditUser.Username = currentUser.Username;
                EditUser.Phone = currentUser.Phone;
                EditUser.facebook = currentUser.facebook;
                EditUser.twitter = currentUser.twitter;
                EditUser.instagram = currentUser.instgram;
                return Json(EditUser, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("login");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditProfileViewModel validModel)
        {
            string email = Request.Cookies["user"].Value;
            var currentUser = database.Users.Where(a => a.Email == email).FirstOrDefault();
            currentUser.Name = validModel.Name;
            currentUser.Username = validModel.Username;
            currentUser.Phone = validModel.Phone;
            currentUser.Location = validModel.Location;
            currentUser.Bio = validModel.Bio;
            currentUser.facebook = validModel.facebook;
            currentUser.twitter = validModel.twitter;
            currentUser.instgram = validModel.instagram;
            database.SaveChanges();
            return RedirectToAction("profile", new { username = currentUser.Username });
        }


        public ActionResult UploadProfilePicture(HttpPostedFileBase file)
        {
            if (Request.Cookies["user"] != null)
            {
                string email = Request.Cookies["user"].Value;
                var currentUser = database.Users.Where(a => a.Email == email).FirstOrDefault();
                if (file != null)
                {
                    if (Path.GetExtension(file.FileName).ToLower() == ".jpg" || Path.GetExtension(file.FileName).ToLower() == ".png"
                        || Path.GetExtension(file.FileName).ToLower() == ".jpeg")
                    {
                        string fullPath = Server.MapPath("/images/users/" + currentUser.Picture);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        var result = cloudinaryService.UploadImage(file);
                        if (result != null)
                        {
                            currentUser.Picture = result.PublicId;
                            currentUser.Version = "v"+result.Version;
                            currentUser.Format = result.Format;
                        }


                            var picturepath = Server.MapPath("/images/users/" + currentUser.Id + "__" + Path.GetExtension(file.FileName).ToLower());
                        //currentUser.Picture = "../images/users/" + currentUser.Id + "__" + Path.GetExtension(file.FileName).ToLower();
                        //file.SaveAs(picturepath);
                        database.SaveChanges();

                    }
                }
                return RedirectToAction("profile", new { username = currentUser.Username });
            }
            return RedirectToAction("login");
        }

        public ActionResult UploadNewPhoto(HttpPostedFileBase file, string caption)
        {
            if (Request.Cookies["user"] != null)
            {
                string email = Request.Cookies["user"].Value;
                var currentUser = database.Users.Where(a => a.Email == email).FirstOrDefault();
                if (file != null)
                {
                    if (Path.GetExtension(file.FileName).ToLower() == ".jpg" || Path.GetExtension(file.FileName).ToLower() == ".png"
                        || Path.GetExtension(file.FileName).ToLower() == ".jpeg")
                    {
                        var result = cloudinaryService.UploadImage(file);
                        if (result != null)
                        {
                            //var lastPhotoId = database.Photos.Count() + 1;
                            //var picturepath = Server.MapPath("/images/uploads/" + lastPhotoId + "__" + Path.GetExtension(file.FileName).ToLower());
                            //var pictureName = "../images/uploads/" + lastPhotoId + "__" + Path.GetExtension(file.FileName).ToLower();
                            Photo newPhoto = new Photo();
                            newPhoto.Name = result.PublicId;
                            newPhoto.Version = "v"+result.Version;
                            newPhoto.Format = result.Format;
                            newPhoto.User = currentUser;
                            newPhoto.Caption = caption;
                            newPhoto.TimePosted = DateTime.Now;
                            //file.SaveAs(picturepath);
                            database.Photos.Add(newPhoto);
                            database.SaveChanges();
                        }
                        

                    }

                }
                return RedirectToAction("profile", new { username = currentUser.Username });
            }
            return RedirectToAction("login");
        }


        public ActionResult removeProfilePicture(string id)
        {
            if (Request.Cookies["user"] != null)
            {
                string email = Request.Cookies["user"].Value;
                var currentUser = database.Users.Where(a => a.Email == email).FirstOrDefault();

                string fullPath = Server.MapPath("/images/users/" + currentUser.Picture);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                currentUser.Picture = null;
                database.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("login");

        }

        public ActionResult logout()
        {
            if (Request.Cookies["user"] != null)
            {
                Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("login");
        }

        public ActionResult addToList(string Username)
        {
            if (!IsUserFollowing(Username))
            {
                var ProfileOwner = database.Users.Where(a => a.Username == Username).FirstOrDefault();

                string email = Request.Cookies["user"].Value;
                var LoggedInUser = database.Users.Where(a => a.Email == email).FirstOrDefault();

                var newList = new List() { FollowingUser = LoggedInUser, FollowedUser = ProfileOwner };

                database.Lists.Add(newList);
                database.SaveChanges();
                return Json("success");
            }

            return Json("failed");

        }

        public ActionResult RemoveFromList(string Username)
        {
            if (IsUserFollowing(Username))
            {
                var ProfileOwner = database.Users.Where(a => a.Username == Username).FirstOrDefault();

                string email = Request.Cookies["user"].Value;
                var LoggedInUser = database.Users.Where(a => a.Email == email).FirstOrDefault();

                var removeList = database.Lists.Where(a => a.FollowingUser.Id == LoggedInUser.Id && a.FollowedUser.Id == ProfileOwner.Id).FirstOrDefault();

                database.Lists.Remove(removeList);
                database.SaveChanges();
                return Json("success");
            }

            return Json("failed");

        }
    }


}