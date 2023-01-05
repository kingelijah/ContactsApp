using AutoMapper;
using ContactsApp.ViewModel;
using DataAccess.EFCore.Repositories;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ContactsApp.Controllers
{
    // <summary>
    /// The controller groups together all methods related to Contacts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IValidator<ContactViewModel> _validator;


        public ContactController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<ContactViewModel> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }
        // <summary>
        /// Method to get contact by Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactViewModel>> GetContact(int id)
        {          
            var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
            if (contact == null)
                return NotFound();
            
            return _mapper.Map<ContactViewModel>(contact);
        }

        // <summary>
        /// Method to get edit history by Id.
        /// </summary>
        [HttpGet("GetEditHistory/{id}")]
        public async Task<ActionResult<IEnumerable<EditHistoryViewModel>>> GetEditHistory(int id)
        {
            var histories = await _unitOfWork.histories.FindAsync(e => e.ContactId == id);
            if (histories == null)
                return NotFound();

            return _mapper.Map<IEnumerable<EditHistoryViewModel>>(histories).ToList();
        }

        // <summary>
        /// Method to get all contacts
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactViewModel>>> GetContacts()
        {
    
            var contacts = await _unitOfWork.Contacts.GetAllAsync();
            if (contacts == null)
                return NotFound();
            return _mapper.Map<IEnumerable<ContactViewModel>>(contacts).ToList();

        }

        // <summary>
        /// method to post contact
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> PostContact(ContactViewModel contactViewModel)
        {
            try
            {
                ValidationResult result = await _validator.ValidateAsync(contactViewModel);

                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                await _unitOfWork.Contacts.AddAsync(_mapper.Map<Contact>(contactViewModel));
                _unitOfWork.Complete();
                return Ok();
            }
            catch(DbUpdateException ex)
            {
               return BadRequest("Email Already Exists");
            }
           
        }

        // <summary>
        /// method to update contact
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> UpdateContact(ContactViewModel contactViewModel)
        {
            ValidationResult result = await _validator.ValidateAsync(contactViewModel);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            
            await _unitOfWork.Contacts.UpdateAsync(_mapper.Map<Contact>(contactViewModel));
            _unitOfWork.Complete();
           await SaveHistory(contactViewModel);
            return NoContent();
        }

        // <summary>
        /// Method to delete contact
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
            await _unitOfWork.Contacts.RemoveAsync(contact);
            _unitOfWork.Complete();
            return NoContent();
        }
        private async Task SaveHistory(ContactViewModel contactViewModel)
        {
            EditHistory hist = new EditHistory();
            hist.ContactId = contactViewModel.Id;
            hist.ModifiedDate = DateTime.Now;
             await _unitOfWork.histories.AddAsync(hist);
            _unitOfWork.Complete();
        }
    }
}
