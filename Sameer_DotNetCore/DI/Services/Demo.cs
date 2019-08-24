using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI.Services
{
    public class Demo
    {
        public string GetMessageDemo(string Name)
        {
            return $"Hello {Name} from ADODataManger";
        }
    }
}
