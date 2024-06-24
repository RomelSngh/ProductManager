using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Data;
using AutoMapper;
using ProductManagement.Mapper;

namespace LocationSearchApiMVCWithUsersTests1
{
    public abstract class TestBase : IDisposable
    {
        private ProductDbContext _dbContext;
        private IMapper _mapper;
        protected ProductDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                  .Options;
            _dbContext = new ProductDbContext(options);
            _dbContext.Database.EnsureCreated();
            return _dbContext;
        }

        protected IMapper GetMockMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mockMapper.CreateMapper();
            return mapper;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
