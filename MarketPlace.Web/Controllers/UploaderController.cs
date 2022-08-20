using MarketPlace.Application.Extensions;
using MarketPlace.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MarketPlace.Web.Controllers
{
    public class UploaderController : SiteBaseController
    {
        [HttpPost]
        public IActionResult UploadImage(IFormFile upload,string CKEditorFuncName,string CKEditor,string LangCode)
        {
            if (upload.Length <= 0) return null;
            if (!upload.IsImage())
            {
                var notImageMessage = "لطفا تصویر خود را وارد کنید";
                var notImage = JsonConvert.DeserializeObject("{ 'uploaded':0, 'error:{ 'message' : \""+ notImageMessage +"\" }' }");
                return Json(notImage);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName);
            upload.AddImageToServer(fileName,PathExtensions.UploadImageServer,null,null);

            return Json(new
            {
                uploaded = true,
                url = $"{PathExtensions.UploadImage}{fileName}"
            });

        }
    }
}
