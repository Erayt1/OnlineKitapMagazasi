using OnlineKitapMagazasi.API.Models;

namespace OnlineKitapMagazasi.API.Services
{
    public interface ITokenService
    {
        string TokenUret(Kullanici kullanici);
    }
}
