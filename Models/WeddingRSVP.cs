#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

// Add this using statement to access NotMapped
using System.ComponentModel.DataAnnotations.Schema;


namespace WeddingPlannerCore.Models;

public class WeddingRSVP
{

    [Key]        
    public int WeddingRSVPId { get; set; }
    
    //ferighn key is a copy of the primary key
    
    //User -------
    public int UserId { get; set; }
    public User? User { get; set; }


    //Wedding -------
    public int WeddingId { get; set; }
    public Wedding? Wedding { get; set; }
    

    //created/updated at  ======================== 
    public DateTime CreatedAt {get;set;} = DateTime.Now;        
    public DateTime UpdatedAt {get;set;} = DateTime.Now;


}