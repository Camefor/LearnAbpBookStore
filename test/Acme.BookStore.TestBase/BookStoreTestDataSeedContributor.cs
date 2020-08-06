using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Acme.BookStore {
    public class BookStoreTestDataSeedContributor
        : IDataSeedContributor, ITransientDependency {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IGuidGenerator _guidGenerator;

        public BookStoreTestDataSeedContributor(
            IRepository<Book, Guid> bookRepository,
            IGuidGenerator guidGenerator) {

            _bookRepository = bookRepository;
            _guidGenerator = guidGenerator;
        }



        public async Task SeedAsync(DataSeedContext context) {
            /* Seed additional test data... */
            await _bookRepository.InsertAsync(
                    new Book(
                        id:_guidGenerator.Create(),
                        name:"Test book 1",
                        type: BookType.Fantastic,
                        publishDate:new DateTime(2016,02,03),
                        price:21
                    )
                );

            await _bookRepository.InsertAsync(
                   new Book(
                       id: _guidGenerator.Create(),
                       name: "Test book 2",
                       type: BookType.Science,
                       publishDate: new DateTime(2020, 08, 06),
                       price: 65
                   )
               );
        }
    }
}