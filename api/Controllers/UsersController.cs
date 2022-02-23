using System.Collections.Generic;
using System.Linq;
using api.Data;
using api.DTO;
using api.Entities;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    public class UsersController : BaseApiController
    {

        private readonly IMapper _mapper;

        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;

        }


        [HttpGet]
        public async Task<ActionResult<MemberDto>> GetUsers()
        {

            var users = await _userRepository.GetMembersAsync();

            return Ok(users);

        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(String UserName)
        {

            return await _userRepository.GetMemberAsync(UserName);

        }
    }
}