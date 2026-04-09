using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utilities.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public override string ToString()
        {
            //Sınıfın kendini JSON formatında döndürür. Bu, hata detaylarının daha okunabilir ve yapılandırılmış bir şekilde sunulmasını sağlar.
            return JsonSerializer.Serialize(this);
        }
    }
}
