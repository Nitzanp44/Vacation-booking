﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FactoryDAL
    {
        static Idal dal = null;
        public static Idal GetDal()
        {
            if (dal == null)
               // dal = new Dal_imp();
                dal = new Dal_XML_imp();
               
            return dal;
        }
    }
}
