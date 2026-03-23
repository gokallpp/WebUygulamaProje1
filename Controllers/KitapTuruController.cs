using Microsoft.AspNetCore.Mvc;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    public class KitapTuruController : Controller
    {

        private readonly UygulamaDbContext _uygulamaDbContext;

        public KitapTuruController(UygulamaDbContext context)
        {
            _uygulamaDbContext = context;
        }


        public IActionResult Index()   // index action çağırıldığı zaman veritabanına gidip kitap türlerini listeleyecek
        {
            List<KitapTuru> objKitapTuruList = _uygulamaDbContext.KitapTurleri.ToList();
             return View(objKitapTuruList);
        }


        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(KitapTuru kitapTuru)
        {
            _uygulamaDbContext.KitapTurleri.Add(kitapTuru);
            _uygulamaDbContext.SaveChanges();
           return RedirectToAction("Index", "KitapTuru");
        }
    }
}
