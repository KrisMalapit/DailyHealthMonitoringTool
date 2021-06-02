using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScreeningTool.Models
{
    public class QREntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ScreenLogId { get; set; }
        public string QRKey { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
