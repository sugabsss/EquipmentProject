using Asp.Versioning;
using EquipmentManagement.API.Responses;
using EquipmentManagement.Application.Commands;
using EquipmentManagement.Application.Dtos;
using EquipmentManagement.Application.Queries;
using EquipmentManagement.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentManagement.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/equipamentos")]
    [Tags("Equipamento")]
    public class EquipmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EquipmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(WebApiResponse<PagedResult<EquipmentDto>>), HttpStatusCodeResponse.Ok)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.PartialContent)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.InternalServerError)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.NotFound)]
        public async Task<IActionResult> GetEquipment(
            CancellationToken cancellationToken,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 20)
        {
            var response = await _mediator.Send(new GetEquipmentQuery(pageIndex, pageSize), cancellationToken);

            if (!response.IsSuccess)
                return StatusCode(HttpStatusCodeResponse.PartialContent, WebApiResponse<string>.Fail(response.Error));

            return Ok(WebApiResponse<PagedResult<EquipmentDto>>.Ok(response.Value));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<EquipmentDto>), HttpStatusCodeResponse.Ok)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.PartialContent)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.InternalServerError)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.NotFound)]
        public async Task<IActionResult> GetEquipmentById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetEquipmentByIdQuery(id), cancellationToken);

            if (!response.IsSuccess)
                return StatusCode(HttpStatusCodeResponse.PartialContent, WebApiResponse<string>.Fail(response.Error));

            return Ok(WebApiResponse<EquipmentDto>.Ok(response.Value));
        }

        [HttpPost]
        [ProducesResponseType(typeof(WebApiResponse<InsertEquipmentCommand>), HttpStatusCodeResponse.Ok)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.PartialContent)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.InternalServerError)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.NotFound)]
        public async Task<IActionResult> InsertEquipment(
            [FromBody] InsertEquipmentCommand commandDto,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(commandDto, cancellationToken);

            if (!response.IsSuccess)
                return StatusCode(HttpStatusCodeResponse.PartialContent, WebApiResponse<string>.Fail(response.Error));

            return Ok(WebApiResponse<EquipmentDto>.Ok(response.Value));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<EquipmentDto>), HttpStatusCodeResponse.Ok)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.PartialContent)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.InternalServerError)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.NotFound)]
        public async Task<IActionResult> UpdateEquipment(
            [FromRoute] Guid id,
            [FromBody] UpdateEquipmentCommand commandDto,
            CancellationToken cancellationToken)
        {
            var commandWithId = commandDto with { Id = id };
            var response = await _mediator.Send(commandWithId, cancellationToken);

            if (!response.IsSuccess)
                return StatusCode(HttpStatusCodeResponse.PartialContent, WebApiResponse<string>.Fail(response.Error));

            return Ok(WebApiResponse<EquipmentDto>.Ok(response.Value));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<bool>), HttpStatusCodeResponse.Ok)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.PartialContent)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.InternalServerError)]
        [ProducesResponseType(typeof(WebApiResponse<string>), HttpStatusCodeResponse.NotFound)]
        public async Task<IActionResult> DeleteEquipment(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new DeleteEquipmentCommand(id), cancellationToken);

            if (!response.IsSuccess)
                return StatusCode(HttpStatusCodeResponse.PartialContent, WebApiResponse<string>.Fail(response.Error));

            return Ok(WebApiResponse<bool>.Ok(response.Value));
        }
    }
}