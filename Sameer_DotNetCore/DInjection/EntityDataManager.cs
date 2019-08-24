using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DInjection
{
    public class EntityDataManager :IDataManger
    {
        public string GetMessage(string Name)
        {
            return $"Hello {Name} fromEntityDataManger";
        }
    }
}
