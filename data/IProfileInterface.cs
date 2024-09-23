public interface IProfileInterface : IDisposable
{
    Task<List<Profile>> GetProfileAsync();
    Task<Profile> GetProfileAsync(int id);
    Task InsertProfileAsync(Profile profile);
    Task UpdateProfileAsync(Profile profile, int id);
    Task SaveAsync();
    Task DeleteProfileAsync(int id);

}