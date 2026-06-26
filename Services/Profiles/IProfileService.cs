using backend.DTOs.Profiles;


namespace backend.Services.Profiles;

public interface IProfileService
{
    Task<ProfileReadDto> GetAsync(int currentUserId);
    Task<ProfileReadDto> UpdateAsync(int currentUserId, ProfileUpdateDto dto);
}