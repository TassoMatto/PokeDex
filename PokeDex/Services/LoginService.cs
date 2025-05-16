namespace PokeDex.Services;

public class LoginService : ILoginService
{
    public bool CheckAutentication(string username, string password)
    {
        // Check if args are invalid
        if (username == "" || password == "") return false;
        
        // Fake login - Password not cripted and username standard
        return (username == "admin" && password == "admin");
    }
}