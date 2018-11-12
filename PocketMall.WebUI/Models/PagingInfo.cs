﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PocketMall.WebUI.Models
{
    public class PagingInfo
    {
        public int TotalIteams { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalIteams / ItemsPerPage);
            }
        }
    }
}