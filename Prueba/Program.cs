using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceIMSS;
using System.Configuration;
using System.Net;
using System.IO;

namespace Prueba
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Categorizacion categorizacion = new Categorizacion();
            Incidentes incidentes = new Incidentes();

            categorizacion.IDTicketRemedy = "INC840214";
            categorizacion.IDTicketInvgate = "0001";
            categorizacion.CatOperacion01 = "APLICACIONES";
            categorizacion.CatOperacion02 = "ATENCION DE APLICACIONES";
            categorizacion.CatOperacion03 = "SOPORTAR";
            categorizacion.CatProducto01 = "SW MEDICO";
            categorizacion.CatProducto02 = "PROVISION DE SERVICIOS MEDICOS";
            categorizacion.CatProducto03 = "DEVENGO";


            Result result = new Result();

            result = incidentes.ActualizaCategorizacion(categorizacion);
           
            Console.WriteLine(result.Resultado);

            Console.ReadKey();

        }
    }
}
