using System;
using System.Collections.Generic;
using System.Text;

namespace Swastika.Infrastructure.CrossCutting.Identity.Data
{
    public static class ClaimData
    {
        public static List<string> UserClaims { get; set; } = new List<string>
                                                            {
                                                                "Add User",
                                                                "Edit User",
                                                                "Delete User"
                                                            };
    }
}
