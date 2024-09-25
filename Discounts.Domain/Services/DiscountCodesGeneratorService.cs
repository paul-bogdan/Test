using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace Discounts.Domain.Services;

public static class DiscountCodesGeneratorService 
{
     private static readonly char[] Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private static readonly int RandomLength = 3; // Number of random characters
    private static readonly ConcurrentDictionary<string, bool> GeneratedCodes = new ConcurrentDictionary<string, bool>();

    // Method to generate unique discount codes based on time
    public static List<string> GenerateDiscountCodes(int numberOfCodes)
    {
        if (numberOfCodes < 1 || numberOfCodes > 2000)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfCodes), "Number of codes should be between 1 and 2000.");
        }

        var codes = new List<string>();

        for (int i = 0; i < numberOfCodes; i++)
        {
            string code;
            do
            {
                code = GenerateTimeBasedCode();
            }
            while (!GeneratedCodes.TryAdd(code, true)); // Ensure the code is unique

            codes.Add(code);
        }

        return codes;
    }

    // Helper method to generate a time-based code
    private static string GenerateTimeBasedCode()
    {
        // Get Unix timestamp (seconds since 1970-01-01)
        long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Convert the timestamp to a base-36 string (letters and numbers) for shorter representation
        string timePart = ConvertToBase36(unixTimestamp);

        // Generate a random string part
        string randomPart = GenerateRandomString(RandomLength);

        // Combine time and random parts
        return timePart + randomPart;
    }

    // Helper method to convert a number to base-36
    private static string ConvertToBase36(long value)
    {
        const string base36Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var result = new StringBuilder();
        
        do
        {
            result.Insert(0, base36Chars[(int)(value % 36)]);
            value /= 36;
        }
        while (value > 0);

        return result.ToString();
    }

    // Helper method to generate a random string
    private static string GenerateRandomString(int length)
    {
        var result = new StringBuilder(length);
        using (var rng =  RandomNumberGenerator.Create())
        {
            var buffer = new byte[length];
            rng.GetBytes(buffer);

            for (int i = 0; i < length; i++)
            {
                result.Append(Characters[buffer[i] % Characters.Length]);
            }
        }
        return result.ToString();
    }
}

