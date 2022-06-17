using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datas.Entities;

[Table("translations")]
public class Translation
{
    [Key]
    [Column("translation_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long TranslationId { get; set; }

    [Required]
    [Column("translation_code")]
    [StringLength(256)]
    public string TranslationCode { get; set; }

    [Column("name_space")]
    [StringLength(256)]
    public string NameSpace { get; set; }

    [Required]
    [Column("language_code")]
    [StringLength(10)]
    public string LanguageCode { get; set; }

    [Required]
    [Column("translation_description")]
    public string TranslationDescription { get; set; }
}