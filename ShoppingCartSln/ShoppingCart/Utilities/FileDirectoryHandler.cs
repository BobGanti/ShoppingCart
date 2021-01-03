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
        private readonly string itemImageFolder, itemClassFolder, mainImageFolder, mainImageThumbFolder, galleryFolder, galleryThumbsFolder;
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
            this.itemImageFolder = itemImageFolder;
            this.upFileRootDirectories = upFileRootDirectories;
            this.file = file;

            itemClassFolder = Path.Combine(this.upFileRootDirectories.ToString(), this.itemImageFolder);
            mainImageFolder = Path.Combine(this.upFileRootDirectories.ToString(), this.itemImageFolder + "\\" + this.id.ToString());
            mainImageThumbFolder = Path.Combine(this.upFileRootDirectories.ToString(), this.itemImageFolder + "\\" + this.id.ToString() + "\\Thumbs");
            galleryFolder = Path.Combine(this.upFileRootDirectories.ToString(), this.itemImageFolder + "\\" + this.id.ToString() + "\\Gallery");
            galleryThumbsFolder = Path.Combine(this.upFileRootDirectories.ToString(), this.itemImageFolder + "\\" + this.id.ToString() + "\\Gallery\\Thumbs");
        }

        public string SetFileDir()
        {
            if (!Directory.Exists(itemClassFolder))
                Directory.CreateDirectory(itemClassFolder);

            if (!Directory.Exists(mainImageFolder))
                Directory.CreateDirectory(mainImageFolder);

            if (!Directory.Exists(mainImageThumbFolder))
                Directory.CreateDirectory(mainImageThumbFolder);

            if (!Directory.Exists(galleryFolder))
                Directory.CreateDirectory(galleryFolder);

            if (!Directory.Exists(galleryThumbsFolder))
                Directory.CreateDirectory(galleryThumbsFolder);

            if (!VerifyFileExtension(this.file))
                return "Wrong file type";

            SaveFilesToDir(mainImageFolder, mainImageThumbFolder);

            return "OK";
        }

        public string UpdateFilesInDir()
        {
            if (!VerifyFileExtension(this.file))
                return "Wrong file type";

            // Delete existing image files
            DirectoryInfo imgFolder = new DirectoryInfo(mainImageFolder);
            foreach (FileInfo image_file in imgFolder.GetFiles())
            {
                image_file.Delete();
            }

            DirectoryInfo imgThumbFolder = new DirectoryInfo(mainImageThumbFolder);
            foreach (FileInfo image_file in imgThumbFolder.GetFiles())
            {
                image_file.Delete();
            }

            return SaveFilesToDir(mainImageFolder, mainImageThumbFolder);
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

        public string SaveGalleryImages()
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