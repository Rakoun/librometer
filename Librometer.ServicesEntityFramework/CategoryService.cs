using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Librometer.Model;
using Librometer.Model.Services;
using Librometer.ServicesSQLCE.Mappers;
using Librometer.ServicesSQLCE.datas;

namespace Librometer.ServicesSQLCE
{
    public partial class CategoryService
        : BaseService<CategoryModel, Category, CategoryCriteria>
        , ICategoryService
    {
        #region Méthodes spécifiques à ICategoryService
        //TODO: si nécessaire
        #endregion


        #region Méthodes surchargée de ServiceBase
        protected override Category MapToEntity(CategoryModel model)
        {
            var global = model.ToCategory();
            return global;
        }

        protected override CategoryModel MapToModel(Category entity)
        {
            var Category = entity.ToCategoryModel();

            return Category;
        }

        protected override IQueryable<Category> GetConcrete(CategoryCriteria criteria)
        {
            Thread.Sleep(3000);

            IQueryable<Category> categories =
                categories = from Category category in Context.Categories
                             select category;

            if (!string.IsNullOrEmpty(criteria.Name))
                categories = categories.Where(c => c.Name.Contains(criteria.Name));

            return categories;
        }

        protected override void DeepCopy(Category oldEntity, Category newEntity)
        {
            oldEntity.Id = newEntity.Id;
            oldEntity.Name = newEntity.Name;
        }

        #endregion // Méthodes surchargée de ServiceBase
    }
}

