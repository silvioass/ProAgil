using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestranteController : ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public PalestranteController(IProAgilRepository repo)
        {
            this._repo = repo;
        }

        /*// GET api/Palestrantes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repo.get(true);

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }            
        }*/

        // GET api/Palestrantes/{PalestranteId}
        [HttpGet("{PalestranteId}")]
        public async Task<IActionResult> Get(int PalestranteId)
        {
            try
            {
                var results = await _repo.GetPalestranteByIdAsync(PalestranteId, true);

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }            
        }

        // GET api/Palestrantes/getByNome/{nome}
        [HttpGet("getByNome/{nome}")]        
        public async Task<IActionResult> Get(string nome)
        {
            try
            {
                var results = await _repo.GetAllPalestranteByNomeAsync(nome, true);

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }            
        }

        // POST api/Palestrantes
        [HttpPost]
        public async Task<IActionResult> Post(Palestrante model)
        {
            try
            {
                _repo.Add(model);

                if(await _repo.SaveChangesAsync()){
                    return Created($"/api/Palestrante/{model.Id}", model);
                }                
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }  

            return BadRequest();
        }

        // POST api/Palestrantes
        [HttpPut("{PalestranteId}")]
        public async Task<IActionResult> Put(int PalestranteId, Palestrante model)
        {
            try
            {
                var Palestrante = await _repo.GetPalestranteByIdAsync(PalestranteId, false);
                if (Palestrante==null) return NotFound();

                _repo.Update(model);

                if(await _repo.SaveChangesAsync()){
                    return Created($"/api/Palestrante/{model.Id}", model);
                }                
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }  

            return BadRequest();
        }

        // DELETE api/Palestrantes
        [HttpDelete("{PalestranteId}")]
        public async Task<IActionResult> Delete(int PalestranteId)
        {
            try
            {
                var Palestrante = await _repo.GetPalestranteByIdAsync(PalestranteId, false);
                if (Palestrante==null) return NotFound();
                
                _repo.Delete(Palestrante);

                if(await _repo.SaveChangesAsync()){
                    return Ok();
                }                
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou: " + ex.Message);
            }  

            return BadRequest();
        }
    }
}