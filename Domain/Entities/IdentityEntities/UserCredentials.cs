using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Models
{
    [NotMapped]
    public class UserCredentials
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string EmailAddress { get; set; }


    }
}
