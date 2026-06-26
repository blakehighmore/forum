using backend.Data;
using backend.DTOs.Profiles;
using backend.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace backend.Services.Profiles;

public class ProfileService : IProfileService
{
    private readonly AppDbContext _db;

    public ProfileService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ProfileReadDto> GetAsync(int currentUserId)
    {
        var profile = await _db.Profiles
            .Where(p => p.UserId == currentUserId)
            .Select(p =>
                new ProfileReadDto(p.FirstName, p.LastName, p.AboutMe, p.Profession, p.Birthday, p.IsEmployed))
            .FirstOrDefaultAsync();

        if (profile is null) throw new NotFoundException("Профиль не был найден");

        return profile;
    }

    public async Task<ProfileReadDto> UpdateAsync(int currentUserId, ProfileUpdateDto dto)
    {
        var profile = await _db.Profiles.Where(p => p.UserId == currentUserId).FirstOrDefaultAsync();

        if (profile is null) throw new NotFoundException("Профиль не был найден");

        profile.FirstName = dto.FirstName;
        profile.LastName = dto.LastName;
        profile.AboutMe = dto.AboutMe;
        profile.Profession = dto.Profession;
        profile.Birthday = dto.Birthday;
        profile.IsEmployed = dto.IsEmployed;
        await _db.SaveChangesAsync();

        return new ProfileReadDto(profile.FirstName, profile.LastName, profile.AboutMe, profile.Profession,
            profile.Birthday, profile.IsEmployed);
    }
}