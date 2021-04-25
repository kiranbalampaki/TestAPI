using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    public class ToDoController : ApiController
    {
        [HttpGet]
        public IEnumerable<ToDo> GetAll()
        {
            using (ToDoDBEntities dbContext = new ToDoDBEntities())
            {
                return dbContext.ToDos.ToList();
            }
        }

        [HttpGet]
        public IEnumerable<ToDo> GetActive()
        {
            using (ToDoDBEntities dbContext = new ToDoDBEntities())
            {
                return dbContext.ToDos.Where(x => x.IsActive == true && x.ToDoDateTime > DateTime.Now).ToList();
            }
        }

        [HttpGet]
        public ToDo GetSingle(int? Id)
        {
            using (ToDoDBEntities dbContext = new ToDoDBEntities())
            {
                return dbContext.ToDos.SingleOrDefault(x => x.ID == Id);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddToDO(ToDo model)
        {
            using (ToDoDBEntities dbContext = new ToDoDBEntities())
            {
                try
                {
                    model.IsActive = true;
                    dbContext.ToDos.Add(model);
                    dbContext.SaveChanges();
                    HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Created);
                    return responseMessage;
                }
                catch (Exception ex)
                {
                    HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    return responseMessage;
                }
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateToDo(int id, ToDo model)
        {
            using (ToDoDBEntities dbContext = new ToDoDBEntities())
            {
                try
                {
                    if (id == model.ID)
                    {
                        dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                        return responseMessage;
                    }
                    else
                    {
                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.NotModified);
                        return responseMessage;
                    }
                }
                catch (Exception ex)
                {
                    HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    return responseMessage;
                }
            }
        }

        [HttpPut]
        public HttpResponseMessage CompleteToDo(int id)
        {
            using (ToDoDBEntities dbContext = new ToDoDBEntities())
            {
                try
                {
                    ToDo toDo = dbContext.ToDos.Find(id);
                    toDo.IsActive = false;
                    dbContext.Entry(toDo).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                    HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                    return responseMessage;
                }
                catch (Exception ex)
                {
                    HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    return responseMessage;
                }
            }
        }

        public HttpResponseMessage DeleteToDo(int id)
        {
            using (ToDoDBEntities dbContext = new ToDoDBEntities())
            {
                try
                {
                    ToDo toDo = dbContext.ToDos.Find(id);
                    if (toDo != null)
                    {
                        dbContext.ToDos.Remove(toDo);
                        dbContext.SaveChanges();
                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                        return responseMessage;
                    }
                    else
                    {
                        HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
                        return responseMessage;
                    }
                }
                catch (Exception ex)
                {
                    HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    return responseMessage;
                }
            }
        }
    }
}