using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace Acme.BookStore {
    public class BookAppService_Tests : BookStoreApplicationTestBase {
        private readonly IBookAppService _bookAppService;
        public BookAppService_Tests() {
            _bookAppService = GetRequiredService<IBookAppService>();
        }

        [Fact]
        public async Task Should_List_Of_Books() {
            //Act 动作
            var result = await _bookAppService.GetListAsync(
                    new PagedAndSortedResultRequestDto()
                );
            //Assert 断言
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(b => b.Name == "Test book 1");
        }

        /// <summary>
        /// 测试创建一个合法book实体的场景:
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Create_A_Valid_Book() {
            //Act
            var result = await _bookAppService.CreateAsync(
                    new CreateUpdateBookDto {
                        Name = "New test book 43",
                        Price = 12,
                        PublishDate = DateTime.Now,
                        Type = BookType.ScienceFiction
                    }
                );

            //Assert 断言
            result.Id.ShouldNotBe(Guid.Empty);
            result.Name.ShouldBe("New test book 43");
        }

        /// <summary>
        /// 测试创建一个非法book实体失败的场景:
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Not_Create_A_Book_Without_Name() {
            var exception = await Assert.ThrowsAsync<Volo.Abp.Validation.AbpValidationException>(async () =>
            {
                await _bookAppService.CreateAsync(
                    new CreateUpdateBookDto() {
                        Name = "",
                        Price = 10,
                        PublishDate = DateTime.Now,
                        Type = BookType.ScienceFiction
                    }
                );
            });
            exception.ValidationErrors.ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));

        }
    }
}
