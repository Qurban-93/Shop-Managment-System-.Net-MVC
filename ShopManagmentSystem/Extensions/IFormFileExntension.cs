namespace ShopManagmentSystem.Extensions
{
    public static class IFormFileExntension
    {

        public static bool CheckImageType(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }

        public static bool CheckImageSize(this IFormFile file, int size)
        {
            return file.Length / 1024 / 1024 < size;
        }

        public static string SaveImage(this IFormFile file, IWebHostEnvironment env, string root)
        {
            string fileName = string.Concat(Guid.NewGuid(), file.FileName);
            string fullPath = Path.Combine(env.WebRootPath, root, fileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        public static string SaveImage(this IFormFile file, IWebHostEnvironment env, string root ,string rootSecond)
        {
            string fileName = string.Concat(Guid.NewGuid(), file.FileName);
            string fullPath = Path.Combine(env.WebRootPath, root, rootSecond, fileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

    }
}
