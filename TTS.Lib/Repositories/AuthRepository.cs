using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TTS.Identity.Models;
using System;
using System.Collections.Generic;

namespace TTS.Lib.Repositories
{
    public class AuthRepository : UserManager<ApplicationUser>
    {
        public AuthRepository(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, 
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
        
    }
}
