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
    public string CustFirstName { get; set; } = null!;

    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(25)]
    public string CustLastName { get; set; } = null!;

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(75)]
    public string CustAddress { get; set; } = null!;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(50)]
    public string CustCity { get; set; } = null!;

    [Required(ErrorMessage = "Province is required.")]
    [StringLength(2)]
    public string CustProv { get; set; } = null!;

    [Required(ErrorMessage = "Postal Code is required.")]
    [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$", ErrorMessage = "Invalid Postal Code format.")]
    [StringLength(7)]
    public string CustPostal { get; set; } = null!;

    [StringLength(25)]
    public string? CustCountry { get; set; }

    [Phone(ErrorMessage = "Invalid Home Phone format.")]
    [StringLength(20)]
    public string? CustHomePhone { get; set; }

    [Required(ErrorMessage = "Business Phone is required.")]
    [Phone(ErrorMessage = "Invalid Business Phone format.")]
    [StringLength(20)]
    public string CustBusPhone { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email format.")]
    [StringLength(50)]
    public string CustEmail { get; set; } = null!;

    public int? AgentId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? UserId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
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
