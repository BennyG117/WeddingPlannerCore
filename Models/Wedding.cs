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
    // [MinLength(10, ErrorMessage = "Must be at least 10 characters long")]
    // [MaxLength(50, ErrorMessage = "No longer than 50 characters long")]
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


    //! TBD - adding many to many - user to vacations linking


// List<Vacation> vacations = db.Vacations.Include(v => v.Creator).ToList();

    // public List<UserPostLike> PostLikes {get; set;} = new List<UserPostLike>();

}
