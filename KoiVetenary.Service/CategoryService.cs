using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data;
using KoiVetenary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service
{
    public interface ICategoryService
    {
        Task<IKoiVetenaryResult> GetCategoriesAsync();

        Task<IKoiVetenaryResult> GetCategoryByIdAsync(int categoryId);

        Task<IKoiVetenaryResult> CreateCategory(Category category);

        Task<IKoiVetenaryResult> UpdateCategory(Category category);

        Task<IKoiVetenaryResult> DeleteCategory(int categoryId);

    }
    public class CategoryService : ICategoryService
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IKoiVetenaryResult> CreateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<IKoiVetenaryResult> DeleteCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<IKoiVetenaryResult> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            if (categories == null || !categories.Any())

                return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Category>());

            return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, categories);
        }

        public Task<IKoiVetenaryResult> GetCategoryByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IKoiVetenaryResult> UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
