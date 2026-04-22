using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    public class KitapController : Controller
    {

        private readonly IKitapRepository _kitapRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment; // KitapController'ın constructor'ında IKitapRepository, IKitapTuruRepository ve IWebHostEnvironment türünde parametreler alıyoruz. Bu parametreler, dependency injection (bağımlılık enjeksiyonu) yoluyla sağlanır. Böylece, KitapController'ın ihtiyaç duyduğu hizmetlere erişebiliriz.
        }


        public IActionResult Index()   // index action çağırıldığı zaman veritabanına gidip kitap türlerini listeleyecek
        {
            //List<Kitap> objKitapList = _kitapRepository.GetAll().ToList();

            List<Kitap> objKitapList = _kitapRepository.GetAll(includeProps:"KitapTuru").ToList();
           
            return View(objKitapList);
        }


        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll()
               .Select(k => new SelectListItem
               {
                   Text = k.Ad,
                   Value = k.Id.ToString()
               });

            ViewBag.KitapTuruList = KitapTuruList; // ViewBag ile kitap türlerini view'e gönderiyoruz.

            if (id == null || id == 0)
            {
                // Ekleme işlemi
                return View();
            }
            else
            {
                // Güncelleme işlemi
                Kitap? kitapVT = _kitapRepository.Get(u => u.Id == id); // id'ye göre kitap  veritabanından bulup getiriyoruz. Get metodu IRepository'den geliyor. Get(Expression<Func<T, bool>> filtre)
                if (kitapVT == null)
                {
                    return NotFound();
                }
                return View(kitapVT);
            }

            
        }

        [HttpPost]
        public IActionResult EkleGuncelle(Kitap kitap, IFormFile? file)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);


            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath; // wwwroot klasörünün fiziksel yolunu alıyoruz. Bu, resim dosyalarını kaydetmek için kullanılır.
                string kitapPath = Path.Combine(wwwRootPath, @"img"); // wwwroot/img/kitap klasörünün fiziksel yolunu oluşturuyoruz. Resimler bu klasöre kaydedilecek.

                if (file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    kitap.ResimUrl = @"\img\" + file.FileName; // Kitap nesnesinin ResimUrl özelliğine, kaydedilen resmin yolunu atıyoruz. Bu yol, view'de resmin görüntülenmesi için kullanılacak.
                }
               


                if (kitap.Id == 0)
                {
                    // Ekleme işlemi
                    _kitapRepository.Ekle(kitap);
                    TempData["basarili"] = "Kitap Başarıyla Eklendi.";
                }
                else
                {
                    // Güncelleme işlemi
                    _kitapRepository.Guncelle(kitap);
                    TempData["basarili"] = "Kitap Başarıyla Güncellendi.";
                }


                _kitapRepository.Kaydet(); 
                return RedirectToAction("Index", "Kitap");

            }
            return View();

        }




        /*
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
        */

        /*
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
        */



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
