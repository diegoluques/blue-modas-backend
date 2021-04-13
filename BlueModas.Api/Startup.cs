using BlueModas.Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace BlueModas.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors();
			services.AddControllers();

			//configuração de serialização
			services.AddControllersWithViews()
					.AddNewtonsoftJson(options => {
						options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
						options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
						options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					});

			// Configuração do container de injeção de dependências
			services.AddScoped<CestaCompraRepository>();
			services.AddScoped<ProdutoRepository>();
			services.AddScoped<ClienteRepository>();
			services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer("Data Source=DEV-LUQUES\\SQL2019;Initial Catalog=BD_BlueModas;Integrated Security=SSPI;"));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseExceptionHandler(builder =>
			{
				builder.Run(async ctx =>
				{
					var errorApp = ctx.Features.Get<IExceptionHandlerFeature>();
					var ex = errorApp.Error;

					ctx.Response.StatusCode = 200;
					ctx.Response.ContentType = "application/json";
					 
					var message = ex.Message;
					var messageType = "warning";

					var strJson = $@"{{ ""sucess"": false, ""message"": ""{message}"", ""message_type"": ""{messageType}"" }}";
					await ctx.Response.WriteAsync(strJson);
				});
			});

			app.UseRouting(); 

			//configuração de pastas staticas
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider( Path.Combine(env.ContentRootPath, "Assets")),
				RequestPath = "/Assets"
			});

			//configurações de cors
			app.UseCors(x => x
			   .AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader());

			app.UseAuthentication();
			app.UseAuthorization(); 

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}