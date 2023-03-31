using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CRUD_First.Models;

public partial class Employee
{
    public int Id { get; set; }
    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; } = null!;

    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }

    [DisplayName("Gender")]
    public string? Gender { get; set; }

    [DisplayName("Are you married ? ")]
    public Boolean MaritalStatus { get; set; }

    [MinAge(18,ErrorMessage ="Age should be >= 18")]
    [DisplayName("Date of Birth")]
    //[DataType(DataType.Date),DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}",ApplyFormatInEditMode =true )]
    //[Range(typeof(DateTime), "01/01/1960", DateTime.Now.ToShortDateString())]
    
    public DateTime? BirthDate { get; set; }

    public string? Hobbies { get; set; }

    [Display(Name = "Hobbies")]
    [NotMapped]
    public List<string>? Multihobbies { get; set; }

    [Column(TypeName = "nvarchar(200)")]
    [DisplayName("Image Name")]
    public string? Photos { get; set; }

    [NotMapped]
    [DisplayName("Upload File")]
    public IFormFile? ImageFile { get; set; }

    [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Salary must be numeric")]
    //[RegularExpression("^[0-9]*$", ErrorMessage = "Salary must be numeric")]
    [Range(5000, Int32.MaxValue,ErrorMessage ="Salary must be >=5000")]
    public decimal? Salary { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Address { get; set; }

    public int? Country { get; set; }
    [NotMapped]
    public string? CountrySelected { get; set; }
    [NotMapped]
    public string? StateSelected { get; set; }
    [NotMapped]
    public string? CitySelected { get; set; }

    public int? State { get; set; }

    public int? City { get; set; }

    [RegularExpression("^[0-9]*$", ErrorMessage = "Zipcode must be numeric")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "ZipCode must be 6 characters")]
    public string? ZipCode { get; set; }

    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.{6,12})(?=.*[0-9])(?=.*[A-Z])(?=.*[@#$%^&+*!=]).*$", ErrorMessage = "Password length should be 6-12 character and should contain 1 capital letter & 1 specia character and 1 number ")]
    public string? Password { get; set; }

    public DateTime? Created { get; set; }
}
