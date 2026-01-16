using System;

namespace TaskHub.api.Services;

public interface IFileStorageSevice
{
    Task<string> UploadAsync(IFormFile file);

}
