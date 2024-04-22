using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Milestone.Models;


namespace Milestone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController:ControllerBase
    {
        private readonly Dictionary<int, string> _data;
        private readonly Dictionary<string, string> _cmnt;
        public ToDoController(Dictionary<int, string> data, Dictionary<string, string> comment)
        {
            _data = data;
            _cmnt = comment;
        }
        
        [HttpGet("FetchAllTask")]
        public IActionResult GetTask()
        {
            var tasks = _data.Select(kv => new { Id = kv.Key, task = kv.Value }).ToList();

            return Ok(tasks);
        }
       
        //[HttpGet("FetchTaskByRole")]
        //public IActionResult GatTask()
        //{
            
        //}
        [HttpPost("CreateComment")]
        public IActionResult AddComment(string Role, string Comment)
        {
            if (Comment == null) return BadRequest("Comment can't be null");
            try
            {
                _cmnt.TryAdd(Role,Comment);
                return Ok(_cmnt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost("CreateTask")]
        public IActionResult PostTask(int Id, string task)
        {
            if (task == null)
            {
                return BadRequest("Task cannot be null");
            }
            _data.TryAdd(Id, task);
            return Ok(_data);
        }
        #region UpdateRequest
        [HttpPut("UpdateTaskDetails")]
        public IActionResult PutTask(int Id, string task)
        {
            if (task == null)
            {
                return BadRequest("Task cannot be null");
            }

            if (!_data.ContainsKey(Id))
            {
                return NotFound("Task not found");
            }
            _data[Id] = task;
            return Ok(_data);
        }
        [HttpPut("UpdateTaskId")]
        public IActionResult PutId(int oldId, int newId)
        {
            if (oldId == newId)
            {
                return BadRequest("new ID is equal to old");
            }
            string oldTask = _data[oldId];
            _data.Remove(oldId);
            _data[newId] = oldTask;

            return (Ok(_data));
        }
        #endregion
        #region Patch
        [HttpPatch("UpdateTask/{id}")]
        public IActionResult UpdateIdOrTask(int id, [FromBody] TaskUpdateDto taskUpdateDto)
        {
            if (id == 0) // Assuming 0 is not a valid ID
            {
                return BadRequest("Invalid ID");
            }

            if (!_data.ContainsKey(id))
            {
                return NotFound("Task not found");
            }

            // Retrieve the existing task content associated with the ID
            string existingData = _data[id];

            //update the task content if provided
            if (!string.IsNullOrEmpty(existingData))
            {
                existingData = taskUpdateDto.Task;
            }

            //Update the id if it exist
            if (taskUpdateDto.NewId != id && taskUpdateDto.NewId.HasValue)
            {
                _data.Remove(id);
                // Add a new entry with the updated ID and the same task content
                _data[taskUpdateDto.NewId.Value] = existingData;
            }

            return Ok(_data);
        }
        #endregion
    }
}
