using InsightCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InsightCore.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public required string Password { get; set; }
        public string? PhoneNumber { get; set; }
        [Column("CountryId")]
        public int? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country? Country { get; set; }

        [Column("CityId")]
        public int? CityId { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; }
        public string? Address { get; set; }
        public required DateTime Birthdate { get; set; }
        public int AccessFailedCount { get; set; }
        public bool Status { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpiry { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationTokenExpiry { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        // Imágenes de perfil y banner (Base64 o URL)
        public string? ProfilePicture { get; set; }
        public string? BannerPicture { get; set; }

        // Usamos BCrypt para generar un hash seguro a partir del texto plano
        public void SetSecurePassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new Exception("Password cannot be empty");

            this.Password = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public bool CheckPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword)) return false;

            // BCrypt compara el texto plano con el Hash guardado
            return BCrypt.Net.BCrypt.Verify(plainPassword, this.Password);
        }

        public void RegisterFailedLogin()
        {
            AccessFailedCount++;
            if (AccessFailedCount <= 3)
            {
                AccessFailedCount = AccessFailedCount;
                LastModified = DateTime.UtcNow; 
                LastModifiedBy = "System";
                Status = false;
            }
        }

        public void ResetAccessFailedCount()
        {
            AccessFailedCount = 0;
            LastModified = DateTime.UtcNow;
            LastModifiedBy = "System";
            Status = true;
        }
    }

}
