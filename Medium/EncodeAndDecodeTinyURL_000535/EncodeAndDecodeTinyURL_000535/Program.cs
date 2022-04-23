// https://leetcode.com/problems/encode-and-decode-tinyurl/ for more information

//Note: This is a companion problem to the System Design problem: Design TinyURL.
//TinyURL is a URL shortening service where you enter a URL such as https://leetcode.com/problems/design-tinyurl
//and it returns a short URL such as http://tinyurl.com/4e9iAk. Design a class to encode a URL and decode a tiny URL.

//There is no restriction on how your encode/decode algorithm should work.
//You just need to ensure that a URL can be encoded to a tiny URL and the tiny URL can be decoded to the original URL.

//Implement the Solution class:
//Solution() Initializes the object of the system.
//String encode(String longUrl) Returns a tiny URL for the given longUrl.
//String decode(String shortUrl) Returns the original long URL for the given shortUrl.It is guaranteed that the given shortUrl was encoded by the same object.

//Example 1:
//Input: url = "https://leetcode.com/problems/design-tinyurl"
//Output: "https://leetcode.com/problems/design-tinyurl"

//Explanation:
//Solution obj = new Solution();
//string tiny = obj.encode(url); // returns the encoded tiny url.
//string ans = obj.decode(tiny); // returns the original url after deconding it.

//Constraints:
//1 <= url.length <= 10^4
//url is guranteed to be a valid URL.
Console.WriteLine("Hello, World!");



public class Codec
{
    private const string Prefix = "http://tinyurl.com/";

    private readonly Dictionary<string, string> _shortToLongMap = new(StringComparer.OrdinalIgnoreCase);

    // Encodes a URL to a shortened URL
    public string encode(string longUrl)
    {
        if (string.IsNullOrWhiteSpace(longUrl))
            return longUrl;

        // TODO: fix hashes collision?
        // TODO: use longer hash?
        // TODO: use not only hexadecimal symbols but all english letters & digits?
        // Ideally, we should expose hash as a string of N characters, where N is for example [6..9].
		// TODO: as it mentioned at Leetcode System Design task, may be it is better to use X-based numbers? Note that URLs in general are case-sensitive (except for domain part).
        var hash = GetHashCode(longUrl);
        var hashString = hash.ToString("X4");
        var shortUrl = Prefix + hashString;
        _shortToLongMap[shortUrl] = longUrl;
        return shortUrl;
    }

    // Decodes a shortened URL to its original URL.
    public string decode(string shortUrl)
    {
        if (shortUrl == null)
            return null;
        if (!shortUrl.StartsWith(Prefix, StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException("Was not encoded by this service.", nameof(shortUrl));

        if (_shortToLongMap.TryGetValue(shortUrl, out var longUrl))
            return longUrl;

        return null;
    }

    private static long GetHashCode(string s)
    {
        if (s == null || s.Length == 1)
            return 0;

        var middleIndex = s.Length / 2;
        var (s1, s2) = (s.Substring(startIndex: 0, length: middleIndex), s.Substring(startIndex: middleIndex, length: s.Length - middleIndex));
        var (hash1, hash2) = (s1.GetHashCode(), s2.GetHashCode());
        return (long)hash1 | ((long)hash2) << 32;
    }
}

// Your Codec object will be instantiated and called as such:
// Codec codec = new Codec();
// codec.decode(codec.encode(url));