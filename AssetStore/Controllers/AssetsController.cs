using AssetStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace AssetStore.Controllers
{
    public class AssetsController : Controller
    {
        private AssetsDbContext _context;

        private PictureDbContext _photoContext;

        private ItemDbContext _itemContext;

        public AssetsController() {
            _context = new AssetsDbContext();
            _photoContext = new PictureDbContext();
            _itemContext = new ItemDbContext();
        }
        [HttpGet]
        public ActionResult Index(string search) {
            AllAssets all = new AllAssets();
            if (search != null)
            {
                if (search.ToLower().Contains("2d"))
                {
                    all.assets2D = _context.Assets2D.ToList();
                }
                if (search.ToLower().Contains("3d"))
                {
                    all.assets3D = _context.Assets3D.ToList();
                }
                if (search.ToLower().Contains("tools"))
                {
                    all.assetsTools = _context.AssetsTools.ToList();
                }
                if (search.ToLower().Contains("audio"))
                {
                    all.assetsAudio = _context.AssetsAudio.ToList();
                }
                if (search.ToLower().Contains("free"))
                {
                    all.assets2D = _context.Assets2D.Where(z => z.Price == 0).ToList();
                    all.assets3D = _context.Assets3D.Where(z => z.Price == 0).ToList();
                    all.assetsAudio = _context.AssetsAudio.Where(z => z.Price == 0).ToList();
                    all.assetsTools = _context.AssetsTools.Where(z => z.Price == 0).ToList();
                }
                if (_context.Assets2D != null)
                {
                    foreach (var item in _context.Assets2D)
                    {
                        if (item.Name.Contains(search) && !all.assets2D.Contains(item))
                        {
                            all.assets2D.Add(item);
                        }
                    }
                }
                if (_context.Assets3D != null)
                {
                    foreach (var item in _context.Assets3D)
                    {
                        if (item.Name.Contains(search) && !all.assets3D.Contains(item))
                        {
                            all.assets3D.Add(item);
                        }
                    }
                    
                }
                if (_context.AssetsTools != null)
                {
                    foreach (var item in _context.AssetsTools)
                    {
                        if (item.Name.Contains(search) && !all.assetsTools.Contains(item))
                        {
                            all.assetsTools.Add(item);
                        }
                    }
                }
                if (_context.AssetsAudio != null)
                {
                    foreach (var item in _context.AssetsAudio)
                    {
                        if (item.Name.Contains(search) && !all.assetsAudio.Contains(item))
                        {
                            all.assetsAudio.Add(item);
                        }
                    }
                }
                
            }
            else
            {
                all.assets2D = _context.Assets2D.ToList();
                all.assets3D = _context.Assets3D.ToList();
                all.assetsTools = _context.AssetsTools.ToList();
                all.assetsAudio = _context.AssetsAudio.ToList();
            }
            if (all.assets2D != null) {
                foreach (Asset2D model in all.assets2D)
                {
                    if (model.imagesId != null)
                    {
                        int elem = int.Parse(model.imagesId.Split(',').First());
                        var photo = _photoContext.AssetsPhotos.FirstOrDefault(z => z.Id == elem);
                        all.asset2dImages.Add(photo.ImageData);
                    }
                }
            }
            if (all.assets3D != null) {
                foreach (Asset3D model in all.assets3D)
                {
                    if (model.imagesId != null)
                    {
                        int elem = int.Parse(model.imagesId.Split(',').First());
                        var photo = _photoContext.AssetsPhotos.FirstOrDefault(z => z.Id == elem);
                        all.asset3dImages.Add(photo.ImageData);
                    }
                }
            }
            if (all.assetsAudio != null)
            {
                foreach (AssetAudio model in all.assetsAudio)
                {
                    if (model.imagesId != null)
                    {
                        int elem = int.Parse(model.imagesId.Split(',').First());
                        var photo = _photoContext.AssetsPhotos.FirstOrDefault(z => z.Id == elem);
                        all.assetAudioImages.Add(photo.ImageData);
                    }
                }
            }
            if (all.assetsTools != null) {
                foreach (AssetTools model in all.assetsTools)
                {
                    if (model.imagesId != null)
                    {
                        int elem = int.Parse(model.imagesId.Split(',').First());
                        var photo = _photoContext.AssetsPhotos.FirstOrDefault(z => z.Id == elem);
                        all.assetsToolsImages.Add(photo.ImageData);
                    }
                }
            }
            return View(all);
        }

        public ActionResult _2D(int Id) {
            var asset = _context.Assets2D.FirstOrDefault(z => z.Id == Id);
            AssetPhotosModel model = new AssetPhotosModel();
            model.asset = asset;
            var user_id = User.Identity.GetUserId();
            //var user_id = System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString();
            var item = _itemContext.Assets.FirstOrDefault(z => z.assetId == model.asset.Id && z.assetType == "2D" && z.buyerId == user_id);
            if (item != null)
            {
                model.item = item;
            }
            //model.asset.Size = model.asset.assetZip.Count() / 1048576f;
            if (model.item != null)
            {
                
            }
            string[] str = asset.imagesId.Split(',');
            int[] ints = Array.ConvertAll(str, s => int.Parse(s));
            for (int i = 0; i < ints.Length; i++)
            {

                var photo = _photoContext.AssetsPhotos.AsEnumerable().FirstOrDefault(z => z.Id == ints[i]);
                model.photos.Add(photo.ImageData);
            }
            return View(model);
        }
        public ActionResult _3D(int Id)
        {
               
            var asset = _context.Assets3D.FirstOrDefault(z => z.Id == Id);
            AssetPhotosModel model = new AssetPhotosModel();
            model.asset = asset;
            string user_id = User.Identity.GetUserId();
            var item = _itemContext.Assets.FirstOrDefault(z => z.assetId == model.asset.Id && z.assetType == "3D" && z.buyerId == user_id);
            if (item != null)
            {
                model.item = item;
            }
            //model.asset.Size = model.asset.assetZip.Count() / 1048576f;
             
            //model.asset.Size = model.asset.assetZip.Count() / 1048576f;
            string[] str = asset.imagesId.Split(',');
            int[] ints = Array.ConvertAll(str, s => int.Parse(s));
            for (int i = 0; i < ints.Length; i++)
            {
                
                var photo = _photoContext.AssetsPhotos.AsEnumerable().FirstOrDefault(z => z.Id == ints[i]);
                model.photos.Add(photo.ImageData);
            }
            return View(model);
        }
        public ActionResult Tools(int Id)
        {
            var asset = _context.AssetsTools.FirstOrDefault(z => z.Id == Id);
            AssetPhotosModel model = new AssetPhotosModel();
            model.asset = asset;
            string user_id = User.Identity.GetUserId();
            var item = _itemContext.Assets.FirstOrDefault(z => z.assetId == model.asset.Id && z.assetType == "Tools" && z.buyerId == user_id);
            if (item != null)
            {
                model.item = item;
            }
            //model.asset.Size = model.asset.assetZip.Count() / 1048576f;
            string[] str = asset.imagesId.Split(',');
            int[] ints = Array.ConvertAll(str, s => int.Parse(s));
            for (int i = 0; i < ints.Length; i++)
            {

                var photo = _photoContext.AssetsPhotos.AsEnumerable().FirstOrDefault(z => z.Id == ints[i]);
                model.photos.Add(photo.ImageData);
            }
            return View(model);
        }
        public ActionResult Audio(int Id)
        {
            var asset = _context.AssetsAudio.FirstOrDefault(z => z.Id == Id);
            AssetPhotosModel model = new AssetPhotosModel();
            model.asset = asset;
            string user_id = User.Identity.GetUserId();
            var item = _itemContext.Assets.FirstOrDefault(z => z.assetId == model.asset.Id&& z.assetType == "Audio" && z.buyerId == user_id);
            if (item != null)
            {
                model.item = item;
            }
            //model.asset.Size = model.asset.assetZip.Count() / 1048576f;
            string[] str = asset.imagesId.Split(',');
            int[] ints = Array.ConvertAll(str, s => int.Parse(s));
            for (int i = 0; i < ints.Length; i++)
            {

                var photo = _photoContext.AssetsPhotos.AsEnumerable().FirstOrDefault(z => z.Id == ints[i]);
                model.photos.Add(photo.ImageData);
            }
            return View(model);
        }

        public ActionResult NotAPublisher() {
            return View();
        }

        /*[Authorize]*/
        public ActionResult Publish()
        {

            if (User.IsInRole("Publisher") || User.IsInRole("Administrator"))
            {
                AssetImages model = new AssetImages();
                return View(model);
            }
            else {
                return RedirectToAction("NotAPublisher");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult Publish(AssetImages model) {
            if (!ModelState.IsValid) {
                return View("Publish", model);
            }
            model.asset.Publisher = User.Identity.Name;
            model.asset.ReleaseDate = DateTime.Now.ToString();
            if(!model.File.FileName.EndsWith("rar") && !model.File.FileName.EndsWith("zip"))
            {
                return View("Publish", model);
            }
            string[] formats = new string[] { ".jpg", ".png", ".jpeg" };
            bool Wrong = true;
            foreach (var image in model.Files)
            {
                foreach(var item in formats)
                {
                    if (image.FileName.Contains(item))
                    {
                        Wrong = false;
                        break;
                    }
                }
            }
            if (Wrong)
            {
                return View("Publish", model);
            }

            foreach (var file in model.Files)
            {
                if (file == null) {
                    //at least 1 photo per asset
                    return View(model);
                }
                else if (file.ContentLength > 0)
                {
                    Picture c = new Picture();
                    c.ImageData = new byte[file.ContentLength];
                    file.InputStream.Read(c.ImageData, 0, c.ImageData.Length);
                   
                    _photoContext.AssetsPhotos.Add(c);
                    _photoContext.SaveChanges();
                    if (model.asset.imagesId == null)
                    {
                        model.asset.imagesId = c.Id.ToString();
                    }
                    else {
                        model.asset.imagesId += "," + c.Id.ToString();
                    }
                }
            }

            if (model.File.ContentLength > 0) {
                

                MemoryStream target = new MemoryStream();
                model.File.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                model.asset.assetZip = data;
            }
            model.asset.Size = model.asset.assetZip.Count() / 1048576f;

            //////
            if (model.asset.Type == "2D")
            {
                Asset2D asset2D = new Asset2D(model.asset);
                _context.Assets2D.Add(asset2D);
            }
            else if (model.asset.Type == "3D")
            {
                Asset3D asset3D = new Asset3D(model.asset);
                _context.Assets3D.Add(asset3D);
            }
            else if (model.asset.Type == "Audio")
            {
                AssetAudio assetAudio = new AssetAudio(model.asset);
                _context.AssetsAudio.Add(assetAudio);
            }
            else if (model.asset.Type == "Tools")
            {
                AssetTools assetTools = new AssetTools(model.asset);
                _context.AssetsTools.Add(assetTools);
            }
            else
            {
                return HttpNotFound();
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                Debug.WriteLine(model.asset.Type);
            }


            AllAssets allAssets = new AllAssets();
            allAssets.assets2D = _context.Assets2D.ToList();
            allAssets.assets3D = _context.Assets3D.ToList();
            allAssets.assetsAudio = _context.AssetsAudio.ToList();
            allAssets.assetsTools = _context.AssetsTools.ToList();

            return RedirectToAction("Index", allAssets);
        }
        public ActionResult View(int Id) {
            Picture p = new Picture();
            p = _photoContext.AssetsPhotos.FirstOrDefault(z => z.Id == Id);
            return View(p);
        }


        public ActionResult Download()
        {
            return new FileContentResult(_context.AssetsAudio.FirstOrDefault(z => z.Id == 2).assetZip, "zip");
        }
        [Authorize]
        public ActionResult AddToCart(int Id, string Type)
        {
            Asset asset = new Asset();
            if (Type == "2D")
            {
                asset = _context.Assets2D.FirstOrDefault(z => z.Id == Id);
            }
            if (Type == "3D")
            {
                asset = _context.Assets3D.FirstOrDefault(z => z.Id == Id);
            }
            if (Type == "Audio")
            {
                asset = _context.AssetsAudio.FirstOrDefault(z => z.Id == Id);
            }
            if (Type == "Tools")
            {
                asset = _context.AssetsTools.FirstOrDefault(z => z.Id == Id);
            }
            Item item = new Item();
            item.assetId = asset.Id;
            item.assetType = asset.Type;
            item.name = asset.Name;
            int first = int.Parse(asset.imagesId.Split(',').First());
            var photo = _photoContext.AssetsPhotos.FirstOrDefault(z => z.Id == first);
            item.image = photo.ImageData;
            item.price = asset.Price;
            item.isInCart = true;

            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    var userIdValue = userIdClaim.Value;
                    item.buyerId = userIdValue;
                }
            }
            _itemContext.Assets.Add(item);
            _itemContext.SaveChanges();


            AllAssets allAssets = new AllAssets();
            allAssets.assets2D = _context.Assets2D.ToList();
            allAssets.assets3D = _context.Assets3D.ToList();
            allAssets.assetsAudio = _context.AssetsAudio.ToList();
            allAssets.assetsTools = _context.AssetsTools.ToList();

            return RedirectToAction("Index", allAssets);
        }
        public ActionResult HowToPublisher() {
            return View();
        }
        public ActionResult AboutUs() {
            return View();
        }
    }
}