﻿namespace Application.DTOs
{
    public abstract class EmployeeBaseDTO
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
       // public string Email { get; set; }

    }
}
