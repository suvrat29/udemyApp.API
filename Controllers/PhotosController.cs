using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using udemyApp.API.Data;
using udemyApp.API.Dtos;
using udemyApp.API.Helpers;
using udemyApp.API.Models;

namespace udemyApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(DataContext context, IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _context = context;
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        // POST: api/users/5/photos
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            try
            {
                if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                {
                    throw new Exception($"The user you are trying to update with id {userId} is not authorized to perform this action.");
                }
            }
            catch (Exception error)
            {
                var function = "AddPhotoForUser";
                var page = "PhotosController";
                var user = $"{userId}";
                Extensions.AddToApplicationLog(error.Message, error.Source, error.StackTrace, function, page, user, _context);

                return Unauthorized();
            }

            try
            {
                var userFromRepo = await _repo.GetUser(userId);

                var file = photoForCreationDto.File;

                var uploadResult = new ImageUploadResult();

                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(file.Name, stream),
                            Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        };

                        uploadResult = _cloudinary.Upload(uploadParams);

                        try
                        {
                            if (uploadResult.Error != null && uploadResult.Error.Message.Length > 0)
                            {
                                throw new Exception(uploadResult.Error.Message);
                            }
                        }
                        catch (Exception error)
                        {
                            var function = "AddPhotoForUser";
                            var page = "PhotosController";
                            var user = $"{userId}";
                            Extensions.AddToApplicationLog(error.Message, error.Source, error.StackTrace, function, page, user, _context);
                        }
                    }
                }

                photoForCreationDto.Url = uploadResult.Uri.ToString();
                photoForCreationDto.PublicId = uploadResult.PublicId;

                var photo = _mapper.Map<Photo>(photoForCreationDto);

                if (!userFromRepo.Photos.Any(userimage => userimage.IsMain))
                    photo.IsMain = true;

                userFromRepo.Photos.Add(photo);

                if (await _repo.SaveAll())
                {
                    var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);

                    return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id }, photoToReturn);
                }
                else
                {
                    throw new Exception($"There was an error when saving the file by user {userId}.");
                }
            }
            catch (Exception error)
            {
                var function = "AddPhotoForUser";
                var page = "PhotosController";
                var user = $"{userId}";
                Extensions.AddToApplicationLog(error.Message, error.Source, error.StackTrace, function, page, user, _context);

                return BadRequest("Could not add the photo.");
            }
        }
    }
}