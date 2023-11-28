using App.Services.Categories;
using App.ViewModels.Categories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Common;

namespace Test.Category;

[TestClass]
public class CategoryTests
{
    [TestMethod]
    public async Task InsertCategoryReturnsId()
    {
        var mapper = CommonTestObjects.GetMapper();
        var dbContext = CommonTestObjects.GetDbContext();
        var categoryService = new CategoryService(dbContext, mapper);

        var categoryCreateDto = GetDefaultCategoryObject();
        var id = await categoryService.InsertAsync(categoryCreateDto);

        Assert.IsTrue(id > 0);
    }

    [TestMethod]
    public async Task UpdateCategoryReturnsId()
    {
        var mapper = CommonTestObjects.GetMapper();
        var dbContext = CommonTestObjects.GetDbContext();
        var categoryService = new CategoryService(dbContext, mapper);

        var categoryCreateDto = GetDefaultCategoryObject();
        var createdId = await categoryService.InsertAsync(categoryCreateDto);

        var category = await categoryService.GetByIdAsync(createdId);
        var categoryEditDto = new CategoryEditDto
        {
            Id = createdId,
            Title = "Test Updated",
            Url = category.Url,
            Text = category.Text,
            ImageUrl = category.ImageUrl
        };

        var updatedId = await categoryService.UpdateAsync(createdId, categoryEditDto);

        Assert.IsTrue(updatedId > 0);
    }

    private static CategoryCreateDto GetDefaultCategoryObject()
    {
        return new CategoryCreateDto
        {
            Title = "Test",
            Url = "Test",
            Text = "Test",
            ImageUrl = "Test"
        };
    }
}
