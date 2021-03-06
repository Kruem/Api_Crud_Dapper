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
        [HttpPost]
        public async Task<ActionResult<List<Hero>>> CreateHero(Hero hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into Hero(NameHero,FirstDate,LastName,place)values(@NameHero,@FirstDate,@LastName,@Place)",hero);
            return Ok(await selectAllHeroes(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<Hero>>> UpdateHero(Hero hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update Hero set NameHero=@NameHero,FirstDate=@FirstDate,LastName=@LastName,place=@Place where Id=@Id", hero);
            return Ok(await selectAllHeroes(connection));
        }

        [HttpDelete("HeroId")]
        public async Task<ActionResult<List<Hero>>> DeleteHero(int HeroId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var hero = await connection.QueryFirstAsync<Hero>("delete Hero where id= @id",
                new { Id = HeroId });
            return Ok(await selectAllHeroes(connection));
        }

        private static async Task<IEnumerable<Hero>> selectAllHeroes(SqlConnection connection)
        {
            return await connection.QueryAsync<Hero>("select * from Hero");
        }
    }
}
