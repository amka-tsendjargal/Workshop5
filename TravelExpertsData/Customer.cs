using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelExpertsData;

[Index("AgentId", Name = "EmployeesCustomers")]
public partial class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(25)]
    [Display(Name = "First Name")]
    public string CustFirstName { get; set; } = null!;

    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(25)]
    [Display(Name = "Last Name")]
    public string CustLastName { get; set; } = null!;

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(75)]
    [Display(Name = "Address")]
    public string CustAddress { get; set; } = null!;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(50)]
    [Display(Name = "City")]
    public string CustCity { get; set; } = null!;

    [Required(ErrorMessage = "Province is required.")]
    [StringLength(2)]
    [Display(Name = "Province")]
    public string CustProv { get; set; } = null!;

    [Required(ErrorMessage = "Postal Code is required.")]        
    //[RegularExpression(@"^(\d{5}(?:[-\s]\d{4})?|[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d)$", ErrorMessage = "Invalid Postal Code format. Correct formats: 12345, 12345-6789, or ANA NAN.")]
    [Display(Name = "Postal Code")]
    public string CustPostal { get; set; } = null!;

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(25)]
    [Display(Name = "Country")]
    public string? CustCountry { get; set; }

    [Required(ErrorMessage = "Home Phone is required.")]
    [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Invalid Home Phone format. Correct format: XXX-XXX-XXXX.")]
    [StringLength(20)]
    [Display(Name = "Phone Number")]
    public string? CustHomePhone { get; set; }


    [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Invalid Business Phone format. Correct format: XXX-XXX-XXXX.")]
    [StringLength(20)]
    [Display(Name = "Business Phone Number")]
    public string? CustBusPhone { get; set; } = null!;

    
    [EmailAddress(ErrorMessage = "Invalid Email format.")]
    [StringLength(50)]
    [Display(Name = "Email")]
    public string? CustEmail { get; set; } = null!;

    [Display(Name = "Agent ID")]
    public int? AgentId { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100)]
    [Unicode(false)]
    [Display(Name = "Username")]
    public string? UserId { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100)]
    [Unicode(false)]
    [Display(Name = "Password")]
    public string? UserPwd { get; set; }

    [Compare("UserPwd", ErrorMessage = "Passwords do not match.")]
    [NotMapped]
    public string? ConfirmPassword { get; set; }

    [ForeignKey("AgentId")]
    [InverseProperty("Customers")]
    public virtual Agent? Agent { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("Customer")]
    public virtual ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();

    [InverseProperty("Customer")]
    public virtual ICollection<CustomersReward> CustomersRewards { get; set; } = new List<CustomersReward>();
}
