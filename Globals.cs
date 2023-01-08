﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModdingTool
{
    internal class Globals
    {
        public static string modPath;
        public static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static int projectileDelayStandard = 0;
        public static int projectileDelayFlaming = 0;
        public static int projectileDelayGunpowder = 0;

    }
}
