using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using QR_API.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace QR_API.Services
{
    public class GenerateQrCodeService
    {

        /// <summary>
        /// Возвращает QR из данных о qr
        /// </summary>
        /// <param name="request">Запрос с данными для QR-кода</param>
        /// <returns>Ответ с данными QR-кода или ошибкой</returns>
        public static ActionResult<QrCodeResponse> GetQrByName(QrCodeRequest request)
        {
            // Проверка на валидность данных
            if (string.IsNullOrWhiteSpace(request.InputData) || !IsValidCustomString(request.InputData))
            {
                return new BadRequestObjectResult(new QrCodeResponse
                {
                    OutputData = "InputData не может быть пустым, null или содержать недопустимые символы."
                });
            }

            if (string.IsNullOrWhiteSpace(request.BgColor) || !IsValidHexColor(request.BgColor))
            {
                return new BadRequestObjectResult(new QrCodeResponse
                {
                    OutputData = "Некорректный цвет фона. Ожидается HEX формат."
                });
            }

            if (string.IsNullOrWhiteSpace(request.FgColor) || !IsValidHexColor(request.FgColor))
            {
                return new BadRequestObjectResult(new QrCodeResponse
                {
                    OutputData = "Некорректный цвет переднего плана. Ожидается HEX формат."
                });
            }

            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeBackgroundColor = ColorTranslator.FromHtml(request.BgColor);
            encoder.QRCodeForegroundColor = ColorTranslator.FromHtml(request.FgColor);

            try
            {
                Bitmap qrcode = encoder.Encode(request.InputData, Encoding.UTF8);

                QrCodeResponse response = new QrCodeResponse
                {
                    OutputData = Convert.ToBase64String(GetArray(qrcode)), // Конвертируем в Base64
                    Format = ImageFormat.Png.ToString() // Устанавливаем формат
                };

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new QrCodeResponse
                {
                    OutputData = ex.Message
                });
            }
        }
        /// Создает массив байтов из битмапа
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        private static byte[] GetArray(Bitmap qrcode)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                qrcode.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
        /// <summary>
        /// Проверка на корректность HEX цвета
        /// </summary>
        /// <param name="hexColor">Цвет в HEX формате</param>
        /// <returns>True, если цвет корректный, иначе False</returns>
        private static bool IsValidHexColor(string hexColor)
        {
            // Цвет должен начинаться с # и содержать 6 или 8 символов после # (с учетом прозрачности)
            if (hexColor.StartsWith("#") && (hexColor.Length == 7 || hexColor.Length == 9))
            {
                // Пробуем преобразовать оставшуюся часть строки в число
                return int.TryParse(hexColor.Substring(1), System.Globalization.NumberStyles.HexNumber, null, out _);
            }
            return false;
        }
        /// <summary>
        /// Проверка строки на соответствие шаблону: допустимы только буквы и цифры, длина 5-10 символов
        /// </summary>
        /// <param name="input">Строка для проверки</param>
        /// <returns>True, если строка корректна, иначе False</returns>
        private static bool IsValidCustomString(string input)
        {
            // Регулярное выражение для строки: допустимы только буквы и цифры, длина от 5 до 10 символов
            string pattern = @"^[\u0000-\uFFFF-[\uD800-\uDFFF]]{1,1000}$";
            return Regex.IsMatch(input, pattern);
        }
    }
}
