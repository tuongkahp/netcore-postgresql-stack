using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datas.Entities;

[Table("languages")]
public class Language
{
    [Key]
    [Column("language_code")]
    [StringLength(10)]
    public string LanguageCode { get; set; }

    [Required]
    [Column("name")]
    [StringLength(256)]
    public string Name { get; set; }

    [Required]
    [Column("date_format")]
    [StringLength(256)]
    public string DateFormat { get; set; }

    [Required]
    [Column("currency")]
    [StringLength(256)]
    public string Currency { get; set; }
}