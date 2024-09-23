using System.ComponentModel.DataAnnotations;

public class Profile 
{
    public int id {get; set;}
    public string? Username{get;set;} 
    public string? Email{get; set;}
    public string? Password {get; set;}

    public string Description{get; set;} = string.Empty;
    public string AvatarUrl{get; set;} = string.Empty;
    public string FirstName{get; set;} = string.Empty;
    public string LastName{get; set;} = string.Empty;
    
    public string City{get; set;} = string.Empty;
   
}