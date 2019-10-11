using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using msgBoard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace msgBoard.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
     
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

     
        [HttpPost("register")]

        public IActionResult Register(IndexViewModel dataModel)
        {
            // Check initial ModelState
            if(ModelState.IsValid)
            {
                // If a User exists with provided email
                if(dbContext.Users.Any(u => u.Email == dataModel.NewUser.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("NewUser.Email", "Email already in use!");
                    
                    // You may consider returning to the View at this point
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher =new PasswordHasher<User>();
                    dataModel.NewUser.Password=Hasher.HashPassword(dataModel.NewUser, dataModel.NewUser.Password);
                    dbContext.Add(dataModel.NewUser);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("UserId",(int)dataModel.NewUser.UserId);
                    return RedirectToAction("success");
                }
            }
            // other code
            return View("Index");
        } 


        [HttpPost("login")]
        public IActionResult Login(IndexViewModel dataModel)
        {
            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == dataModel.NewLogin.Email);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("NewLogin.Email", "Invalid Email");
                    return View("Index");
                }
                
                // Initialize hasher object
                var hasher = new PasswordHasher<Login>();
                
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(dataModel.NewLogin, userInDb.Password, dataModel.NewLogin.Password);
                
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("NewLogin.Password", "Password mismatch");
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId",userInDb.UserId);
                    return RedirectToAction("success");
                }
            }
            return View("Index");
        }

        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }


        [HttpGet("success")]
        public IActionResult Success()
        {
            if (HttpContext.Session.GetInt32("UserId")!=null)
            {
                int? _UserId=HttpContext.Session.GetInt32("UserId");
                ViewBag.userId= (int) _UserId;

                User _user=dbContext.Users.FirstOrDefault(u=>u.UserId==(int)_UserId);
                ViewBag.user=_user;
                
                DateTime _currentTime= DateTime.Now;
                ViewBag.currentTime=_currentTime;

                List<Message> _AllMessage=dbContext.Messages.Include(m=>m.msgCreator).Include(m=>m.msgComments).OrderByDescending(m=>m.CreatedAt).ToList();
                ViewBag.AllMessage=_AllMessage;


                return View("Dashboard");
            }
            return View("Index");
        }

        [HttpPost("newMessage")]
        public IActionResult newMessage(MsgCmt dataModel)
        {
            int? _UserId=HttpContext.Session.GetInt32("UserId");
            Console.WriteLine("@@@@@@@@@@@@@"+_UserId);
            if(ModelState.IsValid)
            {
                Console.WriteLine("**************"+_UserId);
                dataModel.NewMsg.UserId=(int)_UserId;
                dbContext.Add(dataModel.NewMsg);
                dbContext.SaveChanges();
                return RedirectToAction("success");
            }
            User _user=dbContext.Users.FirstOrDefault(u=>u.UserId==(int)_UserId);
            ViewBag.user=_user;
            List<Message> _AllMessage=dbContext.Messages.Include(m=>m.msgCreator).Include(m=>m.msgComments).OrderByDescending(m=>m.CreatedAt).ToList();
            ViewBag.AllMessage=_AllMessage;
            return View("Dashboard");
        }

        [HttpPost("newComment/{MessageId}")]
        public IActionResult newComment(MsgCmt dataModel, int MessageId)
        {
            int? _UserId=HttpContext.Session.GetInt32("UserId");
            if(ModelState.IsValid)
            {
                // Console.WriteLine("**************"+_UserId);
                dataModel.NewCmt.UserId=(int)_UserId;
                dataModel.NewCmt.MessageId=MessageId;
                dbContext.Add(dataModel.NewCmt);
                dbContext.SaveChanges();
                return RedirectToAction("success");
            }
            Console.WriteLine("@@@@@@@@@@@@@"+_UserId);
            User _user=dbContext.Users.FirstOrDefault(u=>u.UserId==(int)_UserId);
            ViewBag.user=_user;

            List<Message> _AllMessage=dbContext.Messages.Include(m=>m.msgCreator).Include(m=>m.msgComments).OrderByDescending(m=>m.CreatedAt).ToList();
            ViewBag.AllMessage=_AllMessage;
            return View("Dashboard");
        }

        [HttpGet("delete/{MessageId}")]
        public IActionResult delete(int MessageId)
        {
            Message thisMessage=dbContext.Messages.FirstOrDefault(m=>m.MessageId==MessageId);
            dbContext.Messages.Remove(thisMessage);
            dbContext.SaveChanges();
            return RedirectToAction("success");
        }







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
}
