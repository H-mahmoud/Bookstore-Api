using AutoMapper;
using Book_Store.Authorization;
using Book_Store.Models;
using Book_Store.Models.Repository;
using Book_Store.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : Controller
    {
        public IDataRepository<CategoryModel> _repo { get; }
        public IMapper _mapper { get; }

        public CategoriesController(IDataRepository<CategoryModel> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<List<GetCategoryViewModel>> Get()
        {
            var categories = _repo.FindAll();

            List<GetCategoryViewModel> data = new List<GetCategoryViewModel>();
            foreach (var category in categories)
            {
                var newForm = _mapper.Map<GetCategoryViewModel>(category);
                data.Add(newForm);
            }

            return Ok(data);
        }

        [HttpGet("{Id}")]
        public ActionResult<GetCategoryViewModel> Get(string Id)
        {
            var category = _repo.FindById(Id);
            if (category == null)
                return NotFound();

            return Ok(_mapper.Map<GetCategoryViewModel>(category));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(PostCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    CategoryModel category = _mapper.Map<CategoryModel>(model);
                    category.Id = Guid.NewGuid().ToString();

                    _repo.Create(category);
                    _repo.Save();

                    return Ok();
                }
                catch
                {
                    return BadRequest("Error");
                }
            }

            return BadRequest("Invalid Data Input");
        }

        [HttpPut("{Id}")]
        [Authorize]
        public IActionResult Put(string Id, PostCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    CategoryModel category = _mapper.Map<CategoryModel>(model);
                    category.Id = Id;

                    var check = _repo.Update(category, Id);

                    if (check == false)
                        return NotFound();
                    _repo.Save();

                    return Ok();
                }
                catch
                {
                    return BadRequest("Error");
                }

            }

            return BadRequest("Invalid Data Input");
        }

        [HttpDelete("{Id}")]
        [Authorize]
        public IActionResult Delete(string Id)
        {
            try
            {
                var check = _repo.Delete(Id);

                if (check == false)
                    return NotFound();

                _repo.Save();
            }
            catch
            {
                return BadRequest("Error");
            }


            return Ok();
        }
    }
}
