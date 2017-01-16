using System.ComponentModel.DataAnnotations;

namespace SQLiteLinqExample.resources.sqlite.Server.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        public string SocialClubName { get; set; }

        public string LastIp { get; set; }
        
        public string LastDisplayName { get; internal set; }
    }
}
