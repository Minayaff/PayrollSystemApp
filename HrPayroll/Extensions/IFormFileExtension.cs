using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HrPayroll.Extensions
{
    public static class IFormFileExtension
    {

        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType == "image/jpg" ||
                    file.ContentType == "image/jpeg" ||
                     file.ContentType == "image/png" ||
                      file.ContentType == "image/gif";
        }

        public async static Task<string> SaveImage(this IFormFile image, string root)
        {
            string filename = Path.Combine(Guid.NewGuid().ToString() + Path.GetFileName(image.FileName));
            string filepath = Path.Combine(root, "images/user",filename); 
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return filename;

        }

    }
}

