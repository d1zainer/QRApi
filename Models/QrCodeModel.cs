using System.Drawing;
using System.Drawing.Imaging;

namespace QR_API.Models
{
    /// <summary>
    /// класс для запроса
    /// </summary>
    public class QrCodeRequest
    {
        public string? InputData { get; set; }  // То, что нужно преобразовать в Qr 
        public string? BgColor { get; set; } // Цвет в формате HEX, например, "#FF0000"
        public string? FgColor { get; set; } // Цвет в формате HEX, например, "#FF0000"
    }

    /// <summary>
    /// класс для ответа
    /// </summary>
    public class QrCodeResponse
    {
        public string? OutputData { get; set; } // То, что нужно возразщается клиенту
        public string? Format { get; set; } // Формат картинки - Png
    }
}
