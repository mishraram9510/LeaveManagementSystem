using System.Web.Mvc;
using LeaveApprovalSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System;
using LeaveApprovalSystem.SProc;
using System.Net.Http.Headers;

namespace LeaveApprovalSystem.Controllers
{
    public class EmployeeController : Controller
    {
        
        string str = @"Data Source=DESKTOP-QK4C7BN;Initial Catalog=LeaveSystem;Persist Security Info=True;User ID=sa;Password=satest";
        // GET: Employee

        Sproc sproc = new Sproc();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(str))
                {
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(sproc.EmployeeSelect, connection: con)) 
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", employee.Username);
                        cmd.Parameters.AddWithValue("@Password", employee.Password);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        dt.Load(dr);
                        con.Close();
                        if (dt.Rows.Count > 0)
                        {
                            employee.id = Convert.ToInt32(dt.Rows[0][0]);
                            employee.Name = Convert.ToString(dt.Rows[0][1]);
                            employee.Designation = Convert.ToString(dt.Rows[0][2]);
                            employee.Username = Convert.ToString(dt.Rows[0][3]);
                            TempData["Employee"] = employee;
                            if (dt.Rows[0][2].ToString() == "Employee")
                            {
                                return RedirectToAction("Index", "Leave");
                            }
                            else if(dt.Rows[0][2].ToString() == "Manager")
                            {
                                return RedirectToAction("Index", "Manager");
                            }
                        }
                    }
                    ViewData["LoginFlag"] = "Invalid Username and Password";
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(str))
                {
                    con.Open();
                    string q = "insert into Employee values ('" + employee.Name + "','" + employee.Designation + "','" + employee.Username + "','" + employee.Password + "')";
                    SqlCommand cmd = new SqlCommand(q,con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
