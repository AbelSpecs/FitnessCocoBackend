using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InsightCore.Domain.Entities
{
    [Table("transacciones")]
    public class Transaction
    {
        public int id { get; set; }
        public int usuario_id { get; set; }
        public string fuente { get; set; }
        public decimal monto { get; set; } // numeric(15,2) mapea a decimal
        public string moneda { get; set; }
        public string descripcion { get; set; } // tipo text
        public string? categoria { get; set; } // Hecho nullable porque veo [null] en la imagen
        public DateOnly fecha { get; set; } // tipo date mapea mejor a DateOnly en .NET 6+
        public DateTime created_at { get; set; } // timestamp without time zone
    }
}
