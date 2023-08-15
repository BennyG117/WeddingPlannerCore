#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
// Add this using statement to access NotMapped
using System.ComponentModel.DataAnnotations.Schema;


namespace WeddingPlannerCore.Models;
public class User
{        
    [Key]        
    public int UserId { get; set; }
    
    [Required]        
    public string FirstName { get; set; }
    
    [Required]        
    public string LastName { get; set; }         
    
    //  ============================================
    [Required]
    [EmailAddress]
    //Adding unique
    [UniqueEmail]
    //TODO: MAY NEED NEW REGEX LINKS
    public string Email { get; set; }        
    
    //password  ======================== 
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; }          
    
    //created/updated at  ======================== 
    public DateTime CreatedAt {get;set;} = DateTime.Now;        
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    

    //Password confirm  ======================== 
    // This does not need to be moved to the bottom
    // But it helps make it clear what is being mapped and what is not
    [NotMapped]
    // There is also a built-in attribute for comparing two fields we can use!
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }
    //TODO: replace POST
    //!updated 
    // list of post objects (adding to an empty list of post objects):

    // //user can have many posts
    // public List<Post> AuthorPosts {get; set;} = new List<Post>();

    // //user can have many likes
    // public List<UserPostLike> UserLikes {get; set;} = new List<UserPostLike>();



}
// Adding Unique ======================== 
public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
    	// Though we have Required as a validation, sometimes we make it here anyways
    	// In which case we must first verify the value is not null before we proceed
        if(value == null)
        {
    	    // If it was, return the required error
            return new ValidationResult("Email is required!");
        }
    
    	// This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
    	if(_context.Users.Any(e => e.Email == value.ToString()))
        {
    	    // If yes, throw an error
            return new ValidationResult("Email must be unique!");
        } else {
    	    // If no, proceed
            return ValidationResult.Success;
        }
    }
}

