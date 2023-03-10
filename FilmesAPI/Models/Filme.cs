using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O titulo do filme é obrigatório")]
    [MaxLength(100, ErrorMessage = "O nome do filme não pode exceder 100 caracteres")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "O nome do diretor é obrigatório")]
    [MaxLength(50, ErrorMessage = "O nome do diretor não pode exceder 50 caracteres")]
    public string Diretor { get; set; }

    [Required(ErrorMessage = "O gênero do filme é obrigatório")]
    [MaxLength(50, ErrorMessage ="O tamanho do gênero não pode exceder 50 caracteres")]
    public string Genero { get; set; }

    [Required(ErrorMessage = "A duração do filme é obrigatório")]
    [Range(1, 400, ErrorMessage = "A duração deve ser entre 70 a 600 minutos")]
    public int Duracao { get; set; }
}
