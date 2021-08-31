
using System;
using System.Text;

namespace ForksWebAPI.Common.Client
{
  public static class Archiver
  {
    public static string Encode(string source)
    {
      source = Archiver.Base64Encode(source);
      StringBuilder stringBuilder = new StringBuilder(source.Length);
      foreach (char ch in source)
        stringBuilder.Append((char) ((uint) ch ^ 4U));
      return stringBuilder.ToString();
    }

    public static string Decode(string source)
    {
      StringBuilder stringBuilder = new StringBuilder(source.Length);
      foreach (char ch in source)
        stringBuilder.Append((char) ((uint) ch ^ 4U));
      return Archiver.Base64Decode(stringBuilder.ToString());
    }

    private static string Base64Encode(string plainText)
    {
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    }

    private static string Base64Decode(string base64EncodedData)
    {
      return Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
    }
  }
}
