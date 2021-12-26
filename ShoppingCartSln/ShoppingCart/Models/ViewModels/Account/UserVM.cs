using ShoppingCart.Models.DataModels;
using ShoppingCart.Models.SupperModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.ViewModels.Account
{
    public class UserVM : NTUser
    {
        public UserVM()
        { }

        public UserVM(User user)
        {
            Id = user.Id;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            Email = user.Email;
            Username = user.Username;
            Password = user.Password;
            Mobile = user.Mobile;
        }

        public string Fullname { get { return Firstname + " " + Lastname; } }

        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "This field should be 8 min & 50 max characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}