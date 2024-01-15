using EduQuiz_5P.Data;
using EduQuiz_5P.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduQuiz_5P.Models
{
    public class UserRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public DateTime RegistrationDate { get; set; }
        public UserRegistration() {
            RegistrationDate = DateTime.UtcNow.ToTimeZone();
        }
    }
}
