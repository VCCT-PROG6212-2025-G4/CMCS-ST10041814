using CMCS.Services.Interfaces;

namespace CMCS.Services
{
    //all code been checked and efficiency by the Chatgpt, original code was brainstorm and gide by the chat gpt
    public class ClaimFileService : IClaimFileService
    {
        private readonly IWebHostEnvironment _env;

        public ClaimFileService(IWebHostEnvironment env)
        {
            _env = env;
        }
        // get the folder create by the claim id
        private string GetClaimFolder(int claimId)
        {
            // get the path
            var path = Path.Combine(
                _env.WebRootPath,
                "Claims",
                claimId.ToString());
            // if the path is not exist then create a folder by this path
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
        // upload files
        public void Upload(int claimId, IFormFile file)
        {
            // save the file in wwwroot/claim/...
            Console.WriteLine(_env.WebRootPath);
            var folder = GetClaimFolder(claimId);

            var filePath = Path.Combine(folder, file.FileName);

            // save the file, create and copy it in that path by the provided claim id
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);
        }
        // get download files path
        public IEnumerable<string> GetFiles(int claimId)
        {
            var folder = GetClaimFolder(claimId);

            return Directory.GetFiles(folder)
                .Select(Path.GetFileName);
        }
        // get the claim id folder and path then delete it
        public void Delete(int claimId, string fileName)
        {
            var folder = GetClaimFolder(claimId);
            var filePath = Path.Combine(folder, fileName);
            // if exist then delete the file
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
