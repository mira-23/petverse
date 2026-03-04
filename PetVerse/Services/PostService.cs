using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PetVerse.Classes;
using PetVerse.Data;
using PetVerse.DTOs;
using PetVerse.Models;
using PetVerse.Queries;

namespace PetVerse.Services
{
    public class PostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        private static List<string> ValidateSimpleData(CreateSimplePostDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(dto.Title) || string.IsNullOrWhiteSpace(dto.Title))
                errors.Add("Title is required");

            if (dto.Title.Length > 128 || dto.Title.Length < 5)
            {
                errors.Add("Title must be between 5 and 128 characters");
            }

            if (string.IsNullOrEmpty(dto.Body) || string.IsNullOrWhiteSpace(dto.Body))
                errors.Add("Post content (body) is required");
            
            return errors;
        }

        private static void ValidateData(CreateTypedPostDTO dto)
        {
            List<string> animalTypes = ["cat", "dog", "other"];
            var errors = ValidateSimpleData(dto);

            if (dto.Photo == null
            || dto.Photo.Name == null
            || dto.Photo.FileName == null
            || dto.Photo.Length == 0)
                errors.Add("Photo is required");

            if (string.IsNullOrEmpty(dto.Type) || string.IsNullOrWhiteSpace(dto.Type))
                errors.Add("Animal type is required");

            if (!animalTypes.Contains(dto.Type))
                errors.Add("Animal type must be cat|dog|other");

            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));
        }

        private static async Task<string> SaveSinglePhotoAsync(Post post, IFormFile photo, string photoType, int index)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Images", $"{photoType}s");
            Directory.CreateDirectory(path);

            string extension = Path.GetExtension(photo.FileName);
            string name = Path.GetFileNameWithoutExtension(photo.FileName);
            string fileName = $"{name}_{photoType}_{index}_{post.Id}{extension}";
            string filePath = Path.Combine(path, fileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await photo.CopyToAsync(fileStream);
            }

            return fileName;
        }

        private static async Task SavePhotoPathToPost(PhotoPost post, CreateTypedPostDTO dto, string photoType)
        {
            post.PhotoPath = await SaveSinglePhotoAsync(post,dto.Photo,photoType,0);
        }

        private async Task SaveMultiplePhotosToBusinessPost(BusinessPost post, CreateBusinessPostDTO dto, string photoType)
        {
            List<PostMedia> medias = [];
            for(int i = 0; i<dto.Media.Count;i++)
            {
                PostMedia postMedia = new PostMedia
                {
                    Path = await SaveSinglePhotoAsync(post,dto.Media[i],photoType,i),
                    BusinessPostId = post.Id
                };
                medias.Add(postMedia);
                post.PostMedias = medias;
                _context.PostMedias.Add(postMedia);
            }
        }

        private async Task SaveToDbAsync(Post post, CreateSimplePostDTO dto, string photoType)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Add(post);

                await _context.SaveChangesAsync();

                if (post is PhotoPost && dto is CreateTypedPostDTO)
                {
                    await SavePhotoPathToPost((PhotoPost)post, (CreateTypedPostDTO)dto, photoType);
                }
                else if(post is BusinessPost)
                {
                    await SaveMultiplePhotosToBusinessPost((BusinessPost)post,(CreateBusinessPostDTO)dto,"Business");
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Database error while creating post");
            }
            catch (IOException)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Error saving photo file to disk");
            }
            catch (ArgumentException)
            {
                await transaction.RollbackAsync();
                throw new ValidationException("Invalid file path (name possibly has invalid characters)");
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException($"An error occurred while creating the post: {e.Message}");
            }
        }

        public async Task<LostAnimalPost> CreateLostAnimalPostAsync(string userId, CreateLostAnimalPostDTO dto)
        {
            ValidateData(dto);

            var post = new LostAnimalPost
            {
                PhotoPath = "",
                Title = dto.Title,
                Body = dto.Body,
                Type = dto.Type,
                UserId = userId,
                Status = "notFound",
                Published = DateTime.Now
            };

            string photoType = "LostAnimal";

            await SaveToDbAsync(post, dto, photoType);
            return post;
        }

        internal async Task<LostAnimalPost?> GetLostAnimalPostByIdAsync(int id)
        {
            return await _context.LostAnimalPosts.FindAsync(id);
        }

        public async Task<AnimalAdoptionPost> CreateAnimalAdoptionPostAsync(string userId, CreateAnimalAdoptionPostDTO dto)
        {
            ValidateData(dto);

            var post = new AnimalAdoptionPost
            {
                PhotoPath = "",
                Title = dto.Title,
                Body = dto.Body,
                Type = dto.Type,
                ShelterProfileId = dto.ShelterId,
                UserId = userId,
                Published = DateTime.Now,
                AdoptedAt = null,
                Status = "available"
            };

            string photoType = "AnimalAdoption";

            await SaveToDbAsync(post, dto, photoType);
            return post;
        }

        internal async Task<AnimalAdoptionPost?> GetAnimalAdoptionPostByIdAsync(int id)
        {
            return await _context.AnimalAdoptionPosts.FindAsync(id);
        }

        public async Task<BusinessPost> CreateBusinessPostAsync(string userId, CreateBusinessPostDTO dto)
        {
            List<string> errors = [];
            errors = ValidateSimpleData(dto);
            if (dto.Media == null || !dto.Media.Any())
                errors.Add("Media is required");
            if (dto.Media == null || !dto.Media.Any(m => m.Length > 0))
                errors.Add("At least one non-empty file is required");
            if (errors.Any())
                throw new ValidationException(string.Join(", ", errors));

            var post = new BusinessPost
            {
                Title = dto.Title,
                Body = dto.Body,
                BusinessProfileId = dto.BusinessId,
                UserId = userId,
                Published = DateTime.Now,
                PostMedias = []
            };

            string photoType = "Business";
            await SaveToDbAsync(post, dto, photoType);
            return post;
        }

        internal async Task<BusinessPost?> GetBusinessPostByIdAsync(int id)
        {
            var post = await _context.BusinessPosts.FindAsync(id);
            if (post != null)
                post.PostMedias = [.. _context.PostMedias.Where(x=>x.BusinessPostId==post.Id)];
            return post;
        }

        private bool DoesUserOwnProfile(Post post,int profileId, string? userId)
        {
            if (userId == null)
                return false;

            if(post is AnimalAdoptionPost)
            {
                return _context.UserToShelterProfileMapping
                .Any(us => us.UserId == userId && us.ShelterProfileId == profileId);
            }
            else if(post is BusinessPost)
            {
                bool a = _context.UserToBusinessProfileMapping
                .Any(us => us.UserId == userId && us.BusinessProfileId == profileId);
                return a;
            }
            else
            {
                return false;
            }
        }

        public IQueryable<DashboardPostRepsonseDTO> FindAllPosts(string? userId, string path)
        {
            var businessPosts = _context.BusinessPosts.ToList();
            var shelterPosts = _context.AnimalAdoptionPosts.ToList();
            var lostAnimalPosts = _context.LostAnimalPosts.ToList();

            var dashboardPostDtos = new List<DashboardPostRepsonseDTO>();

            dashboardPostDtos.AddRange(businessPosts.Select(bp => new DashboardPostRepsonseDTO
                {
                    Title = bp.Title,
                    Body = bp.Body,
                    UserId = DoesUserOwnProfile(bp,bp.BusinessProfileId,userId) ? bp.UserId : null,
                    Published = bp.Published,
                    MediaPaths = [.. _context.PostMedias.Where(x => x.BusinessPostId == bp.Id).Select(x=>$"{path}/Images/Businesss/{x.Path}")],
                    BusinessId = bp.BusinessProfileId
                }));

                dashboardPostDtos.AddRange(shelterPosts.Select(sp => new DashboardPostRepsonseDTO
                {
                    Title = sp.Title,
                    Body = sp.Body,
                    UserId = DoesUserOwnProfile(sp,sp.ShelterProfileId,userId) ? sp.UserId : null,
                    Published = sp.Published,
                    PhotoPath = sp.PhotoPath,
                    Type = sp.Type,
                    ShelterId = sp.ShelterProfileId,
                    AdoptedAt = sp.AdoptedAt,
                    Status = sp.Status
                }));

                dashboardPostDtos.AddRange(lostAnimalPosts.Select(lp => new DashboardPostRepsonseDTO
                {
                    Id = lp.Id,
                    Title = lp.Title,
                    Body = lp.Body,
                    UserId = lp.UserId,
                    Published = lp.Published,
                    PhotoPath = lp.PhotoPath,
                    Type = lp.Type,
                    Status = lp.Status
                }));

                return dashboardPostDtos.AsQueryable();
        }

        public PagedList<DashboardPostRepsonseDTO> GetPosts(PostParameters postParameters, string? userId, string path)
        {
            return PagedList<DashboardPostRepsonseDTO>.ToPagedList(FindAllPosts(userId,path).OrderByDescending(x=>x.Published),
                postParameters.PageNumber);
        }

        public async Task<FoundAnimalPostRepsonseDTO> MarkLostAnimalAsFound(int id, string userId)
        {
            var post = _context.LostAnimalPosts.FirstOrDefault(x=>x.Id == id);
            if (post == null)
            {
                throw new KeyNotFoundException();
            }

            if (post.UserId != userId)
            {
                throw new ArgumentException();
            }

            if (post.Status == "found")
            {
                throw new InvalidOperationException("Pet already marked as found");
            }

            post.Status = "found"; 

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("Error saving to database");
            }

            return new FoundAnimalPostRepsonseDTO
            {
                Id = post.Id,
                PhotoPath = post.PhotoPath,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Status = post.Status
            };
        }

        public async Task MarkAnimalAsAdopted(AdoptionRequestAnswerDTO dto)
        {
            var post = _context.AnimalAdoptionPosts.First(x=>x.Id == dto.AdoptionPostId);

            if (post.Status != "available")
            {
                throw new InvalidOperationException("Animal already marked as adopted!");
            }

            post.Status = "adopted";
            post.AdoptedAt = DateTime.Now;
            _context.AdoptionRequests.Where(x=>x.AdoptionPostId == dto.AdoptionPostId).ToList().ForEach(x=>x.Status = "rejected");
            
            var acceptedRequest = _context.AdoptionRequests
                .FirstOrDefault(x=>x.UserId==dto.userId && x.AdoptionPostId == dto.AdoptionPostId);

            if(acceptedRequest!=null)
            {
                acceptedRequest.Status = "accepted";
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("Error saving to database");
            }
        }
    }

}
