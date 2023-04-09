﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebShopApi.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}