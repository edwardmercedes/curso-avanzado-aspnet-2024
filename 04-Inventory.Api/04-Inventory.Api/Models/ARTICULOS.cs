using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _04_Inventory.Api.Models
{
    public class ARTICULOS
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CODIGO { get; set; }
        public string? DESCRIPCION { get; set; }
        public int CATEGORIA { get; set; }
        public string? MARCA { get; set; }
        public double? PESO { get; set; }
        public string? CODIGO_BARRAS { get; set; }
        public DateTime? CREACION_TSTAMP { get; set; }
        public string ?CREACION_USUARIO { get; set; }
        public DateTime? ULT_MODIF_TSTAMP { get; set; }
        public string? ULT_MODIF_USUARIO { get; set; }
    }
}
