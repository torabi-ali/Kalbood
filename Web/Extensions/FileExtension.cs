using Data.Utility;

namespace Web.Extensions;

public static class FileExtension
{
    public static List<string> ImageExtentions => new() { ".jpg", ".jpeg", ".png" };
    public static List<string> VideoExtentions => new() { ".mp4" };
    public static IEnumerable<string> AllowedExtentions => ImageExtentions.Union(VideoExtentions);

    public static async Task<string> UploadFileAsync(this IFormFile file, string baseUploadPath, string recommendedName = null)
    {
        if (file is null)
        {
            return null;
        }

        if (!AllowedExtentions.Contains(Path.GetExtension(file.FileName).ToLower()))
        {
            throw new ArgumentException("File extentions is not allowed.");
        }

        var directory = Path.Combine("wwwroot", "uploads", baseUploadPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (recommendedName is null)
        {
            recommendedName = file.FileName;
        }
        else
        {
            recommendedName = $"{recommendedName}{Path.GetExtension(file.FileName)}";
        }

        var fileName = StringUtility.TrimFileName(recommendedName);
        var fullPath = Path.Combine(directory, fileName);
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var dbPath = Path.Combine("uploads", baseUploadPath, fileName);
        return '/' + dbPath.Replace("\\", "/");
    }

    public static bool DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}