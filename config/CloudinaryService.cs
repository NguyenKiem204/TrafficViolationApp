using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.IO;
using System.Configuration;

namespace TrafficViolationApp.config
{
    public class CloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = config.AppSettings.Settings;

                var cloudName = appSettings["CloudinaryCloudName"]?.Value;
                var apiKey = appSettings["CloudinaryApiKey"]?.Value;
                var apiSecret = appSettings["CloudinaryApiSecret"]?.Value;

                if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
                {
                    throw new Exception("Cloudinary credentials not found in config");
                }

                var account = new Account(cloudName, apiKey, apiSecret);
                cloudinary = new Cloudinary(account);
            }
            catch (Exception ex)
            {
                throw new Exception($"Configuration error: {ex.Message}", ex);
            }
        }

        public string UploadImage(string filePath)
        {
            try
            {
                string uniqueId = $"traffic_violation_{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileNameWithoutExtension(filePath)}";

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(filePath),
                    PublicId = uniqueId,
                    Folder = "traffic_violations/images"
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image to Cloudinary: {ex.Message}");
                throw new Exception("Failed to upload image", ex);
            }
        }

        public string UploadVideo(string filePath)
        {
            try
            {
                string uniqueId = $"traffic_violation_{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileNameWithoutExtension(filePath)}";

                var uploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(filePath),
                    PublicId = uniqueId,
                    Folder = "traffic_violations/videos"
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading video to Cloudinary: {ex.Message}");
                throw new Exception("Failed to upload video", ex);
            }
        }
    }
}


