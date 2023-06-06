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
    [Table("token")]
    public class Token
    {
        [Column("token_id")]
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]

        public int Id { get; set; }
        [Column("access_token")]
        public required string AccessToken { get; set; }
        [Column("refresh_token")]
        public required string RefreshToken { get; set; }
    }
}
