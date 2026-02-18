using Microsoft.AspNetCore.Authorization;

namespace PetVerse.Policies.Requirements;

public enum ProfileType
{
    User,
    Business,
    Shelter
}

public class ProfileTypeRequirement : IAuthorizationRequirement
{
    public ProfileTypeRequirement(ProfileType profileType) =>
        ProfileType = profileType;

    public ProfileType ProfileType { get; }
}