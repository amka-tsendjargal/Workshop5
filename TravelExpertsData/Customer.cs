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
    [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$", ErrorMessage = "Invalid Postal Code format.")]
    [StringLength(7)]
    [Display(Name = "Postal Code")]
    public string CustPostal { get; set; } = null!;

    [StringLength(25)]
    [Display(Name = "Country")]
    public string? CustCountry { get; set; }

    [Phone(ErrorMessage = "Invalid Home Phone format.")]
    [StringLength(20)]
    [Display(Name = "Phone Number")]
    public string? CustHomePhone { get; set; }

    [Required(ErrorMessage = "Business Phone is required.")]
    [Phone(ErrorMessage = "Invalid Business Phone format.")]
    [StringLength(20)]
    [Display(Name = "Business Phone Number")]
    public string CustBusPhone { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email format.")]
    [StringLength(50)]
    [Display(Name = "Email")]
    public string CustEmail { get; set; } = null!;

    [Display(Name = "Agent ID")]
    public int? AgentId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    [Display(Name = "Username")]
    public string? UserId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    [Display(Name = "Password")]
    public string? UserPwd { get; set; }

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
