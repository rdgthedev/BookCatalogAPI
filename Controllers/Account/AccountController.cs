using BookCatalog.Extension;
using BookCatalog.Interfaces;
using BookCatalog.ViewModels;
using BookCatalog.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using SecureIdentity.Password;
namespace BookCatalog.Controllers.Account
{
    [ApiController]
    [Produces("application/json")]
    public class AccountController(IUserRepository userRepository, ITokenService tokenService) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Efetua o login.
        /// </summary>
        /// <param name="model.Email">Email</param>
        /// <param name="model.Password"></param>
        /// <response code="200">Sucesso.</response>
        /// <response code="403">Usuário ou senha inválidos!</response>
        [HttpPost("v1/accounts/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = await _userRepository.GetByEmailAsync(model.Email);

            if (user == null)
                return StatusCode(403, new ResultViewModel<string>("08LNF01 - Email ou Senha inválidos"));

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(403, new ResultViewModel<string>("08LNF01 - Email ou Senha inválidos"));

            try
            {
                var jwtToken = _tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<dynamic>(new
                {
                    User = user.Name,
                    Token = jwtToken
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("03IE01 - Falha interna!"));
            }
        }

    }
}
