using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspMvcStarter.Models
{
    public class AllModels
    {
        
    }

    public class ForgotPassword
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Code { get; set; }
        public DateTime Time { get; set; }
    }

    public class List
    {
        public int Id { get; set; }
        public User FollowingUser { get; set; }
        public User FollowedUser { get; set; }
    }

    public class FeaturedPost
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ProfileId { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string Version { get; set; }
        public string Format { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string instgram { get; set; }
        public int Tailor { get; set; }

        public virtual List<Photo> Photos { get; set; }
    }

    public class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Format { get; set; }
        public string Caption { get; set; }
        public DateTime TimePosted { get; set; }

        public virtual User User { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public Photo Photo { get; set; }
        public DateTime TimePosted { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }

    public class Like
    {
        public int Id { get; set; }
        public Photo Photo { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int isUnread { get; set; }
        public DateTime TimePosted { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }

}