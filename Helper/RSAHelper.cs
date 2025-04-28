using System.Numerics;

namespace RSADemo.Helper;

public class RSAHelper
{
    private const int P = 19;
    private const int Q = 89;

    public const string SEP = "#";
    
    public int KeyN { get; set; }
    public int KeyE { get; set; }
    public int KeyD { get; set; }
    
    // C <-> M: e || d with n 

    public static string Encrypt(string raw, int e, int n)
    {
        string buffer = "X";
        foreach (var c in raw)
        {
            buffer += $"{SEP}{BigInteger.ModPow(c, e, n)}";
        }

        return buffer.Replace($"X{SEP}", "");
    }

    public static string Decrypt(string was, int d, int n)
    {
        if (was == "") return "";
        var sections = was.Split(SEP);
        string buffer = "";
        foreach (var block in sections)
        {
            buffer += $"{(char)(BigInteger.ModPow(int.Parse(block), d, n))}";
        }

        return buffer;
    }

    public (string publicKey, string privateKey) generateKey(bool useFallback = false)
    {
        if (!useFallback)
        {
            this.KeyE = (new Random()).Next(10000);
            this.KeyE /= 15;
            if (this.KeyE % 2 == 0) this.KeyE -= 1;
        }
        else KeyE = 5;
        this.KeyN = P * Q;
        this.KeyD = ModInverse(KeyE, ((P - 1) * (Q - 1)));
        
        return ($"({this.KeyN}, {this.KeyE})", $"({this.KeyN}, {this.KeyD})");
    }
    
    
    
    
    
    
    static (int gcd, int x, int y) ExtendedGCD(int a, int b)
    {
        if (b == 0)
            return (a, 1, 0); // gcd(a, 0) = a, x = 1, y = 0

        var (gcd, x1, y1) = ExtendedGCD(b, a % b);
        int x = y1;
        int y = x1 - (a / b) * y1;

        return (gcd, x, y);
    }

    // 计算 e 的模逆元
    static int ModInverse(int e, int phi)
    {
        var (gcd, x, _) = ExtendedGCD(e, phi);

        if (gcd != 1)
            throw new ArgumentException($"e and phi are not coprime, modular inverse does not exist. gcd: {gcd}, phi: {phi}, e: {e}");

        // 确保结果是正数
        return (x % phi + phi) % phi;
    }

}