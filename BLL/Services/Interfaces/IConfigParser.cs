using DAL.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IConfigParser
    {
        ConfigItem ParseJToken(JToken token, ConfigItem parent = null);
    }
}
