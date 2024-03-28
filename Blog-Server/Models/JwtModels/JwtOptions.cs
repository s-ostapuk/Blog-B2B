namespace Blog_Server.Models.JwtModels
{
    public record class JwtOptions(
      string Issuer,
      string Audience,
      string SigningKey,
      int ExpirationSeconds
  );
}
