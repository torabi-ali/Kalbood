using System.Globalization;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Web.Areas.Admin.Controllers.Common;

namespace Web.Areas.Admin.Controllers;

public class FileManagerController : BaseAdminController
{
    private readonly string webRootPath;
    private readonly string webPath;
    private readonly List<string> allowedExtensions;

    public FileManagerController(IWebHostEnvironment env)
    {
        // FileManager Content Folder
        webPath = "uploads";
        if (string.IsNullOrWhiteSpace(env.WebRootPath))
        {
            env.WebRootPath = Directory.GetCurrentDirectory();
        }
        webRootPath = Path.Combine(env.WebRootPath, webPath);
        allowedExtensions = ["jpg", "jpe", "jpeg", "gif", "png", "svg", "txt", "pdf", "odp", "ods", "odt", "rtf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "csv", "ogv", "avi", "mkv", "mp4", "webm", "m4v", "ogg", "mp3", "wav", "zip", "rar", "md"];

    }

    public IActionResult Index(string mode, string path, string name, List<IFormFile> files, string old, string @new, string source, string target, string content, bool thumbnail, string @string)
    {
        try
        {
            if (mode == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(path) && path.StartsWith('/'))
            {
                path = path[1..];
            }

            if (!string.IsNullOrWhiteSpace(@new) && @new.StartsWith('/'))
            {
                @new = @new == "/" ? string.Empty : @new[1..];
            }

            if (!string.IsNullOrWhiteSpace(source) && source.StartsWith('/'))
            {
                source = source[1..];
            }

            if (!string.IsNullOrWhiteSpace(target) && target.StartsWith('/'))
            {
                target = target[1..];
            }

            return mode.ToLower(CultureInfo.CurrentCulture) switch
            {
                "initiate" => Json(Initiate()),
                "getinfo" => Json(GetInfo(path)),
                "readfolder" => Json(ReadFolder(path)),
                "addfolder" => Json(AddFolder(path, name)),
                "upload" => Json(Upload(path, files).Result),
                "rename" => Json(Rename(old, @new)),
                "move" => Json(Move(old, @new)),
                "copy" => Json(Copy(source, target)),
                "savefile" => Json(SaveFile(path, content)),
                "delete" => Json(Delete(path)),
                "download" => Download(path),
                "getimage" => GetImage(path),
                "readfile" => ReadFile(path),
                "summarize" => Json(Summarize()),
                "seekfolder" => Json(SeekFolder(path, @string)),
                _ => throw new Exception("Unknown Request!"),
            };
        }
        catch (Exception e)
        {
            // returns all unhandeled exceptions and returns them in JSON format with 500.
            // Issue #314
            return new JsonResult(e.Message)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ContentType = "application/json"
            };
        }
    }

    private dynamic Initiate()
    {
        var result = new
        {
            Data = new
            {
                Type = "initiate",
                Attributes = new
                {
                    Config = new
                    {
                        Security = new
                        {
                            ReadOnly = false,
                            Extensions = new
                            {
                                IgnoreCase = true,
                                Policy = "ALLOW_LIST",
                                Restrictions = allowedExtensions
                            }
                        }
                    }
                }
            }
        };

        return result;

    }

