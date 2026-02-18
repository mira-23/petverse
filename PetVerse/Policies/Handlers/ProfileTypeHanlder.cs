using System.Security.Claims;
using PetVerse.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
using PetVerse.Services;
using PetVerse.Data;
using Microsoft.EntityFrameworkCore;
using PetVerse.DTOs;
using PetVerse.Entities;

namespace PetVerse.Policies.Handlers;

public class ProfileTypeHanlder : AuthorizationHandler<ProfileTypeRequirement>
{
    private readonly ProfileService _profileService;

    private readonly AppDbContext _context;

    public ProfileTypeHanlder(
        ProfileService profileService,
        AppDbContext context)
    {
        _profileService = profileService;
        _context = context;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, ProfileTypeRequirement requirement)
    {
        string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return;
        }

        if (requirement.ProfileType==ProfileType.User && context.Resource is CreateLostAnimalPostDTO)
        {
            context.Succeed(requirement);
            return;
        }
        

        return;
    }
}