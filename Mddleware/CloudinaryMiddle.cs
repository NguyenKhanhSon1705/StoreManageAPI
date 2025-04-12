
using CloudinaryDotNet;

namespace StoreManageAPI.Mddleware
{
    public class CloudinaryMiddle   
    {
        private readonly ICloudinary cloud;
        public CloudinaryMiddle(IConfiguration configuration)
        {
            var cloudinaryAccount = new Account(
            configuration[Config.Config.CloudName],
            configuration[Config.Config.ApiKey],
            configuration[Config.Config.ApiSecret]
        );
            cloud = new Cloudinary(cloudinaryAccount);
        }
       /* public async Task<string> CloudinaryUploadImages(IList<IFormFile> files)
        {
            if (files.Count > 0)
            {

            }

            return null;
        }*/

        public async Task<string> CloudinaryUploadImage(IFormFile? file)
        {
            if(file?.Length <= 0)
            {
                return string.Empty;
            }
            using (var stream = file?.OpenReadStream())
            {
                var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
                {
                    File = new FileDescription(file?.FileName, stream),
                    Folder = "StoreManageAPI"
                };
                var uploadResult = await cloud.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
        }

    }
}
