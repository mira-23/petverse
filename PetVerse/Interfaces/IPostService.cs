using PetVerse.DTOs;

namespace PetVerse.Interfaces
{
    public interface IPostService
    {
        Task CreatePostAsync(CreatePostDto dto);
    }
}

