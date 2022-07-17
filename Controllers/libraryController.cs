using library.Data;
using library.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library.Controllers
{
    [ApiController]
    [Route(template:"v1")]
    public class libraryController : ControllerBase
    {
        [HttpGet]
        [Route(template:"books")]
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context)
        {
            var books = await context.Books.AsNoTracking().ToListAsync();
            return Ok(books);
        }

        [HttpGet]
        [Route(template:"books/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            var book = await context.Books.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
            return book == null ?
            NotFound()
            : Ok(book);
        }

        [HttpPost (template:"books")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            Book model)
            {
                if(!ModelState.IsValid)
                return BadRequest();

                var book = new Book
                {
                    Title = model.Title,
                    Author = model.Author,
                    Edition = model.Edition
                };

                try
                {
                    await context.Books.AddAsync(book);
                    await context.SaveChangesAsync();
                    return Created(uri:$"v1/books/{book.Id}", book);
                }
                catch (System.Exception)
                {
                    return BadRequest();
                }
            }

        [HttpPut (template:"books/{id}")]
        public async Task<IActionResult> PutASync(
            [FromServices] AppDbContext context,
            Book model,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
                return NotFound();

                try
                {
                    book.Title = model.Title;
                    book.Author = model.Author;
                    book.Edition = model.Edition;

                    context.Books.Update(book);
                    await context.SaveChangesAsync();

                    return Ok(book);

                }
                catch (System.Exception)
                {
                    
                    return BadRequest();  
                }
        }

        [HttpDelete (template:"books/{id}")]

        public async Task<IActionResult> DeleteASync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
            {
                var book = await context.Books.FirstOrDefaultAsync(x=>x.Id == id);

                try
                {
                    context.Books.Remove(book);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                catch (System.Exception)
                {
                    
                    return BadRequest();
                }
            }
    }
}