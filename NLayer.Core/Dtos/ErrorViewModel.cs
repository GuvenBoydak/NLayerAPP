﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
            Errors = new List<string>();
        }
        public List<string> Errors { get; set; }
    }
}