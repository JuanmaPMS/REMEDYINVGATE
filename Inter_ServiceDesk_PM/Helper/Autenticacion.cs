using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Inter_ServiceDesk_PM.Helper
{
    public class Autenticacion:System.Web.Services.Protocols.SoapHeader
    {
        [XmlElement(IsNullable = true)]
        public string User { get; set; }

        [XmlElement(IsNullable = true)]
        public string Password { get; set; }

        public bool IsValid()
        {
            string _user = ConfigurationManager.AppSettings["user_imss"];
            string _pass = ConfigurationManager.AppSettings["password_imss"];

            return this.User == _user && this.Password== _pass; 

        }
            
    }
}