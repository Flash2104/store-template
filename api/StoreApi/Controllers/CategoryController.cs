using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Contracts;
using Store.Service.Contracts.Category;
using Store.Service.Contracts.Category.CreateTree;
using Store.Service.Contracts.Category.GetTree;
using Store.Service.Contracts.Category.UpdateTree;
using Store.Service.Contracts.Store.Update;
using StoreApi.AuthPolicies;
using StoreApi.Models;
using StoreApi.Models.Category.CreateTree;
using StoreApi.Models.Category.GetTree;
using StoreApi.Models.Category.ListTrees;
using StoreApi.Models.Category.UpdateTree;
using StoreApi.Models.Store.Update;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RolesConst.Administrator)]
    public class CategoryController : RootController
    {
        private readonly ILogger<StoreController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly ICorrelationService _correlationService;

        public CategoryController(ILogger<StoreController> logger, ICategoryService categoryService, ICorrelationService correlationService) : base(logger)
        {
            _logger = logger;
            _categoryService = categoryService;
            _correlationService = correlationService;
        }

        [HttpGet("list")]
        [Authorize(Roles = RolesConst.Manager)]
        public async Task<ServerResponseDto<ListCategoryTreesResponseDto>> ListAll()
        {
            var logPath = $"{nameof(CategoryController)} {nameof(ListAll)} | ";
            return await HandleRequest(
                _categoryService.ListTrees,
                res => new ListCategoryTreesResponseDto(res.Trees.ToList()),
                logPath
            );
        }

        [HttpGet("get/{id:int}")]
        [Authorize(Roles = RolesConst.Manager)]
        public async Task<ServerResponseDto<GetCategoryTreeResponseDto>> Get(int id)
        {
            var logPath = $"{nameof(CategoryController)} {nameof(Get)} | ";
            return await HandleGetRequest(
                _categoryService.GetTree,
                () => new GetCategoryTreeRequest(id),
                res => new GetCategoryTreeResponseDto(res.CategoryTree),
                logPath
            );
        }

        [HttpPost("update-tree")]
        [Authorize(Roles = RolesConst.Manager)]
        public async Task<ServerResponseDto<UpdateCategoryTreeResponseDto>> UpdateTree([FromBody] UpdateCategoryTreeRequestDto request)
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(CategoryController)} {nameof(UpdateTree)} | ";
            return await HandleRequest(
                _categoryService.UpdateTree,
                request,
                dto => new UpdateCategoryTreeRequest(),
                res => new UpdateCategoryTreeResponseDto(),
                logPath
            );
        }


        [HttpPost("create-tree")]
        [Authorize(Roles = RolesConst.Manager)]
        public async Task<ServerResponseDto<CreateCategoryTreeResponseDto>> CreateTree([FromBody] CreateCategoryTreeRequestDto request)
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(CategoryController)} {nameof(CreateTree)} | ";
            return await HandleRequest(
                _categoryService.CreateTree,
                request,
                dto => new CreateCategoryTreeRequest(),
                res => new CreateCategoryTreeResponseDto(),
                logPath
            );
        }
    }
}
