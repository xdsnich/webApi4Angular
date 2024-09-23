public class ProfileCRUD : IProfileInterface
{
    private readonly ProfileDb _context;
    public ProfileCRUD(ProfileDb context){
        _context = context;
    }

     public Task<List<Profile>> GetProfileAsync() => _context.Profiles.ToListAsync();
    

    public async Task<Profile> GetProfileAsync(int id)
    {
        var profileFromDb = await _context.Profiles
        .FindAsync(new object[]{id});
        if(profileFromDb == null){
            return profileFromDb!;
        }
        return profileFromDb;
    }
        
    public async Task InsertProfileAsync(Profile profile)
    {
        var profileFromDb = await _context.Profiles.AddAsync(profile);
        await SaveAsync();
    }

    public async Task UpdateProfileAsync(Profile profile, int id)
    {
        var profileFromDb = await _context.Profiles
        .FindAsync(new object[]{id});
        if(profileFromDb == null){
            return;}
        profileFromDb.Username = profile.Username;
        profileFromDb.Email = profile.Email;       
        profileFromDb.Password = profile.Password;
        profileFromDb.Description = profile.Description;
        profileFromDb.AvatarUrl = profile.AvatarUrl;
        profileFromDb.FirstName = profile.FirstName;
        profileFromDb.LastName = profile.LastName;
        profileFromDb.City = profile.City;
        
        await SaveAsync();
    }
    
    public async Task DeleteProfileAsync(int id)
    {
        var profileFromDb = await _context.Profiles.FindAsync(new object[]{id}); 
        if (profileFromDb == null ){
            return;
        }
        _context.Profiles.Remove(profileFromDb);
    }

    public async Task SaveAsync(){
        await _context.SaveChangesAsync();
    }
 
    private bool _disposed = false;

    protected virtual void Dispose(bool disposing){
        if(!_disposed){
            if(disposing){
                _context.Dispose();
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

   

    
}