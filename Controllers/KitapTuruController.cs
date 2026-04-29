using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]// Bu attribute, KitapTuruController'a erişimi sadece Admin rolüne sahip kullanıcılara sınırlar. Yani, bu controller'daki tüm action'lara erişmek için kullanıcıların Admin rolüne sahip olması gerekir.
    public class KitapTuruController : Controller
    {

        private readonly IKitapTuruRepository _kitapturuRepository;

        public KitapTuruController(IKitapTuruRepository context)
        {
            _kitapturuRepository = context;
        }



        public IActionResult Index()   // index action çağırıldığı zaman veritabanına gidip kitap türlerini listeleyecekk
        {
            List<KitapTuru> objKitapTuruList = _kitapturuRepository.GetAll().ToList();
            return View(objKitapTuruList);
        }


        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(KitapTuru kitapTuru)
        {
            if (ModelState.IsValid)
            {

                _kitapturuRepository.Ekle(kitapTuru);
                _kitapturuRepository.Kaydet();
                TempData["basarili"] = "Kitap Türü Başarıyla Eklendi.";
                return RedirectToAction("Index", "KitapTuru");

            }
            return View();

        }


        public IActionResult Guncelle(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();

            }
            KitapTuru? kitapTuruVT = _kitapturuRepository.Get(u => u.Id == id);  // id'ye göre kitap türünü veritabanından bulup getiriyoruz. Get metodu IRepository'den geliyor. Get(Expression<Func<T, bool>> filtre)
            if (id == null || id == 0)
            {
                return NotFound();

            }
            return View(kitapTuruVT);
        }


        [HttpPost]
        public IActionResult Guncelle(KitapTuru kitapTuru)
        {
            if (ModelState.IsValid)
            {

                _kitapturuRepository.Guncelle(kitapTuru);
                _kitapturuRepository.Kaydet();
                TempData["basarili"] = "Kitap Türü Başarıyla Güncellendi.";
                return RedirectToAction("Index", "KitapTuru");

            }
            return View();

        }



        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();

            }
            KitapTuru? kitapTuruVT = _kitapturuRepository.Get(u => u.Id == id);
            if (id == null || id == 0)
            {
                return NotFound();

            }
            return View(kitapTuruVT);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            KitapTuru? kitapTuru = _kitapturuRepository.Get(u => u.Id == id);
            if (kitapTuru == null)
            {
                return NotFound();
            }


            _kitapturuRepository.Sil(kitapTuru);
            _kitapturuRepository.Kaydet();
            TempData["basarili"] = "Silme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "KitapTuru");

            
            

        }

    }
}
