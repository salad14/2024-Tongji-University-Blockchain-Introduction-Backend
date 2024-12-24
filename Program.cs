using Microsoft.EntityFrameworkCore;
using StudentCompetitionAPI.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 添加服务到容器
builder.Services.AddControllers();

// 配置Oracle数据库上下文
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// 配置Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Competition API", Version = "v1" });
});

var app = builder.Build();

// 配置中间件
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// 启用Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Competition API V1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
