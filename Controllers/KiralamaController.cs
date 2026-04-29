
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]// Bu attribute, KiralamaController'a erişimi sadece Admin rolüne sahip kullanıcılara sınırlar. Yani, bu controller'daki tüm action'lara erişmek için kullanıcıların Admin rolüne sahip olması gerekir.    
    public class KiralamaController : Controller
    {

        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitapRepository _kitapRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KiralamaController(IKiralamaRepository kiralamaRepository, IKitapRepository kitapRepository, IWebHostEnvironment webHostEnvironment)// Dependency Injection (Bağımlılık Enjeksiyonu) ile gerekli repository'ler ve web host environment'ı alıyoruz.
        {
            _kiralamaRepository = kiralamaRepository;
            _kitapRepository = kitapRepository;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()   
        {
           

            List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps:"Kitap").ToList();
           
            return View(objKiralamaList);
        }


        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll()
               .Select(k => new SelectListItem
               {
                   Text = k.KitapAdi,
                   Value = k.Id.ToString()
               });

            ViewBag.KitapList = KitapList;

            if (id == null || id == 0)
            {
                // Ekleme işlemi
                return View();
            }
            else
            {
                // Güncelleme işlemi
                Kiralama? kiralamavt = _kiralamaRepository.Get(u => u.Id == id);
                if (kiralamavt == null)
                {
                    return NotFound();
                }
                return View(kiralamavt);
            }

            
        }

        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {
            
            if (ModelState.IsValid)
            {           
                if (kiralama.Id == 0)
                {
                    // Ekleme işlemi
                    _kiralamaRepository.Ekle(kiralama);
                    TempData["basarili"] = "Yeni Kiralama Kaydı Başarıyla Oluşturuldu.";
                }
                else
                {
                    // Güncelleme işlemi
                    _kiralamaRepository.Guncelle(kiralama);
                    TempData["basarili"] = "Kiralama Kaydı Başarıyla Güncellendi.";
                }


                _kiralamaRepository.Kaydet(); 
                return RedirectToAction("Index", "Kiralama");

            }
            return View();

        }
     


        public IActionResult Sil(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll()
               .Select(k => new SelectListItem
               {
                   Text = k.KitapAdi,
                   Value = k.Id.ToString()
               });
            
            ViewBag.KitapList = KitapList;



            if (id == null || id == 0)
            {
                return NotFound();

            }
            Kiralama? kiralamaVT = _kiralamaRepository.Get(u => u.Id == id);
            if (id == null || id == 0)
            {
                return NotFound();

            }
            return View(kiralamaVT);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            Kiralama? kiralama = _kiralamaRepository.Get(u => u.Id == id);
            if (kiralama == null)
            {
                return NotFound();
            }


            _kiralamaRepository.Sil(kiralama);
            _kiralamaRepository.Kaydet();
            TempData["basarili"] = "Silme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "Kiralama");

            
            

        }

    }
}
