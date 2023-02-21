using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Invgate;
using ServiceInvgate;
using TaskIncidencias.Models;

namespace TaskIncidencias
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RegistrosInvgate registros = new RegistrosInvgate();
            ProcesaRegistro procesa = new ProcesaRegistro();

            List<RegistrosRequest> list = registros.Get().OrderBy(x => x.fecha).ToList();

            List<int> procesados = new List<int>();
            foreach(RegistrosRequest req in list)
            {
                Resultado resultado = new Resultado();
                if(req.idTipoSolicitud == 1) //Incidentes
                {
                    switch(req.tipoControl)
                    {
                        //case "ESTATUS":
                        //    resultado = procesa.IncidenteActualiza(req.idIncidencia, req.identificadorNum, Convert.ToInt32(req.identificadorAlfa));
                        //    break;
                        case "CATEGORIZACION":
                            resultado = procesa.IncidenteActualizaCategorizacion(req.idIncidencia, req.identificadorNum);
                            break;
                        case "PRIORIDAD":
                            resultado = procesa.IncidenteActualizaPrioridad(req.idIncidencia, req.identificadorNum);
                            break;
                        case "NOTA":
                            resultado = procesa.IncidenteAdicionaNotas(req.idIncidencia, req.identificadorAlfa);
                            break;
                    }
                }
                else if(req.idTipoSolicitud == 2) //WO
                {
                    switch (req.tipoControl)
                    {
                        case "ESTATUS":
                            resultado = procesa.WOActualiza(req.idIncidencia, req.identificadorNum);
                            break;
                        case "CATEGORIZACION":
                            resultado = procesa.WOActualizaCategorizacion(req.idIncidencia, req.identificadorNum);
                            break;
                        case "PRIORIDAD":
                            resultado = procesa.WOActualizaPrioridad(req.idIncidencia, req.identificadorNum);
                            break;
                        case "NOTA":
                            resultado = procesa.WOAdicionaNotas(req.idIncidencia, req.identificadorAlfa);
                            break;
                    }
                }

                if(resultado.Success)
                    procesados.Add(req.id);
            }

            bool actualiza = false;
            if (procesados.Count > 0)
                actualiza = registros.Procesados(procesados);

            Console.WriteLine(actualiza);
            //Console.ReadKey();
        }
    }
}
