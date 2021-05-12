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

namespace Book_Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        public IDataRepository<BookModel> _repo { get; }
        public IMapper _mapper { get; }

        public BooksController(IDataRepository<BookModel> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet]
        public ActionResult<List<GetBookViewModel>> Get()
        {
            var books = _repo.FindAll();

            List<GetBookViewModel> data = new List<GetBookViewModel>();
            foreach (var book in books) {
                var newForm = _mapper.Map<GetBookViewModel>(book);
                data.Add(newForm);
            }

            return Ok(data);
        }

        [HttpGet("{Id}")]
        public ActionResult<GetBookViewModel> Get(string Id)
        {
            var book = _repo.FindById(Id);
            if (book == null)
                return NotFound();

            return Ok(_mapper.Map<GetBookViewModel>(book));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(PostBookViewModel model)
        {
            if (ModelState.IsValid) {

                try {

                    BookModel book = _mapper.Map<BookModel>(model);
                    book.Id = Guid.NewGuid().ToString();

                    _repo.Create(book);
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
        public IActionResult Put(string Id, PostBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    BookModel book = _mapper.Map<BookModel>(model);
                    book.Id = Id;

                    var check = _repo.Update(book, Id);

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
