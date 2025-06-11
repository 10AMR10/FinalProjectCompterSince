using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FinalProject.EF.Translation
{
	public class TranslationService
	{
		private readonly HttpClient _httpClient;
		private const string ApiKey = "15de41c1beef53eba28f"; // Replace with your actual API key

		public TranslationService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<string> TranslateLongTextAsync(string text, string sourceLang, string targetLang)
		{
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Text to translate cannot be null or empty.");

			if (string.IsNullOrWhiteSpace(sourceLang) || string.IsNullOrWhiteSpace(targetLang))
				throw new ArgumentException("Source and target languages cannot be null or empty.");

			// Split the text into chunks
			const int maxChunkSize = 500; // MyMemory API limit per request
			var chunks = SplitIntoChunks(text, maxChunkSize);

			var translatedChunks = new List<string>();

			// Translate each chunk and combine results
			foreach (var chunk in chunks)
			{
				var translatedChunk = await TranslateTextAsync(chunk, sourceLang, targetLang);
				translatedChunks.Add(translatedChunk);
			}

			return string.Join(" ", translatedChunks);
		}

		private async Task<string> TranslateTextAsync(string text, string sourceLang, string targetLang)
		{
			// Build the request URL
			var encodedText = HttpUtility.UrlEncode(text);
			var langPair = $"{sourceLang}|{targetLang}";
			var url = $"https://api.mymemory.translated.net/get?q={encodedText}&langpair={langPair}&key={ApiKey}";

			// Send GET request
			var response = await _httpClient.GetAsync(url);

			if (!response.IsSuccessStatusCode)
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new HttpRequestException($"Translation API error: {response.StatusCode}, Details: {errorContent}");
			}

			// Parse the response and extract only the translated text
			var result = await response.Content.ReadFromJsonAsync<MyMemoryResponse>();
			return result?.ResponseData?.TranslatedText ?? string.Empty;
		}

		private IEnumerable<string> SplitIntoChunks(string text, int maxChunkSize)
		{
			for (int i = 0; i < text.Length; i += maxChunkSize)
			{
				yield return text.Substring(i, Math.Min(maxChunkSize, text.Length - i));
			}
		}

		private class MyMemoryResponse
		{
			public ResponseData ResponseData { get; set; }
		}

		private class ResponseData
		{
			public string TranslatedText { get; set; }
		}
	}
}
