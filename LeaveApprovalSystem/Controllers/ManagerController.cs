using LeaveApprovalSystem.SProc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaveApprovalSystem.Controllers;
using LeaveApprovalSystem.Models;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Remoting.Lifetime;

namespace LeaveApprovalSystem.Controllers
{
    public class ManagerController : Controller
    {
        string str = @"Data Source=DESKTOP-QK4C7BN;Initial Catalog=LeaveSystem;Persist Security Info=True;User ID=sa;Password=satest";
        LeaveController lc = new LeaveController();
        Sproc sproc = new Sproc();
        public ActionResult Index()
        {
            return View(ViewAllLeave());
        }

        // GET: Manager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Manager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Manager/Edit/5
        public ActionResult Edit(int id,Leave leave, int x =0)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sproc.LeaveEdit, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
            }
            if (dt.Rows.Count == 1)
            {
                leave.id = Convert.ToInt32(dt.Rows[0][0]);
                leave.emp_id = Convert.ToInt32(dt.Rows[0][1]);
                leave.from_date = Convert.ToDateTime(dt.Rows[0][2]);
                leave.to_date = Convert.ToDateTime(dt.Rows[0][3]);
                leave.leave_type = Convert.ToString(dt.Rows[0][4]);
                leave.reason = Convert.ToString(dt.Rows[0][5]);
                leave.status = Convert.ToString(dt.Rows[0][6]);
                leave.Name = Convert.ToString(dt.Rows[0][7]);
                leave.Username = Convert.ToString(dt.Rows[0][8]);
                leave.LeaveError = leave.LeaveError;
            }

            return View(leave);
        }

        // POST: Manager/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Leave leave)
        {
            try
            {
                //Employee employee = TempData["Employee"] as Employee;
                using (SqlConnection con = new SqlConnection(str))
                {
                    con.Open();
                    TempData.Keep("Employee");
                    SqlCommand cmd = new SqlCommand(sproc.LeaveStatusUpdate, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@status", leave.status.ToString());
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                leave.LeaveError = "Error occured while updating the data, please try again ;";
                return RedirectToAction("Edit", leave);
            }
        }

        // GET: Manager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Manager/Delete/5
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

        public DataTable ViewAllLeave()
        {
            //DataTable dt = new DataTable();
            //using (SqlConnection con = new SqlConnection(str))
            //{
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand(sproc.LeaveSelectAll, con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    da.Fill(dt);
            //    con.Close();
            //    ViewData["ManagerUser"] = employee.Username;
            //}
            //return dt;

            DataTable dt = new DataTable();
            Employee employee = TempData["Employee"] as Employee;
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                TempData.Keep("Employee");
                SqlCommand cmd = new SqlCommand(sproc.LeaveSelectAll, con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
                ViewData["ManagerUser"] = employee.Username;
            }
            return dt;
        }
    }
}
