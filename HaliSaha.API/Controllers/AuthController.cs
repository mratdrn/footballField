using HaliSaha.Business;
using Microsoft.AspNetCore.Mvc;

namespace HaliSaha.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("kayit")]
        public async Task<IActionResult> KayitOl([FromBody] KayitDto dto)
        {
            var (basarili, mesaj) = await _authService.KayitOlAsync(dto.Ad, dto.Email, dto.Sifre);
            if (!basarili)
                return BadRequest(new { mesaj });

            return Ok(new { mesaj });
        }

        [HttpPost("giris")]
        public async Task<IActionResult> GirisYap([FromBody] GirisDto dto)
        {
            var (basarili, mesaj, token) = await _authService.GirisYapAsync(dto.Email, dto.Sifre);
            if (!basarili)
                return Unauthorized(new { mesaj });

            return Ok(new { mesaj, token });
        }
    }

    // DTO'ları Controller dosyasının altına yazdık — junior için daha basit
    public record KayitDto(string Ad, string Email, string Sifre);
    public record GirisDto(string Email, string Sifre);
}