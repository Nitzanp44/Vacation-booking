﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class FactoryBL
    {
        public static IBL GetBl()
        {
            return new BL_Class();
        }
    }
}
