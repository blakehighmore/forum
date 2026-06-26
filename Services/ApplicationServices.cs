using backend.Data;
using backend.Services.Auth;
using backend.Services.Auth.Jwt;
using backend.Services.Categories;
using backend.Services.PostReactions;
using backend.Services.Posts;
using backend.Services.Profiles;
using backend.Services.Tags;
using backend.Services.Topics;


namespace backend.Services;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IPostReactionService, PostReactionService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();

        return services;
    }
}