    private static int GetUnixTimestamp(DateTime dt)
    {
        return (int)dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    private dynamic SeekFolder(string path, string search)
    {
        path ??= string.Empty;

        var searchPath = Path.Combine(webRootPath, path);
        var data = new List<dynamic>();

        foreach (var file in new DirectoryInfo(searchPath).GetFiles("*" + search + "*", SearchOption.AllDirectories).OrderByDescending(p => p.LastWriteTime))
        {
            var item = new
            {
                Id = MakeWebPath(Path.Combine(Path.GetRelativePath(webRootPath, file.DirectoryName), file.Name), true),
                Type = "file",
                Attributes = new
                {
                    file.Name,
                    Path = MakeWebPath(Path.Combine(Path.GetRelativePath(webRootPath, file.DirectoryName), file.Name), true),
                    Readable = 1,
                    Writable = 1,
                    Created = GetUnixTimestamp(file.CreationTimeUtc),
                    Modified = GetUnixTimestamp(file.LastWriteTimeUtc),
                    Size = file.Length,
                    Extension = file.Extension.TrimStart('.'),
                    // Insert Height and Width logic for images
                    Timestamp = DateTime.Now.Subtract(file.LastWriteTime).TotalSeconds
                }
            };
            data.Add(item);
        }
        foreach (var dir in new DirectoryInfo(searchPath).GetDirectories("*" + search + "*", SearchOption.AllDirectories))
        {

            var item = new
            {
                Id = MakeWebPath(Path.GetRelativePath(webRootPath, dir.FullName), false, true),
                Type = "folder",
                Attributes = new
                {
                    dir.Name,
                    Path = MakeWebPath(dir.FullName, true, true),
                    Readable = 1,
                    Writable = 1,
                    Created = GetUnixTimestamp(dir.CreationTimeUtc),
                    Modified = GetUnixTimestamp(dir.LastWriteTimeUtc),
                    Timestamp = DateTime.Now.Subtract(dir.LastWriteTime).TotalSeconds
                }
            };
            data.Add(item);
        }
        return new
        {
            Data = data
        };
    }

    private dynamic GetInfo(string path)
    {
        path ??= string.Empty;

        FileInfo file = new(path);

        return new
        {
            Data = new
            {
                Id = MakeWebPath(Path.Combine(Path.GetRelativePath(webRootPath, file.DirectoryName), file.Name), true),
                Type = "file",
                Attributes = new
                {
                    file.Name,
                    Path = MakeWebPath(Path.Combine(Path.GetRelativePath(webRootPath, file.DirectoryName), file.Name), false),
                    Readable = 1,
                    Writable = 1,
                    Created = GetUnixTimestamp(file.CreationTimeUtc),
                    Modified = GetUnixTimestamp(file.LastWriteTimeUtc),
                    Size = file.Length,
                    Extension = file.Extension.TrimStart('.'),
                    Timestamp = DateTime.Now.Subtract(file.LastWriteTime).TotalSeconds
                }
            }
        };
    }

    private dynamic ReadFolder(string path)
    {
        path ??= string.Empty;

        var rootpath = Path.Combine(webRootPath, path);

        var rootDirectory = new DirectoryInfo(rootpath);
        var data = new List<dynamic>();

        foreach (var directory in rootDirectory.GetDirectories())
        {
            var item = new
            {
                Id = MakeWebPath(Path.Combine(path, directory.Name), false, true),
                Type = "folder",
                Attributes = new
                {
                    directory.Name,
                    Path = MakeWebPath(Path.Combine(webPath, path, directory.Name), true, true),
                    Readable = 1,
                    Writable = 1,
                    Created = GetUnixTimestamp(directory.CreationTime),
                    Modified = GetUnixTimestamp(directory.LastWriteTime),
                    Timestamp = DateTime.Now.Subtract(directory.LastWriteTime).TotalSeconds
                }
            };

            data.Add(item);
        }

        foreach (var file in rootDirectory.GetFiles().OrderByDescending(p => p.LastWriteTime))
        {
            var item = new
            {
                Id = MakeWebPath(Path.Combine(path, file.Name)),
                Type = "file",
                Attributes = new
                {
                    file.Name,
                    Path = MakeWebPath(Path.Combine(webPath, path, file.Name), true),
                    Readable = 1,
                    Writable = 1,
                    Created = GetUnixTimestamp(file.CreationTime),
                    Modified = GetUnixTimestamp(file.LastWriteTime),
                    Extension = file.Extension.Replace(".", ""),
                    Size = file.Length,
                    Timestamp = DateTime.Now.Subtract(file.LastWriteTime).TotalSeconds,
                }
            };

            data.Add(item);
        }

        var result = new
        {
            Data = data
        };

        return result;
    }

    private dynamic AddFolder(string path, string name)
    {
        var newDirectoryPath = Path.Combine(webRootPath, path, name);

        var directoryExist = Directory.Exists(newDirectoryPath);

        if (directoryExist)
        {
            var errorResult = new { Errors = new List<dynamic>() };

            errorResult.Errors.Add(new
            {
                Code = "500",
                Title = "DIRECTORY_ALREADY_EXISTS",
                Meta = new
                {
                    Arguments = new List<string>
                        {
                            name
                        }
                }
            });

            return errorResult;
        }

        Directory.CreateDirectory(newDirectoryPath);
        var directory = new DirectoryInfo(newDirectoryPath);

        var result = new
        {
            Data =
                new
                {
                    Id = MakeWebPath(Path.Combine(path, directory.Name), false, true),
                    Type = "folder",
                    Attributes = new
                    {
                        directory.Name,
                        Path = MakeWebPath(Path.Combine(webPath, path, directory.Name), true, true),
                        Readable = 1,
                        Writable = 1,
                        Created = GetUnixTimestamp(DateTime.Now),
                        Modified = GetUnixTimestamp(DateTime.Now)
                    }
                }
        };

        return result;
    }

    private async Task<dynamic> Upload(string path, IEnumerable<IFormFile> files)
    {
        var result = new { Data = new List<dynamic>() };

        foreach (var file in files)
        {
            if (file.Length <= 0)
            {
                continue;
            }

            var fileExist = System.IO.File.Exists(Path.Combine(webRootPath, path, file.FileName));
            if (fileExist)
            {
                var errorResult = new { Errors = new List<dynamic>() };

                errorResult.Errors.Add(new
                {
                    Code = "500",
                    Title = "FILE_ALREADY_EXISTS",
                    Meta = new
                    {
                        Arguments = new List<string> { file.FileName }
                    }
                });

                return errorResult;
            }

            using (var fileStream = new FileStream(Path.Combine(webRootPath, path, file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            result.Data.Add(new
            {
                Id = MakeWebPath(Path.Combine(path, file.FileName)),
                Type = "file",
                Attributes = new
                {
                    Name = file.FileName,
                    Extension = Path.GetExtension(file.FileName).Replace(".", ""),
                    Path = MakeWebPath(Path.Combine(webPath, path, file.FileName), true),
                    Readable = 1,
                    Writable = 1,
                    Created = GetUnixTimestamp(DateTime.Now),
                    Modified = GetUnixTimestamp(DateTime.Now),
                    Size = file.Length
                }
            });

        }

        return result;
    }

    private dynamic Rename(string old, string @new)
    {
        var oldPath = Path.Combine(webRootPath, old);

        var fileAttributes = System.IO.File.GetAttributes(oldPath);

        if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory) //Fixed if the directory is compressed
        {
            var oldDirectoryName = Path.GetDirectoryName(old).Split('\\').Last();
            var newDirectoryPath = old.Replace(oldDirectoryName, @new);
            var newPath = Path.Combine(webRootPath, newDirectoryPath);

            var directoryExist = Directory.Exists(newPath);

            if (directoryExist)
            {
                var errorResult = new { Errors = new List<dynamic>() };

                errorResult.Errors.Add(new
                {
                    Code = "500",
                    Title = "DIRECTORY_ALREADY_EXISTS",
                    Meta = new
                    {
                        Arguments = new List<string>
                            {
                                @new
                            }
                    }
                });

                return errorResult;
            }


            Directory.Move(oldPath, newPath);

            var result = new
            {
                Data = new
                {
                    Id = newDirectoryPath,
                    Type = "folder",
                    Attributes = new
                    {
                        Name = @new,
                        Readable = 1,
                        Writable = 1,
                        Created = GetUnixTimestamp(DateTime.Now),
                        Modified = GetUnixTimestamp(DateTime.Now)
                    }
                }
            };

            return result;
        }
        else
        {

            var oldFileName = Path.GetFileName(old);
            var newFilePath = old.Replace(oldFileName, @new);
            var newPath = Path.Combine(webRootPath, newFilePath);

            var fileExist = System.IO.File.Exists(newPath);

            if (fileExist)
            {
                var errorResult = new { Errors = new List<dynamic>() };

                errorResult.Errors.Add(new
                {
                    Code = "500",
                    Title = "FILE_ALREADY_EXISTS",
                    Meta = new
                    {
                        Arguments = new List<string>
                            {
                                @new
                            }
                    }
                });

                return errorResult;
            }

            System.IO.File.Move(oldPath, newPath);

            var result = new
            {
                Data = new
                {
                    Id = newFilePath,
                    Type = "file",
                    Attributes = new
                    {
                        Name = @new,
                        Extension = Path.GetExtension(newPath).Replace(".", ""),
                        Readable = 1,
                        Writable = 1,
                        // created date, size vb.
                        Created = GetUnixTimestamp(DateTime.Now),
                        Modified = GetUnixTimestamp(DateTime.Now)
                    }
                }
            };

            return result;
        }
    }

    private dynamic Move(string old, string @new)
    {
        var fileAttributes = System.IO.File.GetAttributes(Path.Combine(webRootPath, old));

        if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory) //Fixed if the directory is compressed
        {
            var directoryName = Path.GetDirectoryName(old).Split('\\').Last();
            var newDirectoryPath = Path.Combine(@new, directoryName);
            var oldPath = Path.Combine(webRootPath, old);
            var newPath = Path.Combine(webRootPath, @new, directoryName);


            var directoryExist = Directory.Exists(newPath);

            if (directoryExist)
            {
                var errorResult = new { Errors = new List<dynamic>() };

                errorResult.Errors.Add(new
                {
                    Code = "500",
                    Title = "DIRECTORY_ALREADY_EXISTS",
                    Meta = new
                    {
                        Arguments = new List<string>
                            {
                                directoryName
                            }
                    }
                });

                return errorResult;
            }

            Directory.Move(oldPath, newPath);

            var result = new
            {
                Data = new
                {
                    Id = newDirectoryPath,
                    Type = "folder",
                    Attributes = new
                    {
                        Name = directoryName,
                        Readable = 1,
                        Writable = 1,
                        Created = GetUnixTimestamp(DateTime.Now),
                        Modified = GetUnixTimestamp(DateTime.Now)
                    }
                }
            };

            return result;
        }
        else
        {
            var fileName = Path.GetFileName(old);
            var newFilePath = Path.Combine(@new, fileName);
            var oldPath = Path.Combine(webRootPath, old);

            var newPath = @new == "/"
                ? Path.Combine(webRootPath, fileName.Replace("/", ""))
                : Path.Combine(webRootPath, @new, fileName);


            var fileExist = System.IO.File.Exists(newPath);

            if (fileExist)
            {
                var errorResult = new { Errors = new List<dynamic>() };

                errorResult.Errors.Add(new
                {
                    Code = "500",
                    Title = "FILE_ALREADY_EXISTS",
                    Meta = new
                    {
                        Arguments = new List<string>
                            {
                                fileName
                            }
                    }
                });

                return errorResult;
            }

            System.IO.File.Move(oldPath, newPath);

            var result = new
            {
                Data = new
                {
                    Id = newFilePath,
                    Type = "file",
                    Attributes = new
                    {
                        Name = fileName,
                        Extension = Path.GetExtension(@new).Replace(".", ""),
                        Readable = 1,
                        Writable = 1,
                        Created = GetUnixTimestamp(DateTime.Now),
                        Modified = GetUnixTimestamp(DateTime.Now)
                    }
                }
            };

            return result;
        }
    }

    private dynamic Copy(string source, string target)
    {
        var fileAttributes = System.IO.File.GetAttributes(Path.Combine(webRootPath, source));

        if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory) //Fixed if the directory is compressed
        {
            var directoryName = Path.GetDirectoryName(source).Split('\\').Last();
            var newDirectoryPath = Path.Combine(target, directoryName);
            var oldPath = Path.Combine(webRootPath, source);
            var newPath = Path.Combine(webRootPath, target, directoryName);


            var directoryExist = Directory.Exists(newPath);

            if (directoryExist)
            {
                var errorResult = new { Errors = new List<dynamic>() };

                errorResult.Errors.Add(new
                {
                    Code = "500",
                    Title = "DIRECTORY_ALREADY_EXISTS",
                    Meta = new
                    {
                        Arguments = new List<string>
                            {
                                directoryName
                            }
                    }
                });

                return errorResult;
            }

            DirectoryCopy(oldPath, newPath);

            var result = new
            {
                Data = new
                {
                    Id = newDirectoryPath,
                    Type = "folder",
                    Attributes = new
                    {
                        Name = directoryName,
                        Readable = 1,
                        Writable = 1,
                        Created = GetUnixTimestamp(DateTime.Now),
                        Modified = GetUnixTimestamp(DateTime.Now)
                    }
                }
            };

            return result;
        }
        else
        {
            var fileName = Path.GetFileName(source);
            var newFilePath = Path.Combine(@target, fileName);
            var oldPath = Path.Combine(webRootPath, source);
            var newPath = Path.Combine(webRootPath, target, fileName);

            var fileExist = System.IO.File.Exists(newPath);

            if (fileExist)
            {
                var errorResult = new { Errors = new List<dynamic>() };

                errorResult.Errors.Add(new
                {
                    Code = "500",
                    Title = "FILE_ALREADY_EXISTS",
                    Meta = new
                    {
                        Arguments = new List<string>
                            {
                                fileName
                            }
                    }
                });

                return errorResult;
            }

            System.IO.File.Copy(oldPath, newPath);

            var result = new
            {
                Data = new
                {
                    Id = newFilePath,
                    Type = "file",
                    Attributes = new
                    {
                        Name = fileName,
                        Extension = Path.GetExtension(fileName).Replace(".", ""),
                        Readable = 1,
                        Writable = 1,
                        // created date, size vb.
                        Created = GetUnixTimestamp(DateTime.Now),
                        Modified = GetUnixTimestamp(DateTime.Now)
                    }
                }
            };

            return result;

        }
    }

    private dynamic SaveFile(string path, string content)
    {
        var filePath = Path.Combine(webRootPath, path);

        System.IO.File.WriteAllText(filePath, content);

        var fileName = Path.GetFileName(path);
        var fileExtension = Path.GetExtension(fileName);

        var result = new
        {
            Data = new
            {
                Id = path,
                Type = "file",
                Attributes = new
                {
                    Name = fileName,
                    Extension = fileExtension,
                    Readable = 1,
                    Writable = 1
                }
            }
        };

        return result;
    }

    private dynamic Delete(string path)
    {
        var fileAttributes = System.IO.File.GetAttributes(Path.Combine(webRootPath, path));

        if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory) //Fixed if the directory is compressed
        {
            var directoryName = Path.GetDirectoryName(path).Split('\\').Last();

            Directory.Delete(Path.Combine(webRootPath, path), true);

            var result = new
            {
                Data = new
                {
                    Id = path,
                    Type = "folder",
                    Attributes = new
                    {
                        Name = directoryName,
                        Readable = 1,
                        Writable = 1,
                        // created date, size vb.
                        Created = GetUnixTimestamp(DateTime.Now),
                        Modified = GetUnixTimestamp(DateTime.Now),
                        Path = path
                    }
                }
            };

            return result;
        }
        else
        {
            var fileName = Path.GetFileName(Path.Combine(webRootPath, path));
            var fileExtension = Path.GetExtension(fileName).Replace(".", "");

            System.IO.File.Delete(Path.Combine(webRootPath, path));

            var result = new
            {
                Data = new
                {
                    Id = path,
                    Type = "file",
                    Attributes = new
                    {
                        Name = fileName,
                        Extension = fileExtension,
                        Readable = 1,
                        Writable = 1,
                        Created = GetUnixTimestamp(DateTime.Now),
                        Modified = GetUnixTimestamp(DateTime.Now)
                        // Path = $"/{fileName}"
                    }
                }
            };

            return result;
        }
    }

