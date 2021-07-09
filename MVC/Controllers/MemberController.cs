using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Services;
using Microsoft.AspNetCore.Http;
namespace MVC.Controllers
{
    public class MemberController : Controller
    {

        private readonly ILogger<MemberController> _logger;

        private readonly IMemeberServices _memberService;
        public MemberController(ILogger<MemberController> logger, IMemeberServices memberService)
        {
            _logger = logger;
            _memberService = memberService;
        }   
 
        public IActionResult Rookies(string sortBy){
            List<Member> list = new List<Member>();
            if(String.IsNullOrEmpty(sortBy)){
                list = _memberService.AllMembers();
            }
            else if(sortBy == "Male"){
                list = _memberService.AllMembers().Where(x => x.Gender == "Male").ToList();
            }
           
           return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Member member)
        {
            if(ModelState.IsValid){
                _memberService.Create(member);
                return RedirectToAction("Rookies");
            }
            else{
                return BadRequest(ModelState);
            }           
             
            
        }

        public  IActionResult Edit(Guid id)
        {
            var model = _memberService.AllMembers().FirstOrDefault(x => x.Id == id);
            if(model == null) return NotFound();
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(Member member)
        {
            if(ModelState.IsValid){
                _memberService.Update(member);
                return RedirectToAction("Rookies");
            }
            return View(member);

        }

        public IActionResult Detail(Guid id){
            var model = _memberService.AllMembers().FirstOrDefault(x => x.Id == id);
            if(model == null) return NotFound();

            return View(model);
        }
        public IActionResult Delete(Guid id){
            
            var deletedMember  = _memberService.GetMemberById(id);
            // HttpContext.Session.SetString("Deleted", deletedMember.FullName());

            if(deletedMember == null) return NotFound();
            else{
                _memberService.Delete(id);
                return RedirectToAction("Confirm");
            }

        }

        public IActionResult Confirm(){
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
