using Microsoft.AspNetCore.Mvc;
using proyectoHospital.Models;
using proyectoHospital.Repositorio.Implementacion;
using proyectoHospital.Repositorio.Contrato;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;



namespace proyectoHospital.Controllers
{
    [Authorize]
    public class HospitalController : Controller
    {
        IPaciente _paciente;
        IMedico _medico;
        IArea _area;
        public HospitalController(IUsuario usuario)
        {
            _paciente = new PacienteRepositorio();
            _medico = new MedicoRepositorio();
            _area = new AreaRepositorio();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ClaimsPrincipal claimUser = HttpContext.User;
            string nombreUsuario = "";

            if (claimUser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                                .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["nombreUsuario"] = nombreUsuario;
        }
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("IniciarSesion", "Inicio");
        }
        /*
           ----------------PACIENTES-----------------
         */
        //LISTADO
        public async Task<IActionResult> Index()
        {

            return View(await Task.Run(()=> _paciente.listadoPacientes()));
        }
        //AGREGAR
        public async Task<IActionResult> agregarPaciente()
        {
            ViewBag.medico = new SelectList(_medico.listadoMedicos(), "cod_medico", "nom_med");
            ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area");
            return View( new Paciente());
        }
        [HttpPost]
        public async Task<IActionResult> agregarPaciente(Paciente pac)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.medico = new SelectList(_medico.listadoMedicos(), "cod_medico", "nom_med", pac.cod_medico);
                ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area", pac.cod_area);
                return View(await Task.Run(() => pac));
            }
            _paciente.ingresarPaciente(pac);
            ViewBag.medico = new SelectList(_medico.listadoMedicos(), "cod_medico", "nom_med", pac.cod_medico);
            ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area", pac.cod_area);
            TempData["MensajeAgregar"] = "Paciente Agregado Correctamente";
            return RedirectToAction("Index");
        }
        //ELIMINAR
        public async Task<IActionResult> eliminarPaciente(int? id)
        {
            if (id == null){return RedirectToAction("Index"); }
            Paciente pac = _paciente.buscar(id.Value); 
            if (pac == null)
            {
                return NotFound(); 
            }
            return View(await Task.Run(() => pac)); 
        }
        [HttpPost, ActionName("eliminarPaciente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> eliminarPaciente(int id)
        {
            _paciente.eliminarPaciente(id);
            TempData["MensajeEliminar"] = "Paciente Eliminado";
            return RedirectToAction("Index"); 
        }
        //ACTUALIZAR
        public async Task<IActionResult> actualizarPaciente(int? id = null)
        {
            if (id == null)
                return RedirectToAction("Index");
            Paciente pac = _paciente.buscar(id.Value);
            ViewBag.medico = new SelectList(_medico.listadoMedicos(), "cod_medico", "nom_med", pac.cod_medico);
            ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area", pac.cod_area);
            return View(pac);
        }
        [HttpPost]
        public async Task<IActionResult> actualizarPaciente(Paciente pac)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.medico = new SelectList(_medico.listadoMedicos(), "cod_medico", "nom_med", pac.cod_medico);
                ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area", pac.cod_area);
                return View(await Task.Run(() => pac));
            }
            _paciente.actualizarPaciente(pac.cod_paciente, pac.nom_pac, pac.ape_pac, pac.edad_pac, pac.dni, 
            pac.sexo_pac, pac.fec_nac, pac.contac_emer, pac.cod_medico, pac.cod_area);
            ViewBag.medico = new SelectList(_medico.listadoMedicos(), "cod_medico", "nom_med", pac.cod_medico);
            ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area", pac.cod_area);
            TempData["MensajeAlerta"] = "Paciente Actualizado Correctamente";
            return RedirectToAction("Index");
        }
        //INFO
        public async Task<ActionResult> infoPaciente(int id)
        {
            var paciente = _paciente.buscar(id);
            return View(paciente);
        }

        /*
           ----------------MEDICOS-----------------
         */
        //LISTADO
        public async Task<IActionResult> listarMedicos()
        {
            return View(await Task.Run(() => _medico.listadoMedicos()));
        }
        //AGREGAR
         public async Task<IActionResult> agregarMedico()
        {
            ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area"); // Asegúrate de implementar ListarAreas en tu repositorio de áreas
            return View(new Medico());
        }
        [HttpPost]
         public async Task<IActionResult> agregarMedico(Medico med)
         {
             if (!ModelState.IsValid)
             {
                ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area", med.cod_area);
                return View(await Task.Run(() => med));
             }
            ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area", med.cod_area);
            _medico.ingresarMedico(med);
            TempData["MensajeAgregar"] = "Médico Agregado Correctamente";
            return RedirectToAction("listarMedicos");
        }
        //ELIMINAR
        public async Task<IActionResult> eliminarMedico(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("listarMedicos"); // Si no se proporciona un ID, redirige a la lista de áreas.
            }
            Medico med = _medico.buscar(id.Value); // Busca el área para confirmar su eliminación.
            if (med == null)
            {
                return NotFound(); // Si no se encuentra el área, retorna un error 404.
            }
            return View(await Task.Run(() => med)); // Retorna la vista con el área a eliminar para confirmación.
        }
        [HttpPost, ActionName("eliminarMedico")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> eliminarMedico(int id)
        {
            ViewBag.mensaje = _medico.eliminarMedico(id); // Método que elimina el área y puede retornar un mensaje de éxito o error.
            TempData["MensajeEliminar"] = "Médico Eliminado";
            return RedirectToAction("listarMedicos"); // Redirecciona a la lista de áreas después de la eliminación.
        }
        //INFO
        public async Task<ActionResult> infoMedico(int id)
        {
            var medico = _medico.buscar(id);
            return View(medico);
        }
        //ACTUALIZAR
        public async Task<IActionResult> actualizarMedico(int? id = null)
        {
            if (id == null)
                return RedirectToAction("listarMedicos");
            Medico med = _medico.buscar(id.Value);
            ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area", med.cod_area);
            return View(med);
        }
        [HttpPost]
        public async Task<IActionResult> actualizarMedico(Medico med)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area", med.cod_area);
                return View(await Task.Run(() => med));
            }
            _medico.actualizarMedico(med.cod_medico, med.nom_med, med.edad_med, med.sexo_med, med.cod_area);
            ViewBag.area = new SelectList(_area.listadoAreas(), "cod_area", "nom_area", med.cod_area);
            TempData["MensajeAlerta"] = "Médico Actualizado Correctamente";
            return RedirectToAction("listarMedicos");
        }

        /*
           ----------------AREAS-----------------
         */
        public async Task<IActionResult> listarAreas()
        {
            return View(await Task.Run(() => _area.listadoAreas()));
        }

        public async Task<IActionResult> agregarArea()
        {
            return View(await Task.Run(() => new Area()));
        }
        [HttpPost]
        public async Task<IActionResult> agregarArea(Area ar)
        {
            if (!ModelState.IsValid)
            {
                return View(ar);
            }
            _area.ingresarArea(ar);
            TempData["MensajeAgregar"] = "Área Agregada Correctamente";
            return RedirectToAction("listarAreas");
        }
        public async Task<IActionResult> actualizarArea(int ? id = null)
        {
            if (id == null)
                return RedirectToAction("listarAreas");
            Area ar = _area.buscar(id.Value);
            return View(ar);
        }
        [HttpPost]public async Task<IActionResult> actualizarArea(Area ar)
        {
            if (!ModelState.IsValid)
            {
                return View( ar);
            }
            _area.actualizarArea(ar.cod_area, ar.nom_area);
            TempData["MensajeAlerta"] = "Área Actualizada Correctamente";
            return RedirectToAction("listarAreas");

        }
        
        public async Task<IActionResult> eliminarArea(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("listarAreas"); // Si no se proporciona un ID, redirige a la lista de áreas.
            }
            Area area = _area.buscar(id.Value); // Busca el área para confirmar su eliminación.
            if (area == null)
            {
                return NotFound(); // Si no se encuentra el área, retorna un error 404.
            }
            return View(await Task.Run(() => area)); // Retorna la vista con el área a eliminar para confirmación.
        }
        [HttpPost, ActionName("eliminarArea")]
        [ValidateAntiForgeryToken] // Protege contra ataques de falsificación de solicitud en sitios cruzados
        public async Task<IActionResult> eliminarArea(int id)
        {
            _area.eliminarArea(id); // Método que elimina el área y puede retornar un mensaje de éxito o error.
            TempData["MensajeEliminar"] = "Área Eliminada";
            return RedirectToAction("listarAreas"); // Redirecciona a la lista de áreas después de la eliminación.
        }










    }
}
