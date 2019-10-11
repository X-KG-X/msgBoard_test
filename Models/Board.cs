using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace msgBoard.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}


        [Display(Name="First Name:")]
        [MinLength(4,ErrorMessage="Dude your FirstName is longer than 4 characters....commmon!")]
        [Required(ErrorMessage="FirstName is a must Yo!!!")]
        public string FirstName{get;set;}


        [Display(Name="Last Name:")]
        [MinLength(4,ErrorMessage="Dude your LastName is longer than 4 characters....commmon!")]
        [Required(ErrorMessage="LastName is a must Yo!!!")]
        public string LastName{get;set;}


        [Display(Name="Email:")]
        [EmailAddress(ErrorMessage="Use the proper email format yo! It is 2019 for godsake!!!")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Required(ErrorMessage="Email is a must sorry :(")]
        public string Email{get;set;}


        [Display(Name="Password:")]
        [MinLength(8,ErrorMessage="Password needs to be more than 8 characters please")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Of course password is required?")]
        public string Password{get;set;}


        [NotMapped]
        [Display(Name="Confirm Password:")]
        [Compare(nameof(Password), ErrorMessage="Password don't match.")]
        [Required(ErrorMessage="Password Confirmation is required please!")]
        public string RePassword{get;set;}

        public DateTime CreatedAt{get;set;}=DateTime.Now;
        public DateTime UpdatedAt{get;set;}=DateTime.Now;

        //Nagivation properties for related Message anf Comment objects
        public List<Message> MsgPosted {get;set;}
        public List<Comment> CmtPosted {get;set;}
    }

    public class Message
    {
        [Key]
        public int MessageId{get;set;}
        [Required(ErrorMessage="Please your message, It is required!!!")]
        public string msgContent{get;set;}
        public int UserId{get;set;}
        public DateTime CreatedAt{get;set;}=DateTime.Now;
        public DateTime UpdatedAt{get;set;}=DateTime.Now;
        // Navigation property for related User objects
        public User msgCreator {get;set;}
        public List<Comment> msgComments{get;set;}
    }

    public class Comment
    {
        [Key]
        public int CommentId{get;set;}
        [Required(ErrorMessage="Same with comments too! Please enter your comments here.")]
        public string cmtContent {get;set;}
        public int UserId{get;set;}
        public int MessageId{get;set;}
        public DateTime CreatedAt{get;set;}=DateTime.Now;
        public DateTime UpdatedAt{get;set;}=DateTime.Now;

        //Navigation
        public User cmtCreator{get;set;}
        public Message cmtedMsg{get;set;}

    }
    public class Login
    {
        [Display(Name="Email:")]
        [EmailAddress(ErrorMessage="Use the proper email format yo! It is 2019 for godsake!!!")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Required(ErrorMessage="Email is a must sorry :(")]
        public string Email{get;set;}

        [Display(Name="Password:")]
        [Required(ErrorMessage="Of course password is required?")]
        public string Password{get;set;}
    }   
    
     public class IndexViewModel
    {
        public Login NewLogin {get;set;}
        public User NewUser {get;set;}
    }

    public class MsgCmt
    {
        public Message NewMsg {get;set;}
        public Comment NewCmt {get;set;}
    }

}