using Serilog;

var builder = WebApplication.CreateBuilder(args);

//1. A09:2025 - Security Logging and Alerting Failures
// Loglama servislerinin eklenmesi.
//builder.Services.AddLogging();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

//2. A07:2025 - Authentication Failures & A01:2025 - Broken Access Control
// Kimlik doğrulama ve yetkilendirme servislerinin eklenmesi.
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

//3. A02:2025 - Security Misconfiguration
// Güvenlik yapılandırması için gerekli servislerin eklenmesi.
// Hsts (HTTP Strict Trasport Security) yapılandırılması, yaygın ortamında güvenli yönlendirmeyi zorlar.
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

var app = builder.Build();
// 4- A10:2025 - Mishandling of Exceptional conditions
// Global hata/istisna işleyicisi eklenmesi. Gerçek hata mesajlarının (örneğin, stack trace) kullanıcıya gösterilmemesi sağlanır.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
        });
    });

    //app.UseExceptionHandler("/error"); // Hata durumunda yönlendirme yapacak bir endpoint tanımlanabilir.

    //A02 & A04 aktivasyonu
    app.UseHsts();  //HSTS yi etkinleştirir.
}
// 5. A04:2025 - Cryptographic Failure
// Tüm HTTP isteklerinin HTTPS üzerinden yapılmasını zorunlu kılmak için yönlendirme eklenmesi.
app.UseHttpsRedirection();

// 6. A02:2025 - Security Misconfiguration & A05:2025 - Injection
app.Use(async (context, next) =>
{
    //CSP (Context Security Policy) Başlıklarının eklenmesi. Bu, XSS saldırılarını önlenmeye yardımcı olur.
    //XSS: Saldırganın kötü amaçlı kodu (örneğin, JavaScript) kullanıcı tarayıcısında çalıştırılmasını engeller.
    // default-src 'self' : Her içerik sadece bulunduğu kaynaktan (aynı origin) yüklenebilir. harici kaynaklardan içerik yüklenmesine izin verilmez.
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self!");

    // X-Frame-Options başlığının eklenmesi. Bu, clickjacking saldırılarını önlemeye yardımcı olur.
    context.Response.Headers.Append("X-Frame-Options", "DENY"); // Sayfanın hiçbir şekilde iframe içinde yüklenmesine izin verilmez.

    // X-Content-Type-Options başlığının eklenmesi. Bu,MIME (Multipurpose Internet Mail Extensions, Örn: *.css, *.js) türü karıştırma saldırılarını önlemeye yardımcı olur. Kısacası dosyanın içindeki kodları okumayı (sniffing) engeller. Bu sayede uzantı ile içerik uyuşmazsa bile, tarayıcı dosyayı uzantasına göre değil, belirtilen türde (örneğin, text/css) işleyecektir.


    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

    // Referrer-Policy başlığının eklenmesi. Bu, referer bilgisinin (örneğin,hangi sayfadan gelindiğini) paylaşımını kontrol eder. bu sayede hassas bilgilerin (örneğin, URL parametlereli) üçüncü taraflara sızmasını engeller.

    // strict-origin içindeki isteklerde ise sadece origin bilgisini gönderir. Böylece hassas bilgilerin (örneğin, URL parametreleri) üçüncü taraflara sızmasını engeller.
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

    await next(); // Sonraki middleware'e geçilir.
});

// Yönlendirme
app.UseRouting();

// Kimlik doğrulama ve yetkilendirme middleware'lerin aktifleştirilmesi. Bu, A01 ve A07 için gereklidir. ( Sıralama zorunludur, özellikle Routing sonrası gelmeldiir)
app.UseAuthentication();
app.UseAuthorization();

// Haritalar


app.MapGet("/", () => "Hello World!");

app.Run();