    private dynamic ReadFile(string path)
    {
        var filePath = Path.Combine(webRootPath, path);
        var fileName = Path.GetFileName(filePath);
        var fileBytes = System.IO.File.ReadAllBytes(filePath);

        var cd = new ContentDisposition
        {
            Inline = true,
            FileName = fileName
        };
        Response.Headers.ContentDisposition = cd.ToString();

        return File(fileBytes, "application/octet-stream");
    }

    private IActionResult GetImage(string path)
    {
        var filePath = Path.Combine(webRootPath, path);
        var fileName = Path.GetFileName(filePath);
        var fileBytes = System.IO.File.ReadAllBytes(filePath);

        var cd = new ContentDisposition
        {
            Inline = true,
            FileName = fileName
        };
        Response.Headers.ContentDisposition = cd.ToString();

        return File(fileBytes, "image/*");
    }

    private dynamic Download(string path)
    {
        var filePath = Path.Combine(webRootPath, path);
        var fileName = Path.GetFileName(filePath);
        var fileBytes = System.IO.File.ReadAllBytes(filePath);

        return File(fileBytes, "application/x-msdownload", fileName);
    }

    private dynamic Summarize()
    {
        // Get Dir count
        var directories = Directory.GetDirectories(webRootPath, "*", SearchOption.AllDirectories).Length;

        // Get file count
        var directoryInfo = new DirectoryInfo(webRootPath);
        var files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);

        // Get combined file sizes
        var allSize = files.Select(f => f.Length).Sum();

        var result = new
        {
            Data = new
            {
                Id = "/",
                Type = "summary",
                Attributes = new
                {
                    Size = allSize,
                    Files = files.Length,
                    Folders = directories,
                    SizeLimit = 0
                }
            }
        };

        return result;
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName)
    {
        // Get the subdirectories for the specified directory.
        var dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
        }

        var dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        var files = dir.GetFiles();
        foreach (var file in files)
        {
            var temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        foreach (var subdir in dirs)
        {
            var temppath = Path.Combine(destDirName, subdir.Name);
            DirectoryCopy(subdir.FullName, temppath);
        }
    }

    private static string MakeWebPath(string path, bool addSeperatorToBegin = false, bool addSeperatorToLast = false)
    {
        path = path.Replace("\\", "/");

        if (addSeperatorToBegin)
        {
            path = "/" + path;
        }

        if (addSeperatorToLast)
        {
            path += "/";
        }

        return path;
    }
}
