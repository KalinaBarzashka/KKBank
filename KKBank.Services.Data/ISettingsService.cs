using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KKBank.Services.Data
{
    public interface ISettingsService
    {
        public Task CreateRole(string roleName);
    }
}
