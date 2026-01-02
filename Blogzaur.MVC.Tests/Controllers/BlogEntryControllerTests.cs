using Xunit;
using Blogzaur.MVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using System.Net;
using Blogzaur.Application.BlogEntry;
using Blogzaur.Application.BlogEntry.Queries.GetBlogEntryById;
using Moq;
using Blogzaur.Application.BlogEntry.Queries.GetAllBlogEntries;
using Microsoft.AspNetCore.TestHost;

namespace Blogzaur.MVC.Controllers.Tests
{
    public class BlogEntryControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public BlogEntryControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task List_ReturnsViewWithExpectedData_ForExistingBlogEntries()
        {
            // arrange
            var blogEntries = new List<BlogEntryDto>
            {
                new BlogEntryDto { Id = 1, Title = "First Blog Entry", Content = "Content of the first blog entry." },
                new BlogEntryDto { Id = 2, Title = "Second Blog Entry", Content = "Content of the second blog entry." },
                new BlogEntryDto { Id = 3, Title = "Third Blog Entry", Content = "Content of the third blog entry." }
            };

            var mediatorMock = new Moq.Mock<MediatR.IMediator>();
            mediatorMock
                .Setup(BlogEntryDto => BlogEntryDto.Send(Moq.It.IsAny<GetAllBlogEntriesQuery>(), Moq.It.IsAny<CancellationToken>()))
                .ReturnsAsync(blogEntries);

            var client = _factory.WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services => services.AddScoped(_ => mediatorMock.Object))).CreateClient();

            // act
            var response = await client.GetAsync("/BlogEntry/List");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1 class=\"margin-large\" align=\"center\">Blog Entries</h1>");
            content.Should().Contain("First Blog Entry");
            content.Should().Contain("Second Blog Entry");
            content.Should().Contain("Third Blog Entry");

        }

        [Fact]
        public async Task List_ReturnsEmptyView_WhenNoBlogEntriesExist()
        {
            // arrange
            var blogEntries = new List<BlogEntryDto>();

            var mediatorMock = new Moq.Mock<MediatR.IMediator>();
            mediatorMock
                .Setup(BlogEntryDto => BlogEntryDto.Send(Moq.It.IsAny<GetAllBlogEntriesQuery>(), Moq.It.IsAny<CancellationToken>()))
                .ReturnsAsync(blogEntries);

            var client = _factory.WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services => services.AddScoped(_ => mediatorMock.Object))).CreateClient();

            // act
            var response = await client.GetAsync("/BlogEntry/List");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().NotContain("<div class=\"blog-card\" style=\"min-height:400px;\">");

        }
    }
}