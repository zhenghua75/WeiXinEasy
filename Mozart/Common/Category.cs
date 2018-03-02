using System.Collections.Generic;

namespace Mozart.Common
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Module> Modules { get; set; }
    }
}