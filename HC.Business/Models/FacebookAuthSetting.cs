using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HC.Business.Models
{
    public class FacebookAuthSetting
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
