namespace PokeDex.Services;

public interface ILoginService
{
    public bool CheckAutentication(string username, string password);
}