using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Search_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllClients")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<DTOs.ClientDTOs.ClientDTO>> GetAllClients()
        {

            List<DTOs.ClientDTOs.ClientDTO> clientsList = BusinessLayer.clsClient.GetAllClients();
            if (clientsList.Count == 0)
            {
                return NotFound("No Clients Found!");
            }
            return Ok(clientsList);

        }


        [HttpGet("FindClient/{id}", Name = "GetClientById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<DTOs.ClientDTOs.ClientDTO> GetClientById(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return BadRequest($"Not accepted ID {id}");
            }

            BusinessLayer.clsClient client = BusinessLayer.clsClient.Find(id);

            if (client == null)
            {
                return NotFound($"Client with ID {id} not found.");
            }


            DTOs.ClientDTOs.ClientDTO SDTO = client.clientDTO;

            return Ok(SDTO);

        }


        [HttpPost("AddNew", Name = "AddClient")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<DTOs.ClientDTOs.ClientDTO> AddClient(DTOs.ClientDTOs.ClientDTO newClientDTO)
        {
            //we validate the data here
            if (newClientDTO == null || string.IsNullOrEmpty(newClientDTO.Name))
            {
                return BadRequest("Invalid client data.");
            }

            newClientDTO.Id = Guid.NewGuid().ToString();

            BusinessLayer.clsClient Client = new BusinessLayer.clsClient(new DTOs.ClientDTOs.ClientDTO(newClientDTO.Id, newClientDTO.Name, newClientDTO.created_at));
            
            Client.Save();

            newClientDTO.Id = Client.ID;

            return CreatedAtRoute("GetClientById", new { id = newClientDTO.Id }, newClientDTO);

        }

        [HttpPut("Update/{id}", Name = "UpdateClient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DTOs.ClientDTOs.ClientDTO> UpdateClient(string id, DTOs.ClientDTOs.ClientDTO updatedClient)
        {
            if (string.IsNullOrEmpty(id) || updatedClient == null || string.IsNullOrEmpty(updatedClient.Name))
            {
                return BadRequest("Invalid Client data.");
            }

            BusinessLayer.clsClient student = BusinessLayer.clsClient.Find(id);


            if (student == null)
            {
                return NotFound($"Client with ID {id} not found.");
            }


            student.Name = updatedClient.Name;


            if (student.Save())
                return Ok(student.clientDTO);

            else
                return StatusCode(500, new { message = "Error Updating Client" });

        }


        [HttpDelete("Delete/{id}", Name = "DeleteClient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteClient(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest($"Not accepted ID {id}");
            }


            if (BusinessLayer.clsClient.DeleteClient(id))
                return Ok($"Client with ID {id} has been deleted.");


            else
                return NotFound($"Client with ID {id} not found. no rows deleted!");
        }

    }



}

