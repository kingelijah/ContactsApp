using ContactsApp.Contracts;
using ContactsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Controllers
{
    // <summary>
    /// The controller groups together all methods related to Contacts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
       private IContactRepository _contactRepo;
       
        public ContactController(IContactRepository contactRepo)
        {
            _contactRepo = contactRepo;
        }
        // <summary>
        /// Method to get contact by Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {          
            var contact = await _contactRepo.GetContact(id);
            if (contact == null)
                return NotFound();
            
            return contact;
        }

        // <summary>
        /// Method to get edit history by Id.
        /// </summary>
        [HttpGet("GetEditHistory/{id}")]
        public async Task<ActionResult<IEnumerable<EditHistory>>> GetEditHistory(int id)
        {
            var history = await _contactRepo.GetEditHistory(id);
            if (history == null)
                return NotFound();

            return history.ToList();
        }

        // <summary>
        /// Method to get all contacts
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
    
            var contacts = await _contactRepo.GetContacts();
            if (contacts == null)
                return NotFound();
            return contacts.ToList();
           
        }

        // <summary>
        /// method to post contact
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            if (contact == null)
                return BadRequest();
            
           var newContact = await _contactRepo.AddContact(contact);
            if (newContact == null) 
                return BadRequest("Email already exist");
            return CreatedAtAction(nameof(GetContact), new { id = contact.Id}, contact);

        }

        // <summary>
        /// method to update contact
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<int>> UpdateContact(Contact contact)
        {
            if (contact == null)           
                return BadRequest();
            
            int result = await _contactRepo.UpdateContact(contact);
            if (result == 0)
                return NotFound();
            return Ok(result);
        }

        // <summary>
        /// Method to delete contact
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteContact(int id)
        {
            int result = await _contactRepo.DeleteContact(id);
            if(result == 0) 
                return NotFound(); 

            return Ok(id);
        }
       
    }
}
