var builder = WebApplication.CreateBuilder(args);

// Cargar la configuraci�n del archivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

var app = builder.Build();

// Configurar el servicio de configuraci�n
app.Services.AddSingleton(builder.Configuration);

// Agregar servicios necesarios
app.Services.AddControllers();
app.Services.AddEndpointsApiExplorer();
app.Services.AddSwaggerGen();

// Obtener la cadena de conexi�n de la configuraci�n
var connectionString = builder.Configuration.GetConnectionString("umbracoDbDSN");

// Configurar DbContext con la cadena de conexi�n
app.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure middleware...

app.Run();
