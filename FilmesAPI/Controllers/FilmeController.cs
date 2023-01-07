using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    #region :: Conexão banco de dados 
    private FilmeContext _context;
    #endregion

    #region ::AutoMapper
    private IMapper _mapper;
    #endregion

    #region :: Contrutor
    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    #endregion

    #region :: Adicionar Filmes (Post)
    /// <summary>
    /// Adicina um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objetos com os campos necessários para criação de uma filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddFilme([FromBody] CreateFilmeDto filmeDto)
    {
        //AutoMapper irá mapear a classe de filmes 
        Filme filme = _mapper.Map<Filme>(filmeDto);
        //Adiciona Filme atraves da conexão com o banco
        _context.Filmes.Add(filme);
        //Salvas as alterações no banco de dados
        _context.SaveChanges();
        //Irá mostrar o caminho que foi criado o item em questão
        return CreatedAtAction(nameof(GetFilmeForId), new { id = filme.Id }, filme);
    }
    #endregion

    #region :: Recupera a todos os filmes (Get)
    /// <summary>
    /// Recupera uma lista de filmes
    /// </summary>
    /// <param name=""></param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso seja recuperado com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IEnumerable<ReadFilmeDto> GetAllFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        //Skip, utilizado para saber quantos itens da sua lista você pular
        //Take, utilizado para saber quantos itens da sua lita você quer pegar
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }
    #endregion

    #region :: Recupera filmes por ID (GetForId)
    /// <summary>
    /// Recupera filmes por ID
    /// </summary>
    /// <param name="id"> Id necessáio para pesquisa</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso tenha um item com o Id passado no parametro</response>
    /// <response code="404">Caso seja passado um id que não exista</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetFilmeForId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null)
            return NotFound();

        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);

        return Ok(filmeDto);
    }
    #endregion

    #region :: Atualização de Filmes (Put)
    /// <summary>
    /// Atualiza o cadastro do filme
    /// </summary>
    /// <param name="id"> Id necessáio para pesquisa</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a atualização seja feita com sucesso</response>
    /// <response code="404">Caso seja passado um id que não exista</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutFilmes(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null)
            return NotFound();

        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }
    #endregion

    #region :: Atualização parcial (Patch)
    /// <summary>
    /// Atualiza parcialmente o cadastro de um filme
    /// </summary>
    /// <param name="id"> Id necessáio para pesquisa</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a atualização parcial seja feita com sucesso</response>
    /// <response code="404">Caso seja passado um id que não exista</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PatchFilmes(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null)
            return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
            return ValidationProblem(ModelState);

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    #endregion

    #region :: Deleta cadastros de filmes (Delete)
    /// <summary>
    /// Deleta filme cadastrado
    /// </summary>
    /// <param name="id"> Id necessáio para pesquisa</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso o item seja deletado com sucesso</response>
    /// <response code="404">Caso seja passado um id que não exista</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null)
            return NotFound();

        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
    #endregion
}
