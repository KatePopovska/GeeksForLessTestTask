using BLL.Services.Interfaces;
using DAL.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Services
{
    public class ConfigParser : IConfigParser
    {
        public ConfigItem ParseJToken(JToken token, ConfigItem parent = null)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            var configItem = new ConfigItem
            {
                Parent = parent,
                ParentId = parent?.Id
            };

            switch (token.Type)
            {
                case JTokenType.Object:
                    var obj = (JObject)token;
                    foreach (var property in obj.Properties())
                    {
                        var childConfigItem = ParseJToken(property.Value, configItem);
                        childConfigItem.Name = property.Name;
                        configItem.Childrens.Add(childConfigItem);
                    }
                    break;

                case JTokenType.Array:
                    var array = (JArray)token;
                    for (int i = 0; i < array.Count; i++)
                    {
                        var childConfigItem = ParseJToken(array[i], configItem);
                        childConfigItem.Name = $"Item_{i + 1}";
                        configItem.Childrens.Add(childConfigItem);
                    }
                    break;

                case JTokenType.String:
                    configItem.Value = token.Value<string>();
                    break;

                default:
                    throw new ArgumentException($"Unsupported token type: {token.Type}");
            }

            return configItem;
        }
    }
}
