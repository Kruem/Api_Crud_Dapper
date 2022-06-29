using Dapper;
using DapperCrud.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class heroController : ControllerBase
    {
        private readonly IConfiguration _config;

        public heroController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Hero>>> GetAllSuperHeroes()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Hero> heroes = await selectAllHeroes(connection);
            return Ok(heroes);
        }

        [HttpGet("HeroId")]
        public async Task<ActionResult<Hero>> GetHero(int HeroId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var hero = await connection.QueryFirstAsync<Hero>("select * from Hero where id= @id",
                new { Id = HeroId });
            return Ok(hero);
        }

        private static async Task<IEnumerable<Hero>> selectAllHeroes(SqlConnection connection)
        {
            return await connection.QueryAsync<Hero>("select * from Hero");
        }
    }
}
