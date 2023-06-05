using FlightDoc_Syste.Model;
using Microsoft.AspNetCore.Identity;

namespace FlightDoc_Syste.Repositories
{
    public interface IAccountRepository
    {

        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
        public Task<int> SignOutAsync();

    }
}
