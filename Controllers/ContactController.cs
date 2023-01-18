using AutoMapper;
using ContactsApp.ViewModel;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;

namespace ContactsApp.Controllers
{
    /// <summary>
    /// The controller groups together all methods related to Contacts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IValidator<ContactViewModel> _validator;
        private readonly ILogger<ContactController> _logger;


        public ContactController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<ContactViewModel> validator, ILogger<ContactController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
        }
        /// <summary>
        /// Method to get contact by Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactViewModel>> GetContact(int id)
        {
            _logger.LogInformation($"Start GetContact with request {id}");
            var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
            if (contact is null)
                return NotFound();
            
            return _mapper.Map<ContactViewModel>(contact);

        }

        /// <summary>
        /// Method to get edit history by Id.
        /// </summary>
        [HttpGet("GetEditHistory/{id}")]
        public ActionResult<IEnumerable<EditHistoryViewModel>> GetEditHistory(int id)
        {
            _logger.LogInformation($"Start GetEditHistory with request {id}");
            var histories = _unitOfWork.histories.GetHistory(id);
            if (histories is null)
                return NotFound();

            return _mapper.Map<IEnumerable<EditHistoryViewModel>>(histories).ToList();
        }

        /// <summary>
        /// Method to get all contacts
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<ContactViewModel>> GetContacts()
        {
            _logger.LogInformation("Start GetContacts");
            var contacts = _unitOfWork.Contacts.GetAll();
            if (contacts is null)
                return NotFound();
            return _mapper.Map<IEnumerable<ContactViewModel>>(contacts).ToList();

        }

        /// <summary>
        /// method to post contact
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> PostContact(ContactViewModel contactViewModel)
        {
            try
            {
                _logger.LogInformation($"Start PostContact with request {contactViewModel}");
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
                _logger.LogError($"Email Already Exists--- {contactViewModel.Email}");
                return BadRequest("Email Already Exists");
            }
           
        }

        /// <summary>
        /// method to update contact
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> UpdateContact(ContactViewModel contactViewModel)
        {
            _logger.LogInformation($"Start UpdateContact with request {contactViewModel}");
            ValidationResult result = await _validator.ValidateAsync(contactViewModel);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            
           _unitOfWork.Contacts.Update(_mapper.Map<Contact>(contactViewModel));
           await _unitOfWork.histories.AddEditAsync(contactViewModel.Id);
           _unitOfWork.Complete();
           return NoContent();
        }

        /// <summary>
        /// Method to delete contact
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            _logger.LogInformation($"Start DeleteContact with request {id}");
            var contact = await _unitOfWork.Contacts.GetByIdAsync(id);
            _unitOfWork.Contacts.Remove(contact);
            _unitOfWork.Complete();
            return NoContent();
        }
      
    }
}
