using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Notification.Domain.Models
{
    [Table("user_refresh_tokens")]
    public class UserRefreshTokens
    {
        [Column("id")]

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }
        [Column("refresh_token")]
        public string RefreshToken { get; set; }
        [Column("expiration_date")]
        public DateTimeOffset ExpirationDate { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}
