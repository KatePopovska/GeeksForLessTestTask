using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ConfigItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public int? ParentId { get; set; }
        public ConfigItem? Parent { get; set; }
        public List<ConfigItem> Childrens { get; set; } = new List<ConfigItem>();

    }
}
