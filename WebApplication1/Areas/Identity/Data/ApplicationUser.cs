using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Required(ErrorMessage = "First name is required. ")]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [PersonalData]
    [Required(ErrorMessage = "Last name is required. ")]
    [MaxLength(50)]
    public string LastName { get; set; }
}

