using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IConfigService
    {
        Task<int> AddConfigAsync(string jsonString);
        Task<ConfigItem> GetConfigByIdAsync(int id);
    }
}
