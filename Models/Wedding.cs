#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using Microsoft.VisualBasic;

namespace WeddingPlannerCore.Models;

public class Wedding
{
    //*KEY*
    [Key]
    // WeddingId =========================
    public int WeddingId {get; set;}



//! UPDATE EVERYTHIING BELOW: ===============================


// Wedder One (string), Wedder Two (string), Date (Date), Address (string)



    // WedderOne ========================= 
    [Required]
    // [MinLength(3, ErrorMessage = "Must be at least 3 characters long")]
    public string WedderOne {get; set;}


    // WedderTwo ========================= 
    [Required]
    // [MinLength(3, ErrorMessage = "Must be at least 3 characters long")]
    public string WedderTwo {get; set;}



    // WeddingDate ========================= 
    [Required]

    [WeddingDateInFuture]
    [DataType(DataType.Date)]
    [Display(Name = "Wedding Date")]

    public DateTime WeddingDate {get; set;}



    // Address ========================= 
    [Required]
    public string Address {get; set;}
    

    // CreatedAt ======================== 
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    


    // UpdatedAt ======================== 
    public DateTime UpdatedAt { get; set; } = DateTime.Now;



    // foreign key  - OUR ONE TO MANY*============================
    public int UserId {get; set;}


    public User? Creator {get; set;}


    //! ====== adding many to many - user to weddings linking ========

    //many to many
    public List<WeddingRSVP> WeddingGuests {get; set;} = new List<WeddingRSVP>();

    //! =================================================================


}


// CUSTOM VALIDATION BEING USED FOR: WeddingDate Check Outside the above
public class WeddingDateInFutureAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime Now = DateTime.Now;
        DateTime Input = (DateTime)value;


        if (Input < Now)
        {
            return new ValidationResult("Wedding Date must be in the future.");
        } else {
            return ValidationResult.Success;
        }
    }
}

