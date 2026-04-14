using FinCouple.Application.DTOs.Categories;
using FinCouple.Application.Services.Interfaces;
using FinCouple.Domain.Entities;
using FinCouple.Domain.Repositories;

namespace FinCouple.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.Select(MapToResponse);
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = Category.Create(request.Name, request.Color, request.Icon, request.IsDefault);
        await _categoryRepository.AddAsync(category, cancellationToken);
        return MapToResponse(category);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (category is null) return false;
        await _categoryRepository.DeleteAsync(category, cancellationToken);
        return true;
    }

    private static CategoryResponse MapToResponse(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Color = category.Color,
        Icon = category.Icon,
        IsDefault = category.IsDefault
    };
}
