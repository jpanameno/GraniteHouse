using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Extensions;
using GraniteHouse.Models;
using GraniteHouse.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Products = new List<Models.Products>()

            };
        }

        //Get Index SHopping Cart
        public async Task<IActionResult> Index()
        {
            List<int> lstShoppingcart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if(lstShoppingcart.Count>0)
            {
                foreach(int cartItem in lstShoppingcart)
                {
                    Products prod = _db.Products.Include(p=>p.SpecialTags).Include(p=>p.ProductTypes).Where(p => p.Id == cartItem).FirstOrDefault();
                    ShoppingCartVM.Products.Add(prod);
                }
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            ShoppingCartVM.Appointment.AppointmentDate = ShoppingCartVM.Appointment.AppointmentDate
                                                            .AddHours(ShoppingCartVM.Appointment.AppointmentTime.Hour)
                                                            .AddMinutes(ShoppingCartVM.Appointment.AppointmentTime.Minute);

            Appointment appointment = ShoppingCartVM.Appointment;
            _db.Appointments.Add(appointment);
            _db.SaveChanges();

            int appointmentId = appointment.Id;

            foreach(int productId in lstCartItems)
            {
                ProductsSelectedForAppointment productsSelectedForAppointment = new ProductsSelectedForAppointment()
                {
                    AppointmentId = appointmentId,
                    ProductId = productId
                };
                _db.ProductsSelectedForAppointment.Add(productsSelectedForAppointment);
            }
            _db.SaveChanges();
            lstCartItems = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);
            return RedirectToAction("AppointmentConfirmation", "ShoppingCart", new { Id = appointmentId });
        }

        public IActionResult Remove(int Id)
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if (lstCartItems.Count > 0)
            {
                if (lstCartItems.Contains(Id))
                {
                    lstCartItems.Remove(Id);
                }
            }

            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction(nameof(Index));
        }

        //Get Action Method
        public IActionResult AppointmentConfirmation(int Id)
        {
            ShoppingCartVM.Appointment = _db.Appointments.Where(m => m.Id == Id).SingleOrDefault();

            List<ProductsSelectedForAppointment> objProdList = _db.ProductsSelectedForAppointment.Where(p => p.AppointmentId == Id).ToList();
            foreach(ProductsSelectedForAppointment prodAptObj in objProdList)
            {
                ShoppingCartVM.Products.Add(_db.Products.Include(p => p.ProductTypes).Include(p => p.SpecialTags).Where(p => p.Id == prodAptObj.ProductId).FirstOrDefault());
            }

            return View(ShoppingCartVM);
        }
    }
}