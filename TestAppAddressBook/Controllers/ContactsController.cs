using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestAppAddressBook.Services;
using TestAppAddressBook.Models;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using TestAppAddressBook.ViewModels;
using Microsoft.Extensions.Logging;

namespace TestAppAddressBook.Controllers
{
    [Route("/api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ContactsController: ControllerBase
    {
        private readonly ILogger logger;
        private readonly IContactsService service;
        private readonly IMapper mapper;
        public ContactsController(IContactsService serv, IMapper map, ILogger<ContactsController> log) 
        {
            service = serv;
            mapper = map;
            logger = log;
        }

        // GET api/contacts
        /// <summary>
        /// Retrieves Contact's list
        /// </summary>
        /// <returns>A response with contact's list</returns>
        /// <response code="200">Returns the contact's list</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<Contact>>> GetContacts()
        {
            try
            {
                var result = mapper.Map<List<ContactViewModel>>(await service.GetContactsAsync());
                if (result != null)
                {
                    logger.LogDebug($"Returned all contacts from database");
                    return Ok(result);
                }
                logger.LogDebug($"No contacts in the database. Returned 404");
                return NotFound();
            }
            catch(Exception ex) 
            {
                logger.LogError($"Error in GetContacts action: {ex.Message}. Returned 500");
                return null;
            }
        }

        // GET api/contacts/3
        /// <summary>
        /// Retrieves a Contact by ID
        /// </summary>
        /// <param name="id">Contact id</param>
        /// <returns>A response with contact</returns>
        /// <response code="200">Returns contact</response>
        /// <response code="400">For bad request</response>
        /// <response code="404">If contact is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Contact>> GetContactById(int? id)
        {
            try
            {
                if (id != null)
                { 
                    var result = mapper.Map<ContactViewModel>(await service.GetContactByIdAsync(id));
                    if (result != null)
                    {
                        logger.LogDebug($"Returned ContactViewModel with id: {id}");
                        return Ok(result);
                    }
                    logger.LogDebug($"No contact with id: {id} in the database. Returned 404");
                    return NotFound();

                }
                logger.LogDebug($"Invalid Id sent from the client. Returned 400");
                return BadRequest();
            }
            catch(Exception ex)
            {
                logger.LogError($"Error inside GetContatById action: {ex.Message}. Returned 500");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/contacts/
        /// <summary>
        /// Creates a contact
        /// </summary>
        /// <param name="contact">Contact model</param>
        /// <returns>A response with number 1</returns>
        /// <response code="200">Returns number 1</response>
        /// <response code="201">A response as creation of contact</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateContact([FromBody] ContactViewModel contactViewModel)
        {
            try
            {
                if (contactViewModel != null)
                {
                    var result = await service.CreateContactAsync(mapper.Map<Contact>(contactViewModel));
                    if (result > 0)
                    {
                        logger.LogDebug($"Returned 200. Contact added to the database ");
                        return Ok();
                    }
                    logger.LogDebug("Invalid ContactViewModel object sent from the client. Returned 400");
                    return BadRequest();
                }
                logger.LogDebug("Invalid ContactViewModel object sent from the client. Returned 400");
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error inside GetContatById action: {ex.Message}. Returned 500");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/contacts/1
        /// <summary>
        /// Updates Contact 
        /// </summary>
        /// <param name="contact">Contact model</param>
        /// <returns>A response with number 1</returns>
        /// <response code="200">Returns number 1</response>
        /// <response code="201">A response of updating the contact</response>
        /// <response code="400">For bad request</response>
        /// <response code="404">If contact is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateContact(int? id, [FromBody] Contact contact)
        {
            try
            {
                if (contact != null && id != null)
                {
                    int result = await service.UpdateContactAsync(contact, id);
                    if (result > 0)
                    {
                        logger.LogDebug($"Returned 200. Contact with id: {id} updated in the database ");
                        return Ok();
                    }

                    logger.LogDebug($"No contact with id: {id} in the database. Returned 404");
                    return NotFound();
                }
                logger.LogDebug("Invalid ContactViewModel object and/or Id sent from the client. Returned 400");
                return BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in UpdateContact action: {ex.Message}. Returned 500");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/contacts/5
        /// <summary>
        /// Deletes an existing Contact
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <returns>A response with result of deleting the contact</returns>
        /// <response code="200">If contact was deleted successfully</response> 
        /// <response code="400">For bad request</response>
        /// <response code="404">If contact is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(int? id)
        {
            try
            {
                if (id != null)
                {
                    var result = await service.DeleteContactAsync(id);
                    if (result > 0)
                    {
                        logger.LogDebug($"Contact with Id: {id} deleted from the database. Returned 200");
                        return Ok();
                    }
                    logger.LogDebug($"No contact with id: { id} in the database. Returned 404");
                    return NotFound();
                }
                logger.LogDebug($"Invalid Id sent from the client. Returned 400");
                return BadRequest();
            }
            catch(Exception ex) 
            {
                logger.LogError($"Error in DeleteContact action: {ex.Message}. Returned 500");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
