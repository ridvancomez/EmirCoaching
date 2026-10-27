using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Common;

// Tüm admin controller'larının tabanı (Controllers/Admin/).
// [Authorize] burada: türeyen controller'da unutulamaz (varsayılan politika = giriş yapmış Admin).
// [Area] site controller'larıyla aynı adı taşıyan admin controller'larını ayırır (ör. CreatedAtAction link üretimi).
[Authorize]
[Area("Admin")]
public abstract class AdminControllerBase : ApiControllerBase;
