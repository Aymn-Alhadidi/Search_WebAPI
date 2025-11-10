using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Search_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllItem")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<DTOs.ItemDTOs.ItemDTO>> GetAllItems()
        {

            List<DTOs.ItemDTOs.ItemDTO> ItemsList = BusinessLayer.clsItem.GetAllItems();
            if (ItemsList.Count == 0)
            {
                return NotFound("No Items Found!");
            }
            return Ok(ItemsList);

        }


        [HttpGet("FindItem/{id}", Name = "GetItemById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<DTOs.ItemDTOs.ItemDTO> GetItemById(int id)
        {

            if (id <= 0)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            BusinessLayer.clsItem client = BusinessLayer.clsItem.Find(id);

            if (client == null)
            {
                return NotFound($"Client with ID {id} not found.");
            }


            DTOs.ItemDTOs.ItemDTO IDTO = client.ItemDTO;

            return Ok(IDTO);

        }


        [HttpPost("AddNew", Name = "AddItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<DTOs.ItemDTOs.ItemDTO> AddItem(DTOs.ItemDTOs.ItemDTO newItemDTO)
        {
            //we validate the data here
            if (newItemDTO == null || string.IsNullOrEmpty(newItemDTO.Name))
            {
                return BadRequest("Invalid Item data.");
            }

            BusinessLayer.clsItem Item = new BusinessLayer.clsItem(new DTOs.ItemDTOs.ItemDTO(newItemDTO.Id, newItemDTO.Name, newItemDTO.created_at));

            if(Item.Save())
            {
                newItemDTO.Id = Item.ID;
                return CreatedAtRoute("", new { id = newItemDTO.Id }, newItemDTO);
            }

            else
                return StatusCode(500, new { message = "Error Adding Item" });
        }

        [HttpPut("Update/{id}", Name = "UpdateItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DTOs.ItemDTOs.ItemDTO> UpdateClient(int id, DTOs.ItemDTOs.ItemDTO updatedItem)
        {
            if (id <= 0 || updatedItem == null || string.IsNullOrEmpty(updatedItem.Name))
            {
                return BadRequest("Invalid Item data.");
            }

            BusinessLayer.clsItem Item = BusinessLayer.clsItem.Find(id);


            if (Item == null)
            {
                return NotFound($"Item with ID {id} not found.");
            }


            Item.Name = updatedItem.Name;


            if (Item.Save())
                return Ok(Item.ItemDTO);

            else
                return StatusCode(500, new { message = "Error Updating Item" });

        }


        [HttpDelete("Delete/{id}", Name = "DeleteItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteItem(int id)
        {
            if (id <= 0)
            {
                return BadRequest($"Not accepted ID {id}");
            }


            if (BusinessLayer.clsItem.DeleteItem(id))
                return Ok($"Item with ID {id} has been deleted.");


            else
                return NotFound($"Item with ID {id} not found. no rows deleted!");
        }
    }
}
