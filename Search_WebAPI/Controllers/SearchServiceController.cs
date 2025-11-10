using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Search_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchServiceController : ControllerBase
    {

        [HttpGet("GetPopularItems", Name = "PopularItems")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<DTOs.SearchDTOs.PopularItems>> GetPopularItems(string client_id)
        {

            List<DTOs.SearchDTOs.PopularItems> PopluarItemsList = BusinessLayer.clsSearch.GetPopularItems(client_id);

            if (PopluarItemsList.Count == 0)
            {
                return NotFound("No Items Found!");
            }
            return Ok(PopluarItemsList);

        }

        [HttpPost("AddNew", Name = "AddSearchRecord")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DTOs.SearchDTOs.SearchItem> AddSearchRecord(DTOs.SearchDTOs.SearchItem newSearchDTO)
        {
            //we validate the data here
            if (newSearchDTO == null)
            {
                return BadRequest("Invalid Search data.");
            }


            //to not allow user to save item doesnt exist 
            if(newSearchDTO.item_id != null && newSearchDTO.item_id == 0)
            {
                BusinessLayer.clsItem Item = BusinessLayer.clsItem.Find(newSearchDTO.item_id);

                if(Item == null)
                    return NotFound($"Item with ID {newSearchDTO.item_id} not found.");

            }

            newSearchDTO.id = Guid.NewGuid().ToString();
            BusinessLayer.clsSearch SearchRecord = new BusinessLayer.clsSearch(new DTOs.SearchDTOs.SearchItem(newSearchDTO.id, newSearchDTO.client_id, newSearchDTO.keyword, newSearchDTO.item_id, newSearchDTO.item_name, newSearchDTO.searched_at));
            
            if(SearchRecord.AddNewClient())
            {
                newSearchDTO.id = SearchRecord.id;
                return CreatedAtRoute("", new { id = newSearchDTO.id }, newSearchDTO);
            }

            else
                return StatusCode(500, new { message = "Error Adding Search Record" });

        }

    }
}
