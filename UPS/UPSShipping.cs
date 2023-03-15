using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UPS.Models;

namespace UPS
{
    public class UPSShipping
    {
        private string _ShippingTestAPI = "https://wwwcie.ups.com/rest/Ship";
        private string _ShippingLiveAPI = "https://onlinetools.ups.com/rest/Ship";
        private string _ShippingAPI = "";

        public UPSShipping(string shippingAPI = "")
        {
            if (string.IsNullOrWhiteSpace(shippingAPI))
            {
                _ShippingAPI = _ShippingTestAPI;
            }
            else
            {
                _ShippingAPI = shippingAPI;
            }
        }

        public UPSShipmentResponse Shipping(UPSShippingRequest request, bool isProduction = false)
        {
            UPSShipmentResponse response = null;
            string jsonStringResponse = "{}";

            try
            {
                var orgSecurityProtocol = ServicePointManager.SecurityProtocol;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    jsonStringResponse = webClient.UploadString(
                        _ShippingAPI, //isProduction ? _ShippingLiveAPI : _ShippingTestAPI,
                        JsonConvert.SerializeObject(request)
                    );
                }

                response = JsonConvert.DeserializeObject<UPSShipmentResponse>(jsonStringResponse);

                if (response.Fault == null)
                {
                    response.ShipmentResponse.ShipmentResults.PackageResults.ShippingLabel.GraphicImage = Rotate90(
                        response.ShipmentResponse.ShipmentResults.PackageResults.ShippingLabel.GraphicImage,
                        response.ShipmentResponse.ShipmentResults.PackageResults.ShippingLabel.ImageFormat.ImageFormatType
                    );
                }

                ServicePointManager.SecurityProtocol = orgSecurityProtocol;
            }
            catch (IOException ioe)
            {

            }
            catch (Exception e)
            {

            }

            return response;
        }

        public AVResponse ValidateAddress(AVRequest request, bool isProduction = false)
        {
            AVResponse response = null;
            string jsonStringResponse = "{}";

            try
            {
                var orgSecurityProtocol = ServicePointManager.SecurityProtocol;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    jsonStringResponse = webClient.UploadString(
                        "https://onlinetools.ups.com/rest/AV", //isProduction ? _ShippingLiveAPI : _ShippingTestAPI,
                        JsonConvert.SerializeObject(request)
                    );
                }

                AVSingleResponse avSingleResponse = null;
                try
                {
                    avSingleResponse = JsonConvert.DeserializeObject<AVSingleResponse>(jsonStringResponse);

                    response = new AVResponse();
                    response.AddressValidationResponse = new AddressValidationResponse();
                    response.AddressValidationResponse.Response = avSingleResponse.AddressValidationResponse.Response;
                    response.AddressValidationResponse.AddressValidationResult.Add(avSingleResponse.AddressValidationResponse.AddressValidationResult);
                }
                catch { }

                if (avSingleResponse == null)
                {
                    response = JsonConvert.DeserializeObject<AVResponse>(jsonStringResponse);
                }

                // Fix zipcode null value
                foreach(var address in response.AddressValidationResponse.AddressValidationResult)
                {
                    if (string.IsNullOrWhiteSpace(address.Address.PostalCode))
                    {
                        address.Address.PostalCode = address.PostalCodeHighEnd;
                    }
                }
                
                ServicePointManager.SecurityProtocol = orgSecurityProtocol;
            }
            catch (IOException ioe)
            {

            }
            catch (Exception e)
            {

            }

            return response;
        }

        private string Rotate90(string base64Image, ImageFormatType imageType)
        {
            string rotatedBase64Image = "";

            try
            {
                using (MemoryStream imgOutput = new MemoryStream())
                {
                    Bitmap img = (Bitmap)Bitmap.FromStream(new MemoryStream(Convert.FromBase64String(base64Image)));
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    img = img.Clone(new Rectangle(0, 0, 800, 1201), img.PixelFormat);
                    img = ScaleImage(img, 751, 1127);
                    img.Save(imgOutput, imageType == ImageFormatType.GIF ? System.Drawing.Imaging.ImageFormat.Gif : System.Drawing.Imaging.ImageFormat.Png);

                    byte[] byteOutput = new byte[imgOutput.Length];
                    imgOutput.Seek(0, SeekOrigin.Begin);
                    imgOutput.Read(byteOutput, 0, (int)imgOutput.Length);

                    rotatedBase64Image = Convert.ToBase64String(byteOutput);
                }
            }
            catch (Exception ex)
            {
                return base64Image;
            }

            return rotatedBase64Image;
        }

        private Bitmap cropImage(Bitmap img, Rectangle cropArea)
        {
            return img.Clone(cropArea, img.PixelFormat);
        }

        public Bitmap ScaleImage(Bitmap img, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / img.Width;
            var ratioY = (double)maxHeight / img.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(img.Width * ratio);
            var newHeight = (int)(img.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(img, 0, 0, newWidth, newHeight);

            return newImage;
        }

        /*public Bitmap ScaleImage(Bitmap img, int width, int height)
        {
            var newImage = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(img, 0, 0, width, height);

            return newImage;
        }*/
    }
}
