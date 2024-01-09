namespace CatalogMinimalAPI.AppServicesExtensions
{
    public static class ApplicationBuilderExtension
    {
        // Handling exceptions
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if(environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            return app;
        }

        // Enable CORS
        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors(p => p.AllowAnyOrigin().WithMethods("GET").AllowAnyHeader());
            return app;
        }

        // Enable Swagger
        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(p => { });
            return app;
        }
    }
}
