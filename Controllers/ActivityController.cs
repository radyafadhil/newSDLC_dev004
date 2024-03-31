using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.EnterpriseServices.Internal;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using new_SDLC.Models;

namespace new_SDLC.Controllers
{
    public class ActivityController : Controller
    {
        db_sdlcDataContext db = new db_sdlcDataContext(ConfigurationManager.ConnectionStrings["DB_ICT_eSDLCConnectionString"].ConnectionString);
        ClsUploadEmpty clsUpload = new ClsUploadEmpty();

        public ActionResult Index()
        {
            if (Session["kodename"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                var getPICid = db.TBL_M_PICs.Where(x => x.NAME == Session["kodename"].ToString()).FirstOrDefault();
                var getCheck = db.TBL_T_ACTIVITY1s.Where(x => x.PIC_ID == getPICid.PIC_ID.ToString().ToUpper() && x.END_DATETIME == null).OrderByDescending(x => x.START_DATETIME).FirstOrDefault();

                ViewBag.picid = getPICid.PIC_ID.ToString().ToUpper();

                if (getCheck != null)
                {
                    ViewBag.actid = getCheck.ACTIVITY_ID.ToString().ToUpper();
                    ViewBag.stateAct = "upd";

                    ViewBag.actdetail = getCheck.ACTIVITY_DETAIL?.ToString() ?? "";
                    ViewBag.remarks = getCheck.REMARKS?.ToString() ?? "";

                    var getApp = db.TBL_T_APPs.Where(x => x.APP_ID.ToString().ToUpper() == getCheck.APP_ID.ToString().ToUpper()).FirstOrDefault();
                    ViewBag.appid = getApp.APP_NAME.ToString();

                    var getVersion = db.TBL_T_VERSIONs.Where(x => x.VERSION_ID.ToString().ToUpper() == getCheck.VERSION_ID.ToString().ToUpper()).FirstOrDefault();
                    ViewBag.versionid = getVersion.VERSION.ToString();

                    var getStep = db.TBL_M_STEPs.Where(x => x.STEP_ID.ToString() == getCheck.STEP_ID.ToString().ToUpper()).FirstOrDefault();
                    ViewBag.stepid = getStep.NAME.ToString();

                }

                else
                {
                    ViewBag.actid = Guid.NewGuid().ToString().ToUpper();
                    ViewBag.stateAct = "new";
                }

                return View();
            }            
        }

        public ActionResult ReviewActivity()
        {
            if (Session["kodename"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                return View();
            }
        }
        
        public ActionResult IdxCutiIzin()
        {
            if (Session["kodename"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                return View();
            }
        }

        //================================GET DATA=====================
        public JsonResult GetAppName()
        {
            var get = db.TBL_T_APPs.ToList();
            return Json(new { data = get, Status = true, Message = "Data berhasil diambil!" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStep()
        {
            var get = db.TBL_M_STEPs.ToList();
            return Json(new { data = get, Status = true, Message = "Data berhasil diambil!" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVersion(string appId)
        {
            var get = db.TBL_T_VERSIONs.Where(x => x.APP_ID.ToString() == appId).ToList();
            return Json(new { data = get, Status = true, Message = "Data berhasil diambil!" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReviewActivity()
        {
            try
            {
                var get = db.cufn_getAllActivities1().Where(x => x.START_DATETIME.Value.Year == DateTime.Now.Year && x.PIC_NAME.ToString().ToUpper() == Session["kodename"].ToString())
                    .OrderByDescending(x => x.START_DATETIME);
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err GetReviewActivity : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult uploadEmpty(HttpPostedFileBase excel)
        {
            int success = 0, failed = 0;
            try
            {
                if (excel == null)
                {
                    return Json(new { Remarks = true, Success = "0", Failed = "0", Message = "Tidak ada data yang diupload.." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string message = "";
                    string nameGeoTmp = "";
                    string nameTmp = "";
                    System.Data.DataSet ds = new System.Data.DataSet();
                    if (Request.Files["excel"].ContentLength > 0)
                    {
                        string fileExtension = System.IO.Path.GetExtension(Request.Files["excel"].FileName);

                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            string fileLocation = Server.MapPath("~/Content/ContentApps/Upload_Ijin");
                            if (System.IO.File.Exists(fileLocation))
                            {
                                System.IO.File.Delete(fileLocation);
                            }

                            string filename = DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd-hh-mm-ss") + Path.GetFileName(excel.FileName);
                            string pathToExcelFile = Path.Combine(fileLocation, filename);
                            Request.Files["excel"].SaveAs(Path.Combine(fileLocation, filename));
                            string excelConnectionString = string.Empty;
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                            //connection String for xls file format.
                            if (fileExtension == ".xls")
                            {
                                excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.Combine(fileLocation, filename) + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            }
                            //connection String for xlsx file format.
                            else if (fileExtension == ".xlsx")
                            {
                                excelConnectionString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path.Combine(fileLocation, filename) + ";Extended Properties = 'Excel 12.0 Xml;HDR=YES;IMEX=2'; ");

                            }
                            //Create Connection to Excel work book and add oledb namespace
                            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                            excelConnection.Open();
                            DataTable dt = new DataTable();

                            dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            if (dt == null)
                            {
                                return null;
                            }

                            String[] excelSheets = new String[dt.Rows.Count];
                            int t = 0;
                            //excel data saves in temp file here.
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row.ItemArray[2].ToString() == "Sheet1$")
                                {
                                    excelSheets[t] = row["TABLE_NAME"].ToString();
                                    t++;
                                }
                            }
                            OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                            string query = string.Format("Select * from [{0}]", excelSheets[0]);
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                            {
                                dataAdapter.Fill(ds);
                            }
                        }

                        try
                        {
                            bool statusResult = true;
                            List<string> listIjin = new List<string>();
                            List<string> cekDev = new List<string>();
                            string dev = "";
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string username = ds.Tables[0].Rows[i][0].ToString();
                                string tanggal = ds.Tables[0].Rows[i][1].ToString();
                                string desc = ds.Tables[0].Rows[i][2].ToString();

                                cekDev.Add(username);

                                if (username == "")
                                {
                                    failed = failed + 1;
                                    statusResult = false;
                                }

                                else if (cekDev.Count > 0 && !cekDev.Contains(username))
                                {
                                    failed = failed + 1;
                                    statusResult = false;
                                }

                                else if (tanggal == "")
                                {
                                    failed = failed + 1;
                                    statusResult = false;
                                }
                                else if (desc == "")
                                {
                                    failed = failed + 1;
                                    statusResult = false;
                                }

                                else if (desc != "" && (desc.ToUpper() != "CUTI" && desc.ToUpper() != "IJIN" && desc.ToUpper() != "SAKIT" && desc.ToUpper() != "IZIN"))
                                {
                                    failed = failed + 1;
                                    statusResult = false;
                                }

                                else
                                {
                                    dev = username;

                                    string formatAsal = "yyyy-MM-dd"; // Format asal
                                    string formatTujuan = "dd/MM/yyyy"; // Format tujuan
                                    DateTime tanggalNew = DateTime.ParseExact(tanggal, formatAsal, CultureInfo.InvariantCulture);
                                    string tanggalBaru = tanggalNew.ToString(formatTujuan);

                                    listIjin.Add(tanggalBaru + "_" + desc);
                                    success = success + 1;
                                }
                            }

                            if (statusResult == true)
                            {
                                string getReturn = clsUpload.ProcessPdf(cekDev.FirstOrDefault(), listIjin);
                                if (getReturn.Contains("Err"))
                                {
                                    success = 0;
                                    failed = ds.Tables[0].Rows.Count;
                                    message = getReturn;
                                }
                            }

                            else
                            {
                                success = 0;
                            }

                        }
                        catch (Exception e)
                        {
                            failed = failed + 1;
                            message = "";
                            message = e.Message;
                        }
                    }

                    return Json(new { Remarks = true, Success = Convert.ToString(success), Failed = Convert.ToString(failed), Message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { Remarks = false, Success = Convert.ToString(success), Failed = Convert.ToString(failed), Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //================================GET DATA=====================

        public JsonResult CRUDActivity(ActivityTable ActTab)
        {
            try
            {

                if (ActTab.APP_ID.ToString() == "" || ActTab.VERSION_ID.ToString() == "" || ActTab.STEP_ID.ToString() == "")
                {
                    return Json(new { Status = false, Message = "Isi field yang masih kosong ...." }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    var getAppId = db.TBL_T_APPs.Where(x => x.APP_NAME.ToUpper() == ActTab.APP_ID.ToUpper() || x.APP_ID.ToString().ToUpper() == ActTab.APP_ID.ToUpper())
                        .FirstOrDefault();
                    var getVersionId = db.TBL_T_VERSIONs.Where(x => x.VERSION.ToUpper() == ActTab.VERSION_ID.ToUpper() || x.VERSION_ID.ToString().ToUpper() == ActTab.VERSION_ID.ToUpper())
                        .FirstOrDefault();
                    var getStepId = db.TBL_M_STEPs.Where(x => x.NAME.ToUpper() == ActTab.STEP_ID.ToUpper() || x.STEP_ID.ToString().ToUpper() == ActTab.STEP_ID.ToUpper())
                        .FirstOrDefault();

                    var check = db.TBL_T_ACTIVITY1s.Where(x => x.ACTIVITY_ID.ToString() == ActTab.ACTIVITY_ID).FirstOrDefault();

                    if (check == null && ActTab.statusBtn == "START")
                    {                        
                        TBL_T_ACTIVITY1 newData = new TBL_T_ACTIVITY1();

                        newData.ACTIVITY_ID = Guid.Parse(ActTab.ACTIVITY_ID.ToString().ToUpper());
                        newData.START_DATETIME = DateTime.Now;
                        newData.ACTIVITY_DETAIL = ActTab.ACTIVITY_DETAIL;
                        newData.REMARKS = ActTab.REMARKS;
                        newData.APP_ID = getAppId.APP_ID.ToString().ToUpper();
                        newData.VERSION_ID = getVersionId.VERSION_ID.ToString().ToUpper();
                        newData.PIC_ID = ActTab.PIC_ID.ToString().ToUpper();
                        newData.STEP_ID = getStepId.STEP_ID.ToString().ToUpper();

                        db.TBL_T_ACTIVITY1s.InsertOnSubmit(newData);

                    }

                    else if (check != null && ActTab.statusBtn == "START")
                    {
                        check.ACTIVITY_DETAIL = ActTab.ACTIVITY_DETAIL;
                        check.REMARKS = ActTab.REMARKS;
                    }

                    else if (check != null && ActTab.statusBtn == "END")
                    {
                        check.ACTIVITY_DETAIL = ActTab.ACTIVITY_DETAIL;
                        check.REMARKS = ActTab.REMARKS;
                        check.END_DATETIME = DateTime.Now;
                    }

                    db.SubmitChanges();
                    return Json(new { Status = true, Message = "Berhasil Simpan Data ...." }, JsonRequestBehavior.AllowGet);
                }                

            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err SaveActivity : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

    }

    public class ActivityTable
    {
        public string statusBtn { get; set; }
        public string ACTIVITY_ID { get; set; }
        public DateTime START_DATETIME { get; set; }
        public DateTime END_DATETIME { get; set; }
        public string ACTIVITY_DETAIL { get; set; }
        public string REMARKS { get; set; }
        public string APP_ID { get; set; }
        public string VERSION_ID { get; set; }
        public string PIC_ID { get; set; }
        public string STEP_ID { get; set; }
    }
}