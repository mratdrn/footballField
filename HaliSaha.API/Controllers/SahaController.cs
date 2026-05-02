using HaliSaha.Business;
using HaliSaha.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaliSaha.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SahaController : ControllerBase
    {
        private readonly SahaService _sahaService;

        public SahaController(SahaService sahaService)
        {
            _sahaService = sahaService;
        }

        // Herkes görebilir
        [HttpGet]
        public async Task<IActionResult> TumSahalar()
        {
            var sahalar = await _sahaService.TumSahalariGetirAsync();
            return Ok(sahalar);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SahaGetir(int id)
        {
            var saha = await _sahaService.SahaGetirAsync(id);
            if (saha is null)
                return NotFound(new { mesaj = "Saha bulunamadı." });

            return Ok(saha);
        }

        // Sadece Admin
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SahaEkle([FromBody] Saha saha)
        {
            var yeniSaha = await _sahaService.SahaEkleAsync(saha);
            return CreatedAtAction(nameof(SahaGetir), new { id = yeniSaha.Id }, yeniSaha);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SahaGuncelle(int id, [FromBody] Saha saha)
        {
            var (basarili, mesaj) = await _sahaService.SahaGuncelleAsync(id, saha);
            if (!basarili)
                return NotFound(new { mesaj });

            return Ok(new { mesaj });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SahaSil(int id)
        {
            var (basarili, mesaj) = await _sahaService.SahaSilAsync(id);
            if (!basarili)
                return NotFound(new { mesaj });

            return Ok(new { mesaj });
        }
    }
}