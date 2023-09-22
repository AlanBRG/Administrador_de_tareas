using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using TareasCompletadas.Data;
using TareasCompletadas.Models;

namespace TareasCompletadas.Controllers
{
    public class TaskItemController : Controller
    {
        private readonly TaskContext _context;

        public TaskItemController(TaskContext context)
        {
            _context = context;
        }

        // GET: TaskItem
        public async Task<IActionResult> Index()
        {
            ViewBag.TareasIncompletas = await _context.Tasks.Where(p => !p.IsCompleted).ToListAsync(); //Recupera todas las tareas incompletas de la base de datos y las almcacena en un viewbag
            ViewBag.TareasCompletadas = await _context.Tasks.Where(p => p.IsCompleted).ToListAsync(); //Recupera todas las tareas completas de la base de datos y las almcacena en un viewbag


              return _context.Tasks != null ? 
                          View(await _context.Tasks.ToListAsync()) :
                          Problem("Entity set 'TaskContext.Tasks'  is null.");
        }

        // GET: TaskItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskItem = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // GET: TaskItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descripcion,FechaEntrega,IsCompleted")] TaskItem taskItem)
        {
            var TaskHistorial = new TaskHistorial
            {
                Id = taskItem.Id,
                Titulo = taskItem.Titulo,
                Descripcion = taskItem.Descripcion,
                FechaEntrega = taskItem.FechaEntrega,
                FechaCreacion = DateTime.Now,
                IsCompleted = taskItem.IsCompleted,
            };

            if (ModelState.IsValid)
            {
                _context.Add(taskItem);
                _context.Historial.Add(TaskHistorial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(taskItem);
        }

        // GET: TaskItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            return View(taskItem);
        }

        // POST: TaskItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,FechaEntrega,IsCompleted")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
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
            return View(taskItem);
        }

        // GET: TaskItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskItem = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // POST: TaskItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'TaskContext.Tasks'  is null.");
            }
            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem != null)
            {
                _context.Tasks.Remove(taskItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskItemExists(int id)
        {
          return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Historial()
        {
            return View(await _context.Historial.ToListAsync());
        }

        public async Task<IActionResult> ImprimirPDF(int id)
        {
            var tarea = await _context.Tasks.FindAsync(id);
            return new ViewAsPdf("ImprimirPDF", tarea)
            {
                FileName = $"Tarea.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }
    }
}
