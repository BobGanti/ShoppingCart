using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ShoppingCart.Utilities
{
    public class FileDirectoryHandler : Controller
    {
        private readonly int id;
        private readonly string productIdFolder, productClassFolder, mainFileFolder, mainFileThumbFolder, galleryFolder, galleryThumbsFolder;
        private readonly HttpPostedFileBase file;
        private readonly DirectoryInfo upFileRootDirectories;

        public FileDirectoryHandler()
        { }

        public FileDirectoryHandler(
            int id, 
            string itemImageFolder, 
            DirectoryInfo upFileRootDirectories, 
            HttpPostedFileBase file)
        {
            this.id = id;
            this.productIdFolder = itemImageFolder;
            this.upFileRootDirectories = upFileRootDirectories;
            this.file = file;

            productClassFolder = Path.Combine(this.upFileRootDirectories.ToString(), this.productIdFolder);
            mainFileFolder = Path.Combine(this.upFileRootDirectories.ToString(), this.productIdFolder + "\\" + this.id.ToString());
            mainFileThumbFolder = Path.Combine(this.upFileRootDirectories.ToString(), this.productIdFolder + "\\" + this.id.ToString() + "\\Thumbs");
            galleryFolder = Path.Combine(this.upFileRootDirectories.ToString(), this.productIdFolder + "\\" + this.id.ToString() + "\\Gallery");
            galleryThumbsFolder = Path.Combine(this.upFileRootDirectories.ToString(), this.productIdFolder + "\\" + this.id.ToString() + "\\Gallery\\Thumbs");
        }

        public string SetFileDir()
        {
            if (!Directory.Exists(productClassFolder))
                Directory.CreateDirectory(productClassFolder);

            if (!Directory.Exists(mainFileFolder))
                Directory.CreateDirectory(mainFileFolder);

            if (!Directory.Exists(mainFileThumbFolder))
                Directory.CreateDirectory(mainFileThumbFolder);

            if (!Directory.Exists(galleryFolder))
                Directory.CreateDirectory(galleryFolder);

            if (!Directory.Exists(galleryThumbsFolder))
                Directory.CreateDirectory(galleryThumbsFolder);

            if (!VerifyFileExtension(this.file))
                return "Wrong file type";

            SaveFilesToDir(mainFileFolder, mainFileThumbFolder);

            return "OK";
        }

        public string UpdateFilesInDir()
        {
            if (!VerifyFileExtension(this.file))
                return "Wrong file type";

            // Delete existing image files
            DirectoryInfo imgFolder = new DirectoryInfo(mainFileFolder);
            foreach (FileInfo image_file in imgFolder.GetFiles())
            {
                image_file.Delete();
            }

            DirectoryInfo imgThumbFolder = new DirectoryInfo(mainFileThumbFolder);
            foreach (FileInfo image_file in imgThumbFolder.GetFiles())
            {
                image_file.Delete();
            }

            return SaveFilesToDir(mainFileFolder, mainFileThumbFolder);
        }

        public bool DeleteFolder(string mainImageFolder)
        {
            try
            {
                if (Directory.Exists(mainImageFolder))
                {
                    Directory.Delete(mainImageFolder, true);
                }
                return true;               
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string SaveGalleryFiles()
        {
            if (!VerifyFileExtension(this.file))
                return "Wrong file type";

            return SaveFilesToDir(galleryFolder, galleryThumbsFolder);
        }

        public void DeleteGalleryFiles(string galleryPath, string thumbsPath)
        {
            if (System.IO.File.Exists(galleryPath))
                System.IO.File.Delete(galleryPath);

            if (System.IO.File.Exists(thumbsPath))
                System.IO.File.Delete(thumbsPath);
        }

        private bool VerifyFileExtension(HttpPostedFileBase file)
        {
            string ext = this.file.ContentType.ToLower();

            if (ext != "image/jpg" &&
                ext != "image/jpeg" &&
                ext != "image/pjpeg" &&
                ext != "image/gif" &&
                ext != "image/x-png" &&
                ext != "image/png"
                )
                return false;

            return true;
        }

        private string SaveFilesToDir(string folder1, string folder2)
        {
            // Save new image files in the main & thumbs image folders
            string imageName = file.FileName;
            string mainPath = string.Format("{0}\\{1}", folder1, imageName);
            file.SaveAs(mainPath);

            string thumbsPath = string.Format("{0}\\{1}", folder2, imageName);
            var img = new WebImage(file.InputStream);
            img.Resize(75, 75);
            img.Save(thumbsPath);

            return "OK";
        }
    }
}