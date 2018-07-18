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

            List<User> Users = _context.users.Include(usr => usr.UserWeddings).ToList();
            List<Wedding> Weds = _context.weddings.Include(wed => wed.Vistors).ToList();

            List<Wedding> Weddings = _context.weddings.ToList();

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

            Wedding wed = _context.weddings.Include(w => w.Vistors).FirstOrDefault(w => w.WeddingId == id);

            return RedirectToAction("WeddingDetails", wed);
        }

        public IActionResult WeddingDetails(Wedding wed)
        {

            List<User> Users = _context.users.Include(usr => usr.UserWeddings).ToList();
            List<Wedding> Weds = _context.weddings.Include(w => w.Vistors).ToList();
            List<Vistors> vists = _context.Vistors.Where(v => v.WeddingId == wed.WeddingId).ToList();



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

            user.UserWeddings.Add(wed);
            _context.SaveChanges();



            return RedirectToAction("WeddingDetails", wed);
        }






        [HttpPost("Home/create")]

        public IActionResult Create(User user)
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

        [HttpPost("Home/login")]
        public IActionResult LogingMethod(string Email, string Password)
        {

            User logUser = _context.users.SingleOrDefault(usr => usr.Email == Email);


            if (logUser != null)
            {

                Console.WriteLine(logUser.Email);
                Console.WriteLine(logUser.Password);
                ViewBag.logedUser = logUser;
                HttpContext.Session.SetString("Email", logUser.Email);

                return RedirectToAction("DashboardView");

            }
            else
            {
                Console.WriteLine("Invalid User");
                ViewBag.err = "Invalid User";
                return View("SignIn");
            }

        }

        [HttpGet("Home/Un_RSVP/{id}")]
        public IActionResult Un_RSVP(int id)
        {


            string Email = HttpContext.Session.GetString("Email");
            User user = _context.users.SingleOrDefault(usr => usr.Email == Email);


            List<Wedding> Weddings = _context.weddings.Include(wd => wd.Vistors).ToList();
            Wedding wed = _context.weddings.FirstOrDefault(w => w.WeddingId == id);
            Vistors v = new Vistors();
            v.UsersId = user.UserId;
            _context.Remove(v);
            _context.SaveChanges();

            return RedirectToAction("DashboardView");
        }

        [HttpGet("Home/RSVP/{id}")]

        public IActionResult RSVP(int id)
        {
            string Email = HttpContext.Session.GetString("Email");
            User user = _context.users.SingleOrDefault(usr => usr.Email == Email);

            List<User> Users = _context.users.Include(usr => usr.UserWeddings).ToList();
            List<Wedding> Weddings = _context.weddings.Include(wd => wd.Vistors).ToList();
            List<Vistors> Vistors = _context.Vistors.Include(vis => vis.User).ToList();



            Wedding wed = _context.weddings.Include(vs => vs.Vistors).ThenInclude(u => u.User).FirstOrDefault(w => w.WeddingId == id);

            List<Vistors> vL = _context.Vistors.Where(vv => vv.WeddingId == wed.WeddingId).ToList();
          
                Vistors v = new Vistors();
                v.User = user;
                v.UsersId = user.UserId;
                wed.Guest++;
                wed.Vistors.Add(v);
                _context.SaveChanges();
                return RedirectToAction("DashboardView");

            



        }





        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
