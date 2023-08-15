using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlannerCore.Models;

namespace WeddingPlannerCore.Controllers;

//Added for Include:
using Microsoft.EntityFrameworkCore;

//ADDED for session check
using Microsoft.AspNetCore.Mvc.Filters;



//! *************** REMINDER to update public class "CONTROLLER NAMES" BELOW ***************
[SessionCheck]
public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;

    // Add field - adding context into our class // "db" can eb any name
    private MyContext db;

    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }


// ==============(DASHBOARD)===================
    [HttpGet("dashboard")]
    public IActionResult Index()
    {
        // vacations is AsyncLocal refered to as vacay on the other view page*
    List<Wedding> weddings = db.Weddings.Include(v => v.Creator).Include(l => l.WeddingGuests).ToList();        
        //passing vacations down to the view...
        return View("All", weddings);
    }



// ==============(NEW - wedding)==================

    [HttpGet("wedding/new")]
    public IActionResult New()
    {
        //returns itself if left blank
        return View();
    }

// ========(handle NEW WeddingMethod - view)=========
//TODO: check if wedder one is in the db and if wedder two is also in db, if both exist in the db then validation will trigger.


    [HttpPost("wedding/create")]
    //bringing in the model
    public IActionResult Create(Wedding newWedding)
    {
        //check category and declaration at top of method = "newWedding" // does wedder one exist in the db?

// if (!db.Users.Any(u => u.FirstName == newWedding.WedderOne || u.FirstName == newWedding.WedderTwo))
//         {
//             ModelState.AddModelError("WedderOne", "Wedding created cannot be between two users");
//         }

        
        if (db.Users.Any(u => u.FirstName == newWedding.WedderOne) && db.Users.Any(u => u.FirstName == newWedding.WedderTwo))
        {
            ModelState.AddModelError("WedderOne", "Your SO already registered your wedding!");
            ModelState.AddModelError("WedderTwo", "Your SO already registered your wedding!");
        }

        //checks model requirements*
        if(!ModelState.IsValid)
        {
            //trigger to see validations
            return View("New");
        }




        newWedding.UserId = (int) HttpContext.Session.GetInt32("UUID");

        //weddings from context
        db.Weddings.Add(newWedding);
        db.SaveChanges();
        //When success, send to Details view single wedding
        return RedirectToAction("Details",  new {id = newWedding.WeddingId});
    }


// ==============(get wedding view/view one)===================
    [HttpGet("wedding/{id}")]

    //adding in id parameter*
    public IActionResult Details(int id)
    {
        // confirm it matches the id we're passing in above*
    Wedding? weddings = db.Weddings.Include(v => v.Creator).Include(r => r.WeddingGuests).ThenInclude(u => u.User).FirstOrDefault(p => p.WeddingId == id);

    if (weddings == null)
    {
        return RedirectToAction("Index");
    }
        //passing weddings (the data) down to the view...
        return View("Details", weddings);
    }

// ==============(Edit Weddings)===================
    [HttpGet("wedding/{id}/edit")]

    //adding in id parameter*
    public IActionResult Edit(int id)
    {
        // confirm it matches the id we're passing in above*
    Wedding? weddings = db.Weddings.Include(v => v.Creator).FirstOrDefault(p => p.WeddingId == id);

    //confirming the creator of the vacation is the one able to edit it* (Session check)
    if (weddings == null || weddings.UserId != HttpContext.Session.GetInt32("UUID"))
    {
        return RedirectToAction("Index");
    }
        //passing weddings (the data) down to the view...
        return View("Edit", weddings);
    }


// ==============(Update Wedding)===================
    [HttpPost("wedding/{id}/update")]

    //adding in id parameter*
    public IActionResult Update(Wedding editedWedding, int id)
    {
        //check category and declaration at top of method = "editedWedding" // does wedder one exist in the db?
        if (db.Users.Any(u => u.FirstName == editedWedding.WedderOne) && db.Users.Any(u => u.FirstName == editedWedding.WedderTwo))
        {
            ModelState.AddModelError("WedderOne", "Your SO already registered your wedding!");
            ModelState.AddModelError("WedderTwo", "Your SO already registered your wedding!");
        }


        if (!ModelState.IsValid)
        {
            return Edit(id);
        }
        // confirm it matches the id we're passing in above*
    Wedding? weddings = db.Weddings.Include(v => v.Creator).FirstOrDefault(p => p.WeddingId == id);

    //confirming the creator of the vacation is the one able to edit it* (Session check)
    if (weddings == null || weddings.UserId != HttpContext.Session.GetInt32("UUID"))
    {
        return RedirectToAction("Index");
    }
        weddings.WedderOne = editedWedding.WedderOne;
        weddings.WedderTwo = editedWedding.WedderTwo;
        weddings.WeddingDate = editedWedding.WeddingDate;
        weddings.Address = editedWedding.Address;
        weddings.UpdatedAt = DateTime.Now;

        db.Weddings.Update(weddings);
        db.SaveChanges();
        // return RedirectToAction("Edit", new {id = id});
        // return RedirectToAction("Index");
        return RedirectToAction("Details",  new {id = editedWedding.WeddingId});
    }


    //Delete Method ============================================
    [HttpPost("wedding/{id}/delete")]
    public IActionResult Delete(int id)

    
    {
        Wedding? weddings = db.Weddings.FirstOrDefault(v => v.WeddingId == id);

        //added to stop from deleting other's input data
        if(weddings == null || weddings.UserId != HttpContext.Session.GetInt32("UUID")) 
        {
            return RedirectToAction("Index");
        }

        db.Weddings.Remove(weddings);
        db.SaveChanges();
        return RedirectToAction("Index");
    }





    //! setting up many to many RSVP method ================
    //RSVP Method ============================================
    [HttpPost("weddings/{id}/rsvp")]
    public IActionResult RSVP(int id)
    {
        int? userId = HttpContext.Session.GetInt32("UUID");

        if (userId == null) 
        {
            return RedirectToAction("Index");
        }
        
        //must equal for session check
        WeddingRSVP? existingRSVP = db.WeddingRSVPs.FirstOrDefault(u => u.UserId == userId.Value && u.WeddingId == id);

        if(existingRSVP != null)
        {
            db.WeddingRSVPs.Remove(existingRSVP);
        }
        else
        {
            WeddingRSVP newRSVP = new WeddingRSVP()
            {
                WeddingId = id,
                UserId = userId.Value 
            };
            db.WeddingRSVPs.Add(newRSVP);
        }
        db.SaveChanges();
        return RedirectToAction("Index");

    }








// ===================================


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}



//!SESSION CHECK ===========================================
// Name this anything you want with the word "Attribute" at the end -- adding filter for session at top*
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UUID");
        // Check to see if we got back null
        if(userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Index", "User", null);
        }
    }
}