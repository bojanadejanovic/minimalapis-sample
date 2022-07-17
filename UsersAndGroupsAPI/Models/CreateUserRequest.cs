﻿using Newtonsoft.Json;

namespace UsersAndGroupsAPI.Models
{
    public class CreateUserRequest
    {
        public string? Name { get; set; }

        public int GroupId { get; set; }
        
        public string? Email { get; set; }

        public string? Password { get; set; }
        
    }
}
