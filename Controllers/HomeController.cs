using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginReg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;


namespace LoginReg.Controllers
{


    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context)
        {


            _context = context;
            _context.SaveChanges();
        }



        [HttpGet("")]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();


        }





        [HttpGet("Registerd")]
        public IActionResult SignIn()
        {
            Console.WriteLine("Got inside registerd");


            return View("SignIn");
        }



        public IActionResult DashboardView()
        {
            List<Wedding> Weddings = _context.weddings.Include(w => w.Vists).ToList();


            string Email = HttpContext.Session.GetString("Email");
            User user = _context.users.SingleOrDefault(usr => usr.Email == Email);





            ViewBag.loggedUser = user;
            ViewBag.weds = Weddings;


            return View("Dashboard");


        }


        [HttpGet("Home/dashboard")]
        public IActionResult redDashView()
        {


            return RedirectToAction("DashboardView");
        }



        [HttpGet("Home/newWedding")]
        public IActionResult NewWedding()
        {



            return View("Create");
        }



        [HttpGet("Home/wed/{id}")]
        public IActionResult getWedding(int id)
        {

            Console.WriteLine("In Wedding Details");

            Wedding wed = _context.weddings.FirstOrDefault(w => w.WeddingId == id);

            return RedirectToAction("WeddingDetails", wed);
        }

        public IActionResult WeddingDetails(Wedding wed)
        {

            List<User> Users = _context.users.Include(usr => usr.Weddings).ToList();
            List<Wedding> Weds = _context.weddings.Include(w => w.Vists).ToList();
            List<Vistors> vists = _context.Vistors.Where(v => v.WeddingId == wed.WeddingId)
            .Include(v => v.Users).ToList();



            ViewBag.vis = vists;
            ViewBag.wed = wed;

            return View("WeddingDetails");

        }
        [HttpPost("Home/addWedding")]
        public IActionResult addWedding(Wedding wed, string Wedder1, string Wedder2)
        {
            string Email = HttpContext.Session.GetString("Email");
            User user = _context.users.SingleOrDefault(usr => usr.Email == Email);


            wed.Wedding_Names = Wedder1 + " and " + Wedder2;
            wed.Stat = user.UserId;

            user.Weddings.Add(wed);
            _context.SaveChanges();



            return RedirectToAction("WeddingDetails", wed);
        }






        [HttpPost("Home/create")]

        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                user.Confirm_Password = Hasher.HashPassword(user, user.Confirm_Password);

                user.Created_At = DateTime.Now;
                user.Updated_At = DateTime.Now;

                _context.Add(user);
                _context.SaveChanges();
                ViewBag.user = user;

                HttpContext.Session.SetString("Email", user.Email);
                return RedirectToAction("DashboardView");
            }
            else
            {
                return View("Index");
            }


        }

        [HttpPost("Home/login")]
        public IActionResult LogingMethod(string Email, string Password)
        {

            List<Wedding> Weddings = _context.weddings.Include(wd => wd.Vists).ToList();

            User logUser = _context.users.SingleOrDefault(usr => usr.Email == Email);




            PasswordHasher<User> Hasher = new PasswordHasher<User>();

            if (logUser != null && Password != null)
            {

                if (0 != Hasher.VerifyHashedPassword(logUser, logUser.Password, Password))
                {

                    ViewBag.logedUser = logUser;
                    HttpContext.Session.SetString("Email", logUser.Email);

                    return RedirectToAction("DashboardView");

                }
                else
                {
                    
                    ViewBag.err = "Password or Username is not valid";
                    return View("SignIn");

                }


            }
            else
            {
                
                ViewBag.err = "Email or Password can't be empty";
                return View("SignIn");
            }


        }



        [HttpGet("Home/Delete/{id}")]
        public IActionResult Delete(int id)
        {

            Wedding wed = _context.weddings.FirstOrDefault(w => w.WeddingId == id);
            _context.Remove(wed);
            _context.SaveChanges();


            return RedirectToAction("DashboardView");
        }


        [HttpGet("Home/Un_RSVP/{id}")]
        public IActionResult Un_RSVP(int id)
        {


            string Email = HttpContext.Session.GetString("Email");
            User user = _context.users.SingleOrDefault(usr => usr.Email == Email);


            // List<Wedding> Weddings = _context.weddings.Include(wd => wd.Vists).ToList();
            Wedding wed = _context.weddings.FirstOrDefault(w => w.WeddingId == id);

            Vistors v = _context.Vistors.FirstOrDefault(vis => vis.UserId == user.UserId);
            wed.Guest--;
            _context.Remove(v);
            _context.SaveChanges();

            return RedirectToAction("DashboardView");
        }

        [HttpGet("Home/RSVP/{id}")]

        public IActionResult RSVP(int id)
        {
            string Email = HttpContext.Session.GetString("Email");
            User user = _context.users.SingleOrDefault(usr => usr.Email == Email);


            Wedding wed = _context.weddings.FirstOrDefault(w => w.WeddingId == id);

            Vistors v = new Vistors()
            {

                Users = user,
                Wedding = wed


            };
            wed.Guest++;
            user.Vists.Add(v);
            wed.Vists.Add(v);
            _context.Add(v);
            _context.SaveChanges();




            return RedirectToAction("DashboardView");





        }





        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
