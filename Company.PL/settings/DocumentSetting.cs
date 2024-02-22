using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Company.PL.settings
{
    public static class DocumentSetting
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            var FolderPath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\File", folderName);
            var fileName=$"{Guid.NewGuid()}{file.FileName}";
            var filePath=Path.Combine(FolderPath, fileName);
            using var FS=new FileStream(filePath, FileMode.Create);
            file.CopyTo(FS);
            return fileName;

        }
        public static void DeleteFile(string fileName,string folderName)
        {
           string filePath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\File",folderName,fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
