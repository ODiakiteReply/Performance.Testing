using System;
using System.ComponentModel;

namespace Performance.Testing.Common.Enums
{
    public enum RoleEnum
    {
        [Description("Admin")]
        ADMIN = 1,
        [Description("Manager")]
        MANAGER = 2,
        [Description("Tecnical")]
        TECNICAL = 3
    }
}
