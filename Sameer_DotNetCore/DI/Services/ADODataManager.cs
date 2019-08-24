using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionDemo.Services
{
    public class ADODataManager :IDataManger
    {
       public string GetMessage(string Name)
        {
            return $"Hello {Name} from ADODataManger";
        }
    }
}
