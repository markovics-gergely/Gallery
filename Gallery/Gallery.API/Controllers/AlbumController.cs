using Gallery.BLL.Infrastructure.Commands;
using Gallery.BLL.Infrastructure.DataTransferObjects;
using Gallery.BLL.Infrastructure.Queries;
using Gallery.BLL.Infrastructure.ViewModels;
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

        public AlbumController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<EnumerableWithCountViewModel<AlbumListViewModel>>> GetAlbums(
            [FromQuery] GetAlbumsDTO dto, 
            CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetAlbumsQuery(dto, user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<EnumerableWithCountViewModel<AlbumListViewModel>>> GetUserAlbums(
            [FromRoute] Guid userId, 
            [FromQuery] GetAlbumsDTO dto, 
            CancellationToken cancellationToken)
        {
            var query = new GetAlbumsQuery(dto, HttpContext.User, userId);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("favorites")]
        public async Task<ActionResult<EnumerableWithCountViewModel<AlbumListViewModel>>> GetUserFavoriteAlbums(
            [FromQuery] GetAlbumsDTO dto,
            CancellationToken cancellationToken)
        {
            var query = new GetUserFavoriteAlbumsQuery(dto, HttpContext.User);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpGet("{albumId}")]
        [AllowAnonymous]
        public async Task<ActionResult<AlbumDetailsViewModel>> GetAlbumDetails(
            [FromRoute] Guid albumId, 
            CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetAlbumDetailsQuery(albumId, user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum([FromForm] CreateAlbumDTO dto, CancellationToken cancellationToken)
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

        [HttpPut("favorites")]
        public async Task<IActionResult> EditFavorites([FromBody] EditFavoritesDTO dto, CancellationToken cancellationToken)
        {
            var command = new EditFavoritesCommand(dto, HttpContext.User);
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
