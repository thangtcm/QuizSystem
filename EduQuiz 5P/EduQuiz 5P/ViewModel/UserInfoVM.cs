using System.ComponentModel.DataAnnotations;
using EduQuiz_5P.Data;
using EduQuiz_5P.Enums;
using EduQuiz_5P.Helpers;
using Microsoft.AspNetCore.Authentication;

namespace EduQuiz_5P.ViewModel
{
    public class UserInfoVM
    {
        public long? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        public string? PasswordOld { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool RememberMe { get; set; }
        public double Point { get; set; }
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
        public virtual ICollection<string>? Roles { get; set; }
        public virtual ICollection<string>? Tokens { get; set; }

        public UserInfoVM()
        {
        }
        public UserInfoVM(ApplicationUser user)
        {
            this.FullName = user.FullName;
            this.UserName = user.UserName;
            this.UserId = user.Id;
            this.Gender = user.Gender;
            this.Point = user.Point;
        }

        public UserInfoVM(ApplicationUser user, List<string> roles)
        {
            this.FullName = user.FullName;
            this.UserName = user.UserName;
            this.Birthday = user.Birthday;
            this.Email = user.Email;
            this.UserId = user.Id;
            this.Roles = roles;
        }
    } 
}
