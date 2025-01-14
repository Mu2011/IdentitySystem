using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentitySystem.Models;
using IdentitySystem.Data;

namespace IdentitySystem.Controllers;

public class CoursesController(IdentitySystemContext context) : Controller
{
  private readonly IdentitySystemContext _context = context;

  // GET: Courses
  [AllowAnonymous]
  public async Task<IActionResult> Index() => View(await _context.Course.ToListAsync());

  // GET: Courses/Details/5
  public async Task<IActionResult> Details(int? id)
  {
    if (id == null)
    {
      return NotFound();
    }

    var course = await _context.Course
        .FirstOrDefaultAsync(m => m.CourseID == id);
    if (course == null)
    {
      return NotFound();
    }

    return View(course);
  }

  // GET: Courses/Create
  // [Authorize]
  public IActionResult Create()
  {
    return View();
  }

  // POST: Courses/Create
  // To protect from overposting attacks, enable the specific properties you want to bind to.
  // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create([Bind("CourseID,CourseName,Price")] Course course)
  {
    if (ModelState.IsValid)
    {
      _context.Add(course);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }
    return View(course);
  }

  // [Authorize]
  // GET: Courses/Edit/5
  public async Task<IActionResult> Edit(int? id)
  {
    if (id == null)
    {
      return NotFound();
    }

    var course = await _context.Course.FindAsync(id);
    if (course == null)
    {
      return NotFound();
    }
    return View(course);
  }

  // POST: Courses/Edit/5
  // To protect from overposting attacks, enable the specific properties you want to bind to.
  // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(int id, [Bind("CourseID,CourseName,Price")] Course course)
  {
    if (id != course.CourseID)
    {
      return NotFound();
    }

    if (ModelState.IsValid)
    {
      try
      {
        _context.Update(course);
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!CourseExists(course.CourseID))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }
      return RedirectToAction(nameof(Index));
    }
    return View(course);
  }

  // [Authorize]
  // GET: Courses/Delete/5
  public async Task<IActionResult> Delete(int? id)
  {
    if (id == null)
    {
      return NotFound();
    }

    var course = await _context.Course
      .FirstOrDefaultAsync(m => m.CourseID == id);
    if (course == null)
    {
      return NotFound();
    }

    return View(course);
  }

  // POST: Courses/Delete/5
  [HttpPost, ActionName("Delete")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteConfirmed(int id)
  {
    var course = await _context.Course.FindAsync(id);
    if (course != null)
    {
      _context.Course.Remove(course);
    }

    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
  }

  private bool CourseExists(int id)
  {
    return _context.Course.Any(e => e.CourseID == id);
  }
}