using AutoMapper;
using Book_Store.Authorization;
using Book_Store.Models;
using Book_Store.Models.Repository;
using Book_Store.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : Controller
    {
        public IDataRepository<AuthorModel> _repo { get; }
        public IMapper _mapper { get; }

        public AuthorsController(IDataRepository<AuthorModel> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<List<GetAuthorViewModel>> Get()
        {
            var authors = _repo.FindAll();

            List<GetAuthorViewModel> data = new List<GetAuthorViewModel>();
            foreach (var author in authors)
            {
                var newForm = _mapper.Map<GetAuthorViewModel>(author);
                data.Add(newForm);
            }

            return Ok(data);
        }

        [HttpGet("{Id}")]
        public ActionResult<GetAuthorViewModel> Get(string Id)
        {
            var author = _repo.FindById(Id);
            if (author == null)
                return NotFound();

            return Ok(_mapper.Map<GetAuthorViewModel>(author));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(PostAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    AuthorModel author = _mapper.Map<AuthorModel>(model);
                    author.Id = Guid.NewGuid().ToString();

                    _repo.Create(author);
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
        public IActionResult Put(string Id, PostAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try 
                {

                    AuthorModel author = _mapper.Map<AuthorModel>(model);
                    author.Id = Id;

                    var check = _repo.Update(author, Id);

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
