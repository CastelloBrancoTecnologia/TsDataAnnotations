using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using TsDataAnnotations.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;

using CommunityToolkit.Mvvm.ComponentModel;

using LinqToDB;
using Mainwave.MimeTypes;
using System.Reflection;
using TsDataAnnotations.Server.Services;
using System.Security.Claims;
using System.Net;

namespace TsDataAnnotations.Server.Controllers;

[ApiController]
[Route("api/[action]")]
public class TsDataAnnotationsController : ControllerBase
{
    private readonly ILogger<TsDataAnnotationsController> _logger;
    private readonly IConfiguration _configuration;
    private readonly TsDataAnnotationsDb _db;

    public TsDataAnnotationsController(TsDataAnnotationsDb connection, ILogger<TsDataAnnotationsController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration; // ex configuration["Logging:LogLevel:Default"];
        _db = connection;

        try
        {
            if (!_db.Usuarios.Any(u => u.Email.ToLower() == "cesar@castellobranco.tec.br"))
            {
                Usuario u = new()
                {
                    Nome = "Cesar Castello Branco",
                    Email = "cesar@castellobranco.tec.br",
                    Celular = "+55(21)971379399",
                    HashSenha = Usuario.Hash("1@cbt!")
                };

                u.Id = _db.InsertWithInt64Identity(u);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no construtor do controller");
        }
    }

    [HttpGet()]
    [Authorize]
    public ActionResult<string> GetUsernameAndPlocicies()
    {
        return Ok ("teste");
    }

    [HttpGet]
    [Route("authenticated")]
    [Authorize]
    public string Authenticated() => $"Autenticado - {this.User?.Identity?.Name}";

    [HttpGet]
    [Route("roles")]
    // [Authorize(Roles = "admin")]
    public string roles()
    {
        if (this.User != null)
        {
            string [] roles = this.User.Claims.Where (claim => claim.Type == ClaimTypes.Role).Select (claim => claim.Value).ToArray();

            return string.Join(",", roles);
        }

        return "";
    }

    [HttpPost()]
    [AllowAnonymous]
    public ActionResult<TokenModel> Login([FromBody] LoginModel l)
    {
        try
        {
            if (l.IsValid())
            {
                return BadRequest (new TokenModel()
                {
                    Username = l.Username,
                    Token = "",
                    Expires = DateTimeOffset.MinValue,
                    Message = l.ErrorMessages()
                });
            }

            Usuario? u = _db.Usuarios.SingleOrDefault(x => x.Username.Trim().ToLower() == l.Username.Trim().ToLower());

            if (u == null)
            {
                return NotFound(new TokenModel()
                {
                    Username = l.Username,
                    Message = "Usuario nao encontrado ou senha incoreeta"
                });
            }
            else
            {
                if (Usuario.VerificaSenha(u, l.Senha))
                {
                    UsuariosRole[] roles = _db.UsuariosRoles.Where(x => x.IdUsuario == u.Id).ToArray();

                    TokenModel tokenModel = TokenService.GenerateToken(u, roles ); 

                    tokenModel.Message = "Logado com sucesso";

                    this.Response.Headers["Authorization"] = $"Bearer {tokenModel.Token}";

                    return Ok (tokenModel);
                }
                else
                {
                    return NotFound(new TokenModel()
                    {
                        Username = l.Username,
                        Message = "Usuario nao encontrado ou senha incoreeta"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no webmethodo Login do controller");

            return BadRequest (new TokenModel()
            {
                Username = l.Username,
                Message = "Erro interno ao logar"
            });
        }
    }
}
