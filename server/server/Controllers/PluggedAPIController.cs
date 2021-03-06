﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api/PluggedAPI")]
    [ApiController]
    public class PluggedAPIController : ControllerBase
    {
        private readonly PluggedContext _context;

        public PluggedAPIController(PluggedContext context)
        {
            _context = context;
        }

        // GET: api/PluggedAPI/posts
        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        
        //Load all of the Profiles who have applied for a given audition (id)
        [HttpGet("applicants/{id}")]
        public async Task<ActionResult<IEnumerable<Profile>>> GetApplicants(int id)
        {
            var auditionprofile = _context.AuditionProfiles.Where(a => a.AuditionId == id).ToList();

            var applicantProfiles = new List<Profile>();

            foreach(AuditionProfile elem in auditionprofile)
            {
                applicantProfiles.Add(_context.Profiles.Find(elem.ProfileId));
            }

            return applicantProfiles;
        }

        //Load all of the Profiles who are members of a given ensemble (id)
        [HttpGet("members/{id}")]
        public async Task<ActionResult<IEnumerable<Profile>>> GetMembers(int id)
        {
            var profileEnsemble = _context.ProfileEnsembles.Where(a => a.EnsembleId == id).ToList();

            var memberProfiles = new List<Profile>();

            foreach (ProfileEnsemble elem in profileEnsemble)
            {
                memberProfiles.Add(_context.Profiles.Find(elem.ProfileId));
            }

            return memberProfiles;
        }


        //Display all information about a given audition (id)
        [HttpGet("auditions/{id}")]
        public async Task<ActionResult<Audition>> GetAudition(int id)
        {
            var audition = await _context.Auditions.FindAsync(id);

            if (audition == null)
            {
                return NotFound();
            }

            return audition;
        }

        //Change any information about a given audition (id)
        [HttpPost("auditions/{id}")]
        public async Task<IActionResult> PostAudition(int id, Audition aud)
        {

            if (id != aud.AuditionId)
            {
                return BadRequest();
            }

            _context.Entry(aud).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();

        }


        //Close a given audition (id) by setting its closed date to now
        [HttpGet("closeAud/{id}")]
        public async Task<IActionResult> CloseAudition(int id)
        {

            var audition = await _context.Auditions.FindAsync(id);

            if (audition == null)
            {
                return NotFound();
            }

            audition.Closed_Date = System.DateTime.Now;

            await _context.SaveChangesAsync();


            return NoContent();

        }

        //Open a given audition (id) by setting its closed date to 30 days from now
        [HttpGet("openAud/{id}")]
        public async Task<IActionResult> OpenAudition(int id)
        {

            var audition = await _context.Auditions.FindAsync(id);

            if (audition == null)
            {
                return NotFound();
            }

            System.DateTime today = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(30, 0, 0, 0);
            audition.Closed_Date = today.Add(duration);

            await _context.SaveChangesAsync();

            return NoContent();

        }

        public class AddProfiletoEnsemble
        {
            public string name { get; set; }
            public string EnsembleId { get; set; }
        }

        public class RemoveProfileEnsemble
        {
            public string ProfileId { get; set; }
            public string EnsembleId { get; set; }
        }

        //Add a profile to an ensemble
        [HttpPost("addProfile")]
        public async Task<IActionResult> addProfile(AddProfiletoEnsemble addition)
        {
            var name = addition.name.Split();
            Console.WriteLine(name[0]);
            Console.WriteLine(name[1]);
            Console.WriteLine(addition.EnsembleId);
            var Profiles = _context.Profiles.Where(p => p.First_Name == name[0] && p.Last_Name == name[1]).ToList();

            foreach(Profile profile in Profiles)
            {
                ProfileEnsemble profens = new ProfileEnsemble();
                profens.Start_Date = System.DateTime.Now;
                profens.End_Date = System.DateTime.MaxValue;
                profens.ProfileId = profile.ProfileId;
                profens.Profile = profile;
                profens.Ensemble = _context.Ensembles.Find(int.Parse(addition.EnsembleId));
                profens.EnsembleId = int.Parse(addition.EnsembleId);

                if (ModelState.IsValid)
                {
                    _context.Add(profens);
                    await _context.SaveChangesAsync();
                }

            }

            return NoContent();

        }


        //Remove a profile from an ensemble.
        [HttpPost("remProfile")]
        public async Task<IActionResult> removeProfile(RemoveProfileEnsemble rm)
        {
            var prof_ens = _context.ProfileEnsembles.Where(p => p.EnsembleId == int.Parse(rm.EnsembleId) && p.ProfileId == int.Parse(rm.ProfileId)).ToList()[0];
            _context.Remove(prof_ens);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        // GET: api/PluggedAPI/posts/5
        [HttpGet("posts/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        /*
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        */


        // POST api/PluggedAPI/posts
        [HttpPost("posts")]
        public async Task<ActionResult<Post>> Postpost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.PostId }, post);
        }
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
