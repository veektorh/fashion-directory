using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AspMvcStarter.Models
{
    public class cloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        /// <summary>
        /// Set up cloudinary acccount 
        /// </summary>
        /// <param name="apiKey">The Api Key</param>
        /// <param name="apiSecret">The Api Secret</param>
        /// <param name="cloudName">Optional CloudName</param>


        public cloudinaryService(string apiKey = "apikey", string apiSecret = "apisecret", string cloudName = "cloudname")
        {
            var myAccount = new Account { ApiKey = apiKey, ApiSecret = apiSecret, Cloud = cloudName };
            _cloudinary = new Cloudinary(myAccount);
        }
        

        public ImageUploadResult UploadImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.InputStream),
                    //Transformation = new Transformation().Width(200).Height(200).Crop("thumb").Gravity("face")
                };

                var uploadResult = _cloudinary.Upload(uploadParams);
                return uploadResult;
            }
            return null;
        }

        /// <summary>
        /// Delete a resource
        /// </summary>
        /// <param name="publicId"></param>

        public void DeleteResource(string publicId)
        {
            //var delParams = new DelResParams() { PublicIds = new List() { publicId }, Invalidate = true }; _cloudinary.DeleteResources(delParams);
        }
    }




}