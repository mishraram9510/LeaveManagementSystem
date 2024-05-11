using LeaveApprovalSystem.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LeaveApprovalSystem.SProc;
using System.Runtime.Remoting.Lifetime;

namespace LeaveApprovalSystem.Controllers
{
    public class LeaveController : Controller
    {
        string str = @"Data Source=DESKTOP-QK4C7BN;Initial Catalog=LeaveSystem;Persist Security Info=True;User ID=sa;Password=satest";
        // GET: Leave
        Sproc sproc = new Sproc();
        public ActionResult Index()
        {
            Employee employee = TempData["Employee"] as Employee;
            TempData.Keep("Employee");
            ViewData["LoginUser"] = employee.Username;
            return View(LeaveData());
        }

        // GET: Leave/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Leave/Create
        public ActionResult Create()
        {
            Employee employee = TempData["Employee"] as Employee;
            TempData.Keep("Employee");
            ViewData["NameFlag"] = employee.Name;
            return View();
        }

        // POST: Leave/Create
        [HttpPost]
        public ActionResult Create(Leave leave)
        {
            try
            {
                Employee employee = TempData["Employee"] as Employee;
                using (SqlConnection con = new SqlConnection(str))
                {
                    con.Open();
                    TempData.Keep("Employee");
                    SqlCommand cmd = new SqlCommand(sproc.LeaveEntry, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@emp_id",employee.id);
                    cmd.Parameters.AddWithValue("@from_date", leave.from_date);
                    cmd.Parameters.AddWithValue("@to_date", leave.to_date);
                    cmd.Parameters.AddWithValue("@leave_type", leave.leave_type);
                    cmd.Parameters.AddWithValue("@reason", leave.reason);
                    cmd.Parameters.AddWithValue("@status", "Pending");
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

        // GET: Leave/Edit/5
        public ActionResult Edit(int id,Leave leave, int x = 1)
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
            if(dt.Rows.Count == 1)
            {
                leave.id =Convert.ToInt32(dt.Rows[0][0]);
                leave.emp_id = Convert.ToInt32(dt.Rows[0][1]);
                leave.from_date = Convert.ToDateTime(dt.Rows[0][2]);
                leave.to_date = Convert.ToDateTime(dt.Rows[0][3]);
                leave.leave_type = Convert.ToString(dt.Rows[0][4]);
                leave.reason = Convert.ToString(dt.Rows[0][5]);
                leave.Name = Convert.ToString(dt.Rows[0][7]);
                leave.Username = Convert.ToString(dt.Rows[0][8]);
                leave.LeaveError = leave.LeaveError;
            }

            return View(leave);
        }

        // POST: Leave/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Leave leave)
        {
            try
            {
                Employee employee = TempData["Employee"] as Employee;
                using (SqlConnection con = new SqlConnection(str))
                {
                    con.Open();
                    TempData.Keep("Employee");
                    SqlCommand cmd = new SqlCommand(sproc.LeaveEditUpdate, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@emp_id", employee.id);
                    cmd.Parameters.AddWithValue("@from_date", leave.from_date);
                    cmd.Parameters.AddWithValue("@to_date", leave.to_date);
                    cmd.Parameters.AddWithValue("@leave_type", leave.leave_type);
                    cmd.Parameters.AddWithValue("@reason", leave.reason);
                    cmd.Parameters.AddWithValue("@status", "Pending");
                    cmd.Parameters.AddWithValue("@id", leave.id);
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

        // GET: Leave/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sproc.LeaveDelete, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
            }
            return RedirectToAction("Index");
        }

        // POST: Leave/Delete/5
        public DataTable LeaveData()
        {
            DataTable dt = new DataTable();
            Employee employee = TempData["Employee"] as Employee;
            using (SqlConnection con = new SqlConnection(str))
            {
                con.Open();
                TempData.Keep("Employee");
                SqlCommand cmd = new SqlCommand(sproc.LeaveSelect, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", employee.id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            return dt;
        }
        //public View ViewBagF()
        //{
        //    ViewData["LoginFlag"] = "Invalid Username and Password";
        //    return ViewData["LoginFlag"];
        //}

    }
}
