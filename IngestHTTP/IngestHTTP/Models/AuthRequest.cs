﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngestHTTP.Models
{
    public class AuthRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public int companyid { get; set; }
    }
}