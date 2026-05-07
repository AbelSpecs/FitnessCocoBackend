using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int CountryId { get; set; }
        public bool Status { get; set; }
        public string? Token { get; set; }
        

    }
}
