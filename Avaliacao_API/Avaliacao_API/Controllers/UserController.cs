using Avaliacao_API.Entity;
using Avaliacao_API.Interfaces;
using Avaliacao_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avaliacao_API.Controllers
{
    [Controller]
    [Route("/User")]
     // Exige autenticação JWT para acessar esta rota
    public class UserController : ControllerBase
    {
        IUser _userRepository;
       
        public UserController(IUser userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("/GetToken")]
        public IActionResult GetToken()
        {
            var token = TokenService.GenerateToken(new USERS());
            return Ok(token);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ShowAllUsers()
        {
            return Ok(_userRepository.ShowUsers());
        }
        /*[HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            return Ok(_userRepository.ShowUserByID(id));
        }
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            _userRepository.CreateUser(user);
            return Ok("Usuário criado com sucesso!");
        }
        [HttpPut]
        public IActionResult UpdateUser(User user)
        {
            _userRepository.EditUser(user);
            return Ok("Usuário Editado!");
        }
        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
            return Ok("Usuário deletado com sucesso");
        }*/
    }
}
