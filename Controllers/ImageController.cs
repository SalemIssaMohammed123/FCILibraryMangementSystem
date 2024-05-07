using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers
{
    public class ImageController : Controller
    {
        private readonly string[] _extensions = new[] { ".png", ".jpg", ".jpeg" };
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.JsonResult CheckImage(IFormFile image)
        {
            if (image != null)
            {
                var extension = System.IO.Path.GetExtension(image.FileName);

                if (!_extensions.Contains(extension.ToLower()))
                {
                    return Json(false);
                }
            }

            return Json(true);
        }
    }
}
