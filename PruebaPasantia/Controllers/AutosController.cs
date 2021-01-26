using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PruebaPasantia.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaPasantia.Controllers
{
    public class AutosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioAuto repositorio;

        public AutosController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioAuto repositorio)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
        }
        // GET: HomeController1
        public ActionResult Index()
        {
            var autos = repositorio.ObtenerTodos();
            return View(autos);

        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            return View(e);
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Auto u)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {

                int res = repositorio.Alta(u);

                if (u.Foto1File != null && u.IdAuto > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "Foto1_" + u.IdAuto + Path.GetExtension(u.Foto1File.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Foto1 = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        u.Foto1File.CopyTo(stream);
                    }

                    if (u.Foto2File != null && u.IdAuto > 0)
                    {
                        wwwPath = environment.WebRootPath;
                        path = Path.Combine(wwwPath, "Uploads");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                        fileName = "Foto2_" + u.IdAuto + Path.GetExtension(u.Foto2File.FileName);
                        pathCompleto = Path.Combine(path, fileName);
                        u.Foto2 = Path.Combine("/Uploads", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            u.Foto2File.CopyTo(stream);
                        }

                    }
                    else {
                        u.Foto2 = "";
                    }
                    repositorio.Modificacion(u);
                }

                

                TempData["Mensaje"] = RepositorioBase.mensajeExitoso("create");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return View();
            }
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            var u = repositorio.ObtenerPorId(id);

            return View(u);
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Auto u)
        {
            try
            {
                u.IdAuto = id;
                if (u.Foto1File != null)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "Foto1_" + u.IdAuto + Path.GetExtension(u.Foto1File.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Foto1 = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        u.Foto1File.CopyTo(stream);
                    }
                }
                else
                {
                    var us = repositorio.ObtenerPorId(id);
                    u.Foto1 = us.Foto1;
                }

                if (u.Foto2File != null)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "Foto2_" + u.IdAuto + Path.GetExtension(u.Foto2File.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Foto2 = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        u.Foto2File.CopyTo(stream);
                    }
                }
                else
                {
                    var us = repositorio.ObtenerPorId(id);
                    u.Foto2 = us.Foto2;
                }

                repositorio.Modificacion(u);
                TempData["Mensaje"] = RepositorioBase.mensajeExitoso("edit");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData["Error"] = RepositorioBase.mensajeError("edit");

                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);

            return View(entidad);
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Auto entidad)
        {
            try
            {
                entidad = repositorio.ObtenerPorId(id);

                repositorio.Baja(id);

                if (System.IO.File.Exists(environment.WebRootPath + "/Uploads/Foto1_" + id + Path.GetExtension(entidad.Foto1)))
                    System.IO.File.Delete(environment.WebRootPath + "/Uploads/Foto1_" + id + Path.GetExtension(entidad.Foto1));

                if (System.IO.File.Exists(environment.WebRootPath + "/Uploads/Foto2_" + id + Path.GetExtension(entidad.Foto2)))
                    System.IO.File.Delete(environment.WebRootPath + "/Uploads/Foto2_" + id + Path.GetExtension(entidad.Foto2));

                TempData["Mensaje"] = RepositorioBase.mensajeExitoso("delete");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = RepositorioBase.mensajeError("delete");
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }
    }
}
