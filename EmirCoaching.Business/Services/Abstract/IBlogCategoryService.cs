using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

// Blog filtre sekmeleri GetAllAsync ile listelenir
public interface IBlogCategoryService
    : ICrudService<BlogCategoryResponse, CreateBlogCategoryRequest, UpdateBlogCategoryRequest>;
