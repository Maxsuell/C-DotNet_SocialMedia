using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.DTO;
using api.Entities;
using api.Extensions;
using api.Interfaces;
using api.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    public class UsersController : BaseApiController
    {

        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUserName = user.UserName;

            if(string.IsNullOrEmpty(userParams.Gender))
            userParams.Gender = user.Gender == "male" ? "female" : "male";
            

            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(
                users.CurrentPage,
                userParams.PageSize, 
                users.TotalCount, 
                users.TotalPages
                );

            return Ok(users);

        }
        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(String UserName)
        {
            
            return await _userRepository.GetMemberAsync(UserName);                   

        }

        [HttpPut]
        public async Task<ActionResult<AppUser>> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var UserName = User.GetUsername();
            var user = await _userRepository.GetUserByUsernameAsync(UserName);

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);


            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null)return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;                
            }
            
            user.Photos.Add(photo);
            
            if(await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser", new {username = user.UserName} ,_mapper.Map<PhotoDto>(photo));

            }else {
                return BadRequest("Problem Addding photo");
            }

                        
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if( photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            
            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot delete your main photo");;

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);                
            }
            user.Photos.Remove(photo);
            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delelte photo");


        }
    }
}