using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using QR_API.Models;

namespace QR_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        public QrCodeController()
        {
        }

        [HttpPost("GetQr")] //api/GetQr
        public ActionResult<QrCodeResponse> GeQrByName([FromBody] QrCodeRequest qr)
        {
            if (qr == null)
            {
                return new BadRequestObjectResult(new QrCodeResponse
                {
                    OutputData = "Ошибка! Тело запроса не может быть пустым!"
                });
            }
            var result = Services.GenerateQrCodeService.GetQrByName(qr);
            return result;
        }
    }
}
