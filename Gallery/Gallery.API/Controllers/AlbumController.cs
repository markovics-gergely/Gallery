using Gallery.BLL.Infrastructure.Commands;
using Gallery.BLL.Infrastructure.DataTransferObjects;
using Gallery.BLL.Infrastructure.Queries;
using Gallery.BLL.Infrastructure.ViewModels;
using Gallery.DAL.Configurations.Interfaces;
using Gallery.DAL.Types;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(Roles = RoleTypes.All)]
    public class AlbumController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IGalleryConfigurationService _config;

        public AlbumController(IMediator mediator, IGalleryConfigurationService config)
        {
            _mediator = mediator;
            _config = config;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<EnumerableWithCountViewModel<AlbumViewModel>>> GetAlbums(
            [FromQuery] GetAlbumsDTO dto,
            CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetAlbumsQuery(dto, user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("config")]
        public IActionResult GetConfig()
        {
            return Ok(new { MaxUploadSize = _config.GetMaxUploadSize(), MaxUploadCount = _config.GetMaxUploadCount() });
        }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<EnumerableWithCountViewModel<AlbumViewModel>>> GetUserAlbums(
            [FromRoute] Guid userId,
            [FromQuery] GetAlbumsDTO dto,
            CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetAlbumsQuery(dto, user, userId);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("user")]
        public async Task<ActionResult<EnumerableWithCountViewModel<UserProfileViewModel>>> GetOwnUserAlbums(
            [FromQuery] GetAlbumsDTO dto,
            CancellationToken cancellationToken)
        {
            var query = new GetOwnAlbumsQuery(dto, HttpContext.User);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("favorites")]
        public async Task<ActionResult<EnumerableWithCountViewModel<AlbumViewModel>>> GetUserFavoriteAlbums(
            [FromQuery] GetAlbumsDTO dto,
            CancellationToken cancellationToken)
        {
            var query = new GetUserFavoriteAlbumsQuery(dto, HttpContext.User);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("{albumId}")]
        [AllowAnonymous]
        public async Task<ActionResult<AlbumViewModel>> GetAlbumDetails(
            [FromRoute] Guid albumId,
            CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetAlbumDetailsQuery(albumId, user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAlbum([FromForm] CreateAlbumDTO dto, CancellationToken cancellationToken)
        {
            var command = new CreateAlbumCommand(dto, HttpContext.User);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("{albumId}/like")]
        [AllowAnonymous]
        public async Task<IActionResult> LikeAlbum([FromRoute] Guid albumId, CancellationToken cancellationToken)
        {
            var command = new LikeAlbumCommand(albumId);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPut("{albumId}")]
        public async Task<IActionResult> EditAlbumData([FromRoute] Guid albumId, [FromBody] EditAlbumDTO editAlbumDTO, CancellationToken cancellationToken)
        {
            var command = new EditAlbumDataCommand(albumId, editAlbumDTO, HttpContext.User);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPut("{albumId}/pictures/add")]
        public async Task<IActionResult> UploadAlbumPictures(
            [FromRoute] Guid albumId, 
            [FromForm] AddAlbumPicturesDTO dto, 
            CancellationToken cancellationToken)
        {
            var command = new AddPicturesToAlbumCommand(albumId, dto, HttpContext.User);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPut("{albumId}/pictures/remove")]
        public async Task<IActionResult> RemoveAlbumPictures(
            [FromRoute] Guid albumId, 
            [FromBody] RemoveAlbumPicturesDTO dto, 
            CancellationToken cancellationToken)
        {
            var command = new RemovePicturesFromAlbumCommand(albumId, dto, HttpContext.User);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("favorites/{albumId}")]
        public async Task<IActionResult> AddFavorite([FromRoute] Guid albumId, CancellationToken cancellationToken)
        {
            var command = new AddFavoriteCommand(albumId, HttpContext.User);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpDelete("favorites/{albumId}")]
        public async Task<IActionResult> RemoveFavorite([FromRoute] Guid albumId, CancellationToken cancellationToken)
        {
            var command = new RemoveFavoriteCommand(albumId, HttpContext.User);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpDelete("{albumId}")]
        public async Task<IActionResult> DeleteAlbum([FromRoute] Guid albumId, CancellationToken cancellationToken)
        {
            var command = new DeleteAlbumCommand(albumId, HttpContext.User);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

    }
}
