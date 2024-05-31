



using Autofac;
using Core.Common.Configuration;
using ElmahCore;
using ElmahCore.Mvc;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hub.Business;
using Hub.Common.Settings;
using Hub.Web.Api.Filters;
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;
using Newtonsoft.Json;
using System.Reflection;
using ConfigurationManager = Core.Common.Configuration.ConfigurationManager;

namespace Hub.Web.Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            InitSettings();
            var assembliesToScan = new[] {
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(Core.Business.IDependency)),
                Assembly.GetAssembly(typeof(Core.Data.IDependency)),
                  Assembly.GetAssembly(typeof(YoMentor.ChatGPT.IDependency)),
            };
            // register services only
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => c.Name.EndsWith("Service") || c.Name.EndsWith("Repository")).AsPublicImplementedInterfaces();
            //  services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan).AsPublicImplementedInterfaces();

            services.AddHttpContextAccessor();

            var ioc = new IoC(() => {
                var builder = new ContainerBuilder();
                builder.RegisterAssemblyTypes(assembliesToScan)
                    .Where(c => c.Name.EndsWith("Service") ||
                                c.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces();
                return builder.Build();
            });

            var pathToFirebaseAdminSdkJson = "firebase/AdminSdk.json";
            FirebaseApp.Create(new AppOptions {
                Credential = GoogleCredential.FromFile(pathToFirebaseAdminSdkJson)
            });
            //var ioc = new IoC(() => {
            //    var builder = new ContainerBuilder();
            //    builder.RegisterAssemblyTypes(assembliesToScan)
            //        .AsImplementedInterfaces();
            //    return builder.Build();
            //});

            services.AddSwaggerGen(c => {
                c.CustomSchemaIds(type => type.ToString());
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Yomentor Core API V1", Version = "v1" });

                // Authorization header
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = @"Authorization header using the Bearer scheme. <br/>
                      Enter 'Bearer' [space] and then your token in the text input below.
                      <br/> Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityDefinition("token", new OpenApiSecurityScheme {
                    Description = @"JWT user encrypted token header",
                    Name = "token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                  new OpenApiSecurityScheme
                  {
                    Reference = new OpenApiReference
                      {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                      },
                      Scheme = "oauth2",
                      Name = "Bearer",
                      In = ParameterLocation.Header,

                    },
                    new List<string>()
                  }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                  new OpenApiSecurityScheme
                  {
                    Reference = new OpenApiReference
                      {
                        Type = ReferenceType.SecurityScheme,
                        Id = "token"
                      },
                      Scheme = "oauth2",
                      Name = "token",
                      In = ParameterLocation.Header,

                    },
                    new List<string>()
                  }
                });

                //// Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            services.AddCors(options => {

                options.AddPolicy("AllowedOrigins",
                        builder => {
                            builder.AllowAnyMethod().AllowAnyHeader();
                            //if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AllowedOrigins"]) && ConfigurationManager.AppSettings["AllowedOrigins"] != "*")
                            if (!string.IsNullOrWhiteSpace(GlobalSettings.AllowedOrigins) && GlobalSettings.AllowedOrigins != "*") {
                                builder.WithOrigins(GlobalSettings.AllowedOrigins.Split(','));
                            }
                            else {
                                builder.AllowAnyOrigin();
                            }
                        });
            });

            services.AddMvc().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddControllers();
            //services.AddControllers(config => {
            //    config.Filters.Add(new LoggingFilter());
            //});




            //services.AddElmah();
            services.AddElmah<XmlFileErrorLog>(options => {
                options.LogPath = "~/ElmahLog"; // OR options.LogPath = "с:\errors";
                options.FiltersConfig = "elmah.xml";
            });
            //services.AddCors(options => {
            //    options.AddPolicy("AllowedOrigins",
            //            builder => {
            //                builder.AllowAnyMethod().AllowAnyHeader();
            //                if (Configuration["AllowedOrigins"] != "*") {
            //                    builder.WithOrigins(Configuration["AllowedOrigins"].Split(','));
            //                }
            //                else {
            //                    builder.AllowAnyOrigin();
            //                }
            //            });
            //});

          
        }





        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            app.UseCors("AllowedOrigins");

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CCS API V1");
            });
            app.UseElmah();
            IoC.ServiceProvider = app.ApplicationServices;

            //var backgroundJobScheduler = new BackgroundJobScheduler();
            //backgroundJobScheduler.InitializeBackgroundJobs();

            app.UseHttpsRedirection();

        }

        private void InitSettings() {
            const string CONNECTIONS_SECTION = "ConnectionStrings";
            const string APPSETTINGS_SECTION = "AppSettings";
            //Connections
            if (Configuration.GetSection(CONNECTIONS_SECTION).Exists()) {
                foreach (var item in Configuration.GetSection(CONNECTIONS_SECTION).AsEnumerable()) {
                    var key = item.Key.Replace(CONNECTIONS_SECTION, "");
                    if (!string.IsNullOrWhiteSpace(key)) {
                        ConfigurationManager.ConnectionStrings.Add(key.TrimStart(':'), new ConfigConnection { ConnectionString = item.Value });
                    }
                }
            }

            //AppSettings
            if (Configuration.GetSection(APPSETTINGS_SECTION).Exists()) {
                foreach (var item in Configuration.GetSection(APPSETTINGS_SECTION).AsEnumerable()) {
                    var key = item.Key.Replace(APPSETTINGS_SECTION, "");
                    if (!string.IsNullOrWhiteSpace(key)) {
                        ConfigurationManager.AppSettings.Add(key.TrimStart(':'), item.Value);
                    }
                }
            }
        }
    }


}

