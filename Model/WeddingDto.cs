﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuierobesarteApp.Model
{
    public class WeddingDto
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }

        public string QrCodeImageName { get; set; }
    }
}
