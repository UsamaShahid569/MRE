using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class ConfigModel
    {
        public string SendGridKey { get; set; }
        public string SendGridEmail { get; set; }
        public string SendGridName { get; set; }
        public string FrontEndUrl { get; set; }
        public string AppName { get; set; }
        public string AppNameShortcut { get; set; }
        public string AdminEmail { get; set; }

    }
}
