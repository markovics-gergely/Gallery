using Gallery.BLL.Validators.Interfaces;
using Gallery.DAL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Validators.Implementations
{
    public class AdminRoleValidator : IValidator
    {
        private readonly ClaimsPrincipal user;

        public AdminRoleValidator(ClaimsPrincipal user)
        {
            this.user = user;
        }

        public bool Validate()
            =>  user.IsInRole(RoleTypes.Admin);
    }
}
