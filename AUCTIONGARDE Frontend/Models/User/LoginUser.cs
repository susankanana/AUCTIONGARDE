﻿using System.ComponentModel.DataAnnotations;

namespace AUCTIONGARDE_Frontend.Models.User
{
    public class LoginUser
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        
    }
}
