using App.Services.Categories;
using App.Services.Posts;
using App.ViewModels.Categories;
using App.ViewModels.Posts;
using Test.Common;

namespace Test.Post;

[TestClass]
public class PostTests
{
    [TestMethod]
    public async Task PostInsertAsync()
    {
        var mapper = CommonTestObjects.GetMapper();
        var dbContext = CommonTestObjects.GetDbContext();

        var categoryCreateDto = new CategoryCreateDto
        {
            Title = "Test",
            Url = "Test",
            Text = "Test",
            ImageUrl = "Test"
        };

        var categoryService = new CategoryService(dbContext, mapper);
        var categoryId = await categoryService.InsertAsync(categoryCreateDto);

        var postCreateDto = new PostCreateDto
        {
            Title = "Test",
            Url = "Test",
            Text = "Test",
            ImageUrl = "Test",
            SelectedCategories = new List<int> { categoryId }
        };

        var postService = new PostService(dbContext, mapper);
        var postId = await postService.InsertAsync(postCreateDto);

        Assert.IsTrue(postId > 0);
    }
}