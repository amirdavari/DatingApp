using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly ILikeRepository _likeRepository;

    public LikesController(IUserRepository userRepository, ILikeRepository likeRepository)
    {
        _userRepository = userRepository;
        _likeRepository = likeRepository;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
        var sourceId = User.GetUserId();
        var likedUser = await _userRepository.GetUserByUsernameAsync(username);
        var sourceUser = await _likeRepository.GetUserWithLikes(sourceId);

        if (likedUser == null) return NotFound();

        if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

        var userLike = await _likeRepository.GetUserLike(sourceId, likedUser.Id);

        if (userLike != null) return BadRequest("You already like this user");

        userLike = new UserLike { SourceUserId = sourceId, TargetUserId = likedUser.Id };
        sourceUser.LikekdUsers.Add(userLike);

        if (await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Addlike failed");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();

        var result = await _likeRepository.GetUserLiked(likesParams);
        Response.AddingPaginationHeader(new PaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages));
        
        return Ok(result);
    }

}
