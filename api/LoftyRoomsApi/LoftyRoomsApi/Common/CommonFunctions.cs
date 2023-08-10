using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LoftyRoomsApi.Common
{
    public class CommonFunctions
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public CommonFunctions(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }
        public string UploadPromotionalImage(IFormFile fle)
        {
            string filePath = "";
            if (fle != null)
            {
                var NewFolderName = "PromotionalImage/" + DateTime.Now.ToString("yyyyMMdd");
                var folderName = Path.Combine("Files", NewFolderName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!System.IO.Directory.Exists(pathToSave))
                {
                    System.IO.Directory.CreateDirectory(pathToSave);
                }

                string fileName = Path.GetFileNameWithoutExtension(fle.FileName);
                string extension = Path.GetExtension(fle.FileName);
                fileName = fileName + DateTime.Now.ToString("yyyyMMddHHmm") + extension;
                var fPath = Path.Combine(pathToSave, fileName);
                filePath = "/Files/" + NewFolderName + "/" + fileName;
                using (var stream = new FileStream(fPath, FileMode.Create))
                {
                    fle.CopyTo(stream);
                }
            }
            return filePath;
        }
        public string UploadAdImage1(IFormFile fle)
        {
            string filePath = "";
            if (fle != null)
            {
                var NewFolderName = "AdsImage/" + DateTime.Now.ToString("yyyyMMdd");
                var folderName = Path.Combine("Files", NewFolderName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!System.IO.Directory.Exists(pathToSave))
                {
                    System.IO.Directory.CreateDirectory(pathToSave);
                }

                string fileName = Path.GetFileNameWithoutExtension(fle.FileName);
                string extension = Path.GetExtension(fle.FileName);
                fileName = fileName + DateTime.Now.ToString("yyyyMMddHHmm") + extension;
                var fPath = Path.Combine(pathToSave, fileName);
                filePath = "/Files/" + NewFolderName + "/" + fileName;
                using (var stream = new FileStream(fPath, FileMode.Create))
                {
                    fle.CopyTo(stream);
                }
            }
            return filePath;
        }
        public string UploadFacilityImage(IFormFile fle)
        {
            string filePath = "";
            if (fle != null)
            {
                var NewFolderName = "FacilityImage/" + DateTime.Now.ToString("yyyyMMdd");
                var folderName = Path.Combine("Files", NewFolderName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!System.IO.Directory.Exists(pathToSave))
                {
                    System.IO.Directory.CreateDirectory(pathToSave);
                }

                string fileName = Path.GetFileNameWithoutExtension(fle.FileName);
                string extension = Path.GetExtension(fle.FileName);
                fileName = fileName + DateTime.Now.ToString("yyyyMMddHHmm") + extension;
                var fPath = Path.Combine(pathToSave, fileName);
                filePath = "/Files/" + NewFolderName + "/" + fileName;
                using (var stream = new FileStream(fPath, FileMode.Create))
                {
                    fle.CopyTo(stream);
                }
            }
            return filePath;
        }
        public string UploadProfileImage(IFormFile fle)
        {
            string filePath = "";
            if (fle != null)
            {
                var NewFolderName = "ProfileImage/" + DateTime.Now.ToString("yyyyMMdd");
                var folderName = Path.Combine("Files", NewFolderName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!System.IO.Directory.Exists(pathToSave))
                {
                    System.IO.Directory.CreateDirectory(pathToSave);
                }

                string fileName = Path.GetFileNameWithoutExtension(fle.FileName);
                string extension = Path.GetExtension(fle.FileName);
                fileName = fileName + DateTime.Now.ToString("yyyyMMddHHmm") + extension;
                var fPath = Path.Combine(pathToSave, fileName);
                filePath = "/Files/" + NewFolderName + "/" + fileName;
                using (var stream = new FileStream(fPath, FileMode.Create))
                {
                    fle.CopyTo(stream);
                }
            }
            return filePath;
        }
        public byte[] CreateZipFromFolder(string folderPath)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    // Recursively add all files and subdirectories to the ZIP archive
                    foreach (string filePath in Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories))
                    {
                        string relativePath = filePath.Substring(folderPath.Length + 1); // Get relative path inside the ZIP archive
                        zipArchive.CreateEntryFromFile(filePath, relativePath);
                    }
                }

                return memoryStream.ToArray();
            }
        }

    }
}
