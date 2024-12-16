namespace FinalProject.Api.Helpers
{
	public static class FileMangment
	{
		public static string UploadFile(IFormFile uploadedFile)
		{
			// Define valid extensions
			var validExtentions = new List<string>() { ".pdf", ".jpg", ".jpeg", ".png", ".docx" };
			var extention = Path.GetExtension(uploadedFile.FileName).ToLower();

			// Validate extension
			if (!validExtentions.Contains(extention))
				return null;

			// Validate file size (10 MB limit for images/docx)
			long size = uploadedFile.Length;
			if (size > (10 * 1024 * 1024))
				return null;

			// Generate unique filename
			var filename = Guid.NewGuid() + Path.GetFileName(uploadedFile.FileName);

			// Define the upload path
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

			// Create the directory if it doesn't exist
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			// Save the file
			var filePath = Path.Combine(path, filename);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				uploadedFile.CopyTo(stream);
			}

			// Return relative path
			var relativePath = $"/Uploads/{filename}";
			return relativePath;
		}

	}
}
