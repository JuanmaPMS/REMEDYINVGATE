using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CategoriasInvgateResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parent_category_id { get; set; }


        //auxiliar
        public List<CategoriaComplemento> diccionario { get; set; }
        public string diccionarioCadena { get; set; }

    }

    public class CategoriaComplemento {
        public int nivel { get; set; }

        public int? idCategoria { get; set; }
        public string? categoria { get; set; }
        
    }


}
