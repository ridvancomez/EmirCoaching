namespace EmirCoaching.Business.Common;

// Onay kayıtları için isteğin IP ve tarayıcı bilgisi (controller'da HttpContext'ten doldurulur)
public record RequestInfo(string IpAddress, string UserAgent);
