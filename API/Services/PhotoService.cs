using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Services;

public class PhotoService : IPhotoService
{
    private Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
        _cloudinary = new Cloudinary(acc);
    }
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var  uploadResult = new ImageUploadResult();

        if(file.Length > 0)
        {
            var stream = file.OpenReadStream();
            var uploadParam = new ImageUploadParams
            {
                File = new FileDescription(file.Name, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "da-net7"
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParam);
        }

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deletionParam = new DeletionParams(publicId);
        return await _cloudinary.DestroyAsync(deletionParam);
    }
}
