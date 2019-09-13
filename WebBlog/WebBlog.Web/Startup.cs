using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebBlog.ApplicationCore.DbContexts;
using WebBlog.ApplicationCore.Entities;
using WebBlog.ApplicationCore.Repositories;

namespace WebBlog.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddTransient<BlogDbContext>();
            var connection = Configuration.GetConnectionString("BlogDbContext");
            services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("WebBlog.Web")));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<BlogDbContext>().AddDefaultUI().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.  
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.  
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.  
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings  
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Identity/Pages/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc()
                .AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Post>, PostRepository>();
            services.AddScoped<IRepository<Comment>, CommentRepository>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<BlogDbContext>();
                context.Database.EnsureCreated();
            }
            //env.EnvironmentName = EnvironmentName.Production;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "commentRoute",
                    template: "{controller}/{action}/{postId}/{id}");

            });

            CreateUserRoles(provider).Wait();
            CreateUsers(provider).Wait();
        }

        private ApplicationUser CreateAdminProfile()
        {
            var adminUser = new ApplicationUser
            {
                UserName = Configuration.GetSection("AdminSettings")["UserName"],
                FirstName = Configuration.GetSection("AdminSettings")["FirstName"],
                LastName = Configuration.GetSection("AdminSettings")["LastName"],
                Email = Configuration.GetSection("AdminSettings")["UserEmail"]
            };

            byte[] imageData = null;
            //TO DO
            FileStream file = new FileStream($"./wwwroot/images/img_avatar.png", FileMode.Open);
            var length = file.Length;
            using (var binaryReader = new BinaryReader(file))
            {
                imageData = binaryReader.ReadBytes((int)length);
            }

            adminUser.AccountImage = imageData;
            return adminUser;
        }

        private async Task CreateUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByEmailAsync(Configuration.GetSection("AdminSettings")["UserEmail"]);
            var userPassword = Configuration.GetSection("AdminSettings")["UserPassword"];
            var adminUser = CreateAdminProfile();
            if (user == null)
            {
                var createPowerUser = await userManager.CreateAsync(adminUser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Adding Addmin Role    
            var roleCheck = await roleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database    
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            roleCheck = await roleManager.RoleExistsAsync("User");
            if (!roleCheck)
            {
                //create the roles and seed them to the database    
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

        }
    }
}
