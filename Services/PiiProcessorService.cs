using PiiSignalRDemo.Models;
using System.Text.RegularExpressions;

namespace PiiSignalRDemo.Services
{
    public class PiiProcessorService
    {
        public async Task<PiiResult> ProcessAsync(PiiRequest request)
        {
            await Task.Delay(2000);
            var aadhaarRegex = new Regex(@"\b[2-9]\d{3}\s?\d{4}\s?\d{4}\b");

            bool isValid = aadhaarRegex.IsMatch(request.Text);

            bool containsPii =
                request.Text.Contains("ssn", StringComparison.OrdinalIgnoreCase) ||
                request.Text.Contains("aadhaar", StringComparison.OrdinalIgnoreCase) ||
                isValid;
            return new PiiResult
            {
                Id = request.Id,
                ContainsPII = containsPii,
                Message = containsPii ? "Sensitive Data Found" : "Safe Data",
                RawPrompt = request.Text
            };
        }
    }
}
