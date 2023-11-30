using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RainfallApi.Models
{
    public class ReadingFilter
    {
        [Range(1,100)]
        [DefaultValue(10)]
        public int count { get; set; }
    }
}
