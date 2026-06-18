using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Common.PermissionAttribute;

namespace LiveEvents.Api.Common.Controllers;

/// <summary>
/// Base controller para operaciones CRUD básicas.
/// Las acciones son heredadas por los controladores hijos y resuelven permisos
/// desde el prefijo de permiso definido en cada controlador concreto.
/// </summary>
public abstract class CrudControllerBase<TListResult, TDto, TCreateDto, TUpdateDto, TFilterDto> : ApiControllerBase, IPermissionPrefixProvider
{
    protected abstract string PermissionPrefix { get; }

    string IPermissionPrefixProvider.PermissionPrefix => PermissionPrefix;

    [HttpGet]
    [RequirePermissionFromController("read")]
    public async Task<IActionResult> GetAll([FromQuery] TFilterDto filter, CancellationToken cancellationToken)
        => HandleResult(await GetAllAsync(filter, cancellationToken));

    protected abstract Task<Result<TListResult>> GetAllAsync(TFilterDto filter, CancellationToken cancellationToken);

    [HttpGet("{id}")]
    [RequirePermissionFromController("read")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        => HandleResult(await GetByIdAsync(id, cancellationToken));

    protected abstract Task<Result<TDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    [HttpPost]
    [RequirePermissionFromController("create")]
    public async Task<IActionResult> Create([FromBody] TCreateDto createDto, CancellationToken cancellationToken)
    {
        var result = await CreateAsync(createDto, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetById), GetCreatedAtRouteValues(result.Value), result.Value);
        }

        return HandleError(result.Error);
    }

    protected abstract Task<Result<TDto>> CreateAsync(TCreateDto createDto, CancellationToken cancellationToken);
    protected abstract object GetCreatedAtRouteValues(TDto value);

    [HttpPut("{id}")]
    [RequirePermissionFromController("update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TUpdateDto updateDto, CancellationToken cancellationToken)
        => HandleResult(await UpdateAsync(id, updateDto, cancellationToken));

    protected abstract Task<Result<TDto>> UpdateAsync(Guid id, TUpdateDto updateDto, CancellationToken cancellationToken);

    [HttpDelete("{id}")]
    [RequirePermissionFromController("delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => HandleResult(await DeleteAsync(id, cancellationToken));

    protected abstract Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

public interface ICrudService<TListResult, TDto, TCreateDto, TUpdateDto, TFilterDto>
{
    Task<Result<TListResult>> GetAllAsync(TFilterDto filter, CancellationToken cancellationToken);
    Task<Result<TDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<TDto>> CreateAsync(TCreateDto createDto, CancellationToken cancellationToken);
    Task<Result<TDto>> UpdateAsync(Guid id, TUpdateDto updateDto, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

public abstract class ServiceCrudControllerBase<TListResult, TDto, TCreateDto, TUpdateDto, TFilterDto> : ApiControllerBase, IPermissionPrefixProvider
{
    protected virtual string PermissionPrefix => GetType().Name.EndsWith("Controller", StringComparison.Ordinal)
        ? GetType().Name[..^"Controller".Length].ToLowerInvariant()
        : GetType().Name.ToLowerInvariant();

    string IPermissionPrefixProvider.PermissionPrefix => PermissionPrefix;

    [HttpGet]
    [RequirePermissionFromController("read")]
    public async Task<IActionResult> GetAll(
        [FromQuery] TFilterDto filter,
        [FromServices] ICrudService<TListResult, TDto, TCreateDto, TUpdateDto, TFilterDto> crudService,
        CancellationToken cancellationToken)
        => HandleResult(await crudService.GetAllAsync(filter, cancellationToken));

    [HttpGet("{id}")]
    [RequirePermissionFromController("read")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] ICrudService<TListResult, TDto, TCreateDto, TUpdateDto, TFilterDto> crudService,
        CancellationToken cancellationToken)
        => HandleResult(await crudService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    [RequirePermissionFromController("create")]
    public async Task<IActionResult> Create(
        [FromBody] TCreateDto createDto,
        [FromServices] ICrudService<TListResult, TDto, TCreateDto, TUpdateDto, TFilterDto> crudService,
        CancellationToken cancellationToken)
    {
        var result = await crudService.CreateAsync(createDto, cancellationToken);

        if (!result.IsSuccess)
        {
            return HandleError(result.Error);
        }

        var createdAtRouteValues = TryGetCreatedAtRouteValues(result.Value);
        return createdAtRouteValues is not null
            ? CreatedAtAction(nameof(GetById), createdAtRouteValues, result.Value)
            : Ok(result.Value);
    }

    [HttpPut("{id}")]
    [RequirePermissionFromController("update")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] TUpdateDto updateDto,
        [FromServices] ICrudService<TListResult, TDto, TCreateDto, TUpdateDto, TFilterDto> crudService,
        CancellationToken cancellationToken)
        => HandleResult(await crudService.UpdateAsync(id, updateDto, cancellationToken));

    [HttpDelete("{id}")]
    [RequirePermissionFromController("delete")]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] ICrudService<TListResult, TDto, TCreateDto, TUpdateDto, TFilterDto> crudService,
        CancellationToken cancellationToken)
        => HandleResult(await crudService.DeleteAsync(id, cancellationToken));

    private static RouteValueDictionary? TryGetCreatedAtRouteValues(TDto value)
    {
        if (value is null)
        {
            return null;
        }

        var idProperty = value.GetType().GetProperty("Id");
        if (idProperty is null)
        {
            return null;
        }

        var idValue = idProperty.GetValue(value);
        if (idValue is null)
        {
            return null;
        }

        if (idValue is Guid guid && guid == Guid.Empty)
        {
            return null;
        }

        return new RouteValueDictionary { ["id"] = idValue };
    }
}
