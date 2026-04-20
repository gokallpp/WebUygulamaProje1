using Microsoft.AspNetCore.Mvc;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    public class KitapController : Controller
    {

        private readonly IKitapRepository _kitapRepository;

        public KitapController(IKitapRepository context)
        {
            _kitapRepository = context;
        }


        public IActionResult Index()   // index action çağırıldığı zaman veritabanına gidip kitap türlerini listeleyecek
        {
            List<Kitap> objKitapList = _kitapRepository.GetAll().ToList();
            return View(objKitapList);
        }


        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Kitap kitap)
        {
            if (ModelState.IsValid)
            {

                _kitapRepository.Ekle(kitap);
                _kitapRepository.Kaydet();
                TempData["basarili"] = "Kitap Başarıyla Eklendi.";
                return RedirectToAction("Index", "Kitap");

            }
            return View();

        }


        public IActionResult Guncelle(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();

            }
            Kitap? kitapVT = _kitapRepository.Get(u => u.Id == id);  // id'ye göre kitap  veritabanından bulup getiriyoruz. Get metodu IRepository'den geliyor. Get(Expression<Func<T, bool>> filtre)
            if (id == null || id == 0)
            {
                return NotFound();

            }
            return View(kitapVT);
        }


        [HttpPost]
        public IActionResult Guncelle(Kitap kitap)
        {
            if (ModelState.IsValid)
            {

                _kitapRepository.Guncelle(kitap);
                _kitapRepository.Kaydet();
                TempData["basarili"] = "Kitap Başarıyla Güncellendi.";
                return RedirectToAction("Index", "Kitap");

            }
            return View();

        }



        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();

            }
            Kitap? kitapVT = _kitapRepository.Get(u => u.Id == id);
            if (id == null || id == 0)
            {
                return NotFound();

            }
            return View(kitapVT);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            Kitap? kitap = _kitapRepository.Get(u => u.Id == id);
            if (kitap == null)
            {
                return NotFound();
            }


            _kitapRepository.Sil(kitap);
            _kitapRepository.Kaydet();
            TempData["basarili"] = "Silme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "Kitap");

            
            

        }

    }
}
