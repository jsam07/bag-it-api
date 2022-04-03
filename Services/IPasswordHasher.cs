namespace bagit_api.Services;

public interface IPasswordHasher
{
    string Hash(string password);
  
    bool Check(string hash, string password);
}