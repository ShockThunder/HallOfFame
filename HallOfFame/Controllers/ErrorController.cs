﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFame.Controllers
{
    /// <summary>
    /// Generates server response in case of error
    /// </summary>
    public class ErrorController
    {
        public JsonResult NotFound404()
        {
            return new JsonResult("object not found")
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        public JsonResult InternalError500()
        {
            return new JsonResult("object not found")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public JsonResult InternalError500(string e)
        {
            return new JsonResult(e)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public JsonResult BadRequest400()
        {
            return new JsonResult("incorrect request")
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }


    }
}
