using System;
using System.Collections.Generic;
using System.Text;

namespace FilPan.Settings
{
    public interface IConnSetting
    {
        string Connection { get; set; }

        string MigrationsAssemblyName { get; set; }

        DbServerType DbServer { get; set; }
    }
}
