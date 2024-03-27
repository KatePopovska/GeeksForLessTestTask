using BLL.Services.Interfaces;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ConfigService : IConfigService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ConfigService> _logger;
        private readonly IConfigParser _configParser;

        public ConfigService(ApplicationDbContext context, ILogger<ConfigService> logger, IConfigParser configParser)
        {
            _context = context;
            _logger = logger;
            _configParser = configParser;
        }

        public async Task<int> AddConfigAsync(string jsonString)
        {
            JToken jsonToken = JToken.Parse(jsonString);
            ConfigItem rootConfigItem = _configParser.ParseJToken(jsonToken);

            var entity = _context.Set<ConfigItem>().Add(rootConfigItem);
            await _context.SaveChangesAsync();

            return entity.Entity.Id;
        }

        public async Task<ConfigItem> GetConfigByIdAsync(int id)
        {
            var isExist = await _context.ConfigItems.AnyAsync(x => x.Id == id);
            if (!isExist)
            {
                throw new ArgumentException($"Configuration with id {id} doesn't exists");
            }

            ConfigItem configItem = await _context.ConfigItems
                .Include(x => x.Childrens)
                .FirstOrDefaultAsync(x => x.Id == id);

            await LoadChildrensAsync(configItem.Childrens);
            return configItem;       
        }

        private async Task LoadChildrensAsync(List<ConfigItem> childrens)
        {
            foreach (var node in childrens)
            {
                await _context.Entry(node)
                    .Collection(x => x.Childrens)
                    .LoadAsync();

                if (node.Childrens.Any())
                {
                    await LoadChildrensAsync(node.Childrens);
                }
            }
        }
    }
}
