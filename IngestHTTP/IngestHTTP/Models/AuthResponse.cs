﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngestHTTP.Models
{
    public class AuthResponse
    {
        public string createdAt { get; set; }
        public string expiresOn { get; set; }
        public int id { get; set; }
        public string companyid { get; set; }
        public string jwtToken { get; set; }
    }
}
