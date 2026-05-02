using HaliSaha.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HaliSaha.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Tüm endpoint'ler giriş gerektirir
    public class RezervasyonController : ControllerBase
    {
        private readonly RezervasyonService _rezervasyonService;

        public RezervasyonController(RezervasyonService rezervasyonService)
        {
            _rezervasyonService = rezervasyonService;
        }

        [HttpGet("benim")]
        public async Task<IActionResult> BenimRezervasyonlarim()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var liste = await _rezervasyonService.KullanicininRezervasyonlari(userId);
            return Ok(liste);
        }

        [HttpGet("tumü")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TumRezervasyonlar()
        {
            var liste = await _rezervasyonService.TumRezervasyonlar();
            return Ok(liste);
        }

        [HttpPut("{id}/iptal")]
        public async Task<IActionResult> IptalEt(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var rol = User.FindFirstValue(ClaimTypes.Role)!;

            var (basarili, mesaj) = await _rezervasyonService.RezervasyonIptalEt(id, userId, rol);
            if (!basarili)
                return BadRequest(new { mesaj });

            return Ok(new { mesaj });
        }



        [HttpPost]
        public async Task<IActionResult> RezervasyonYap([FromBody] RezervasyonDto dto)
        {
            // JWT token'dan giriş yapan kullanıcının Id'sini al
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr is null)
                return Unauthorized();

            var userId = int.Parse(userIdStr);

            var (basarili, mesaj, rezervasyon) = await _rezervasyonService
                .RezervasyonYapAsync(dto.SahaId, userId, dto.BaslangicZamani, dto.BitisZamani);

            if (!basarili)
                return BadRequest(new { mesaj });

            return Ok(new { mesaj, rezervasyon });
        }
    }

    public record RezervasyonDto(int SahaId, DateTime BaslangicZamani, DateTime BitisZamani);
}