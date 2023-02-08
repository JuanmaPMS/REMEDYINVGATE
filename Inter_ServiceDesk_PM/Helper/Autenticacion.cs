using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Inter_ServiceDesk_PM.Helper
{
    public class Autenticacion:System.Web.Services.Protocols.SoapHeader
    {
        public string User { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            string _user = ConfigurationManager.AppSettings["user_imss"];
            string _pass = ConfigurationManager.AppSettings["password_imss"];

            return this.User == _user && this.Password== _pass; 

        }
            
    }
}