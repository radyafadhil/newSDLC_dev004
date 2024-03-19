using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using new_SDLC.Models;

namespace new_SDLC.Controllers
{
    public class MasterController : Controller
    {

        db_sdlcDataContext db = new db_sdlcDataContext(ConfigurationManager.ConnectionStrings["DB_ICT_eSDLCConnectionString"].ConnectionString);

        // GET: Master

        #region master document
        public ActionResult MasterDocument()
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
        
        public JsonResult getMasterDoc()
        {
            try
            {
                var get = db.TBL_M_DOC_TYPEs.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err Logout :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getDetailDoc(string id)
        {
            try
            {
                var get = db.TBL_M_DOC_TYPEs.Where(x => x.DOC_TYPE_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();

                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err DeleteDocType :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteDocType(string id)
        {
            try
            {
                string kodeName = Session["kodename"].ToString().ToUpper();
                if (kodeName == "DEV004")
                {
                    var delete = db.TBL_M_DOC_TYPEs.Where(x => x.DOC_TYPE_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_DOC_TYPEs.DeleteOnSubmit(delete);
                    db.SubmitChanges();
                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else if (kodeName == "DEV007")
                {
                    var delete = db.TBL_M_DOC_TYPEs.Where(x => x.DOC_TYPE_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_DOC_TYPEs.DeleteOnSubmit(delete);
                    db.SubmitChanges();
                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Akses login tidak dapat hapus data ..." }, JsonRequestBehavior.AllowGet);
                }
                
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err DeleteDocType :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult simpanDoc(ClsMaster clsmaster)
        {
            try
            {
                TBL_M_DOC_TYPE newdata = new TBL_M_DOC_TYPE();
                newdata.DOC_TYPE_ID = Guid.NewGuid();
                newdata.TYPE = clsmaster.TYPE;
                db.TBL_M_DOC_TYPEs.InsertOnSubmit(newdata);
                db.SubmitChanges();

                return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err simpanDoc :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateDoc(ClsMaster clsmaster)
        {
            try
            {
                var update = db.TBL_M_DOC_TYPEs.Where(x => x.DOC_TYPE_ID.ToString().ToUpper() == clsmaster.ID_DOC.ToUpper()).FirstOrDefault();

                if (update != null)
                {
                    update.TYPE = clsmaster.TYPE.ToString();
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Data tidak ditemukan..." }, JsonRequestBehavior.AllowGet);
                }


            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err simpanDoc :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region master pic
        public ActionResult MasterPIC()
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

        public JsonResult getMasterPIC()
        {
            try
            {
                var get = db.TBL_M_PICs.OrderBy(x => x.NAME).ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getMasterPIC :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeletePIC(string id)
        {
            try
            {
                string kodeName = Session["kodename"].ToString().ToUpper();
                if (kodeName == "DEV004")
                {
                    var delete = db.TBL_M_PICs.Where(x => x.PIC_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_PICs.DeleteOnSubmit(delete);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else if (kodeName == "DEV007")
                {
                    var delete = db.TBL_M_PICs.Where(x => x.PIC_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_PICs.DeleteOnSubmit(delete);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Akses login tidak dapat hapus data ..." }, JsonRequestBehavior.AllowGet);
                }
                
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err DeletePIC :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getDetailPIC(string id)
        {
            try
            {
                var get = db.TBL_M_PICs.Where(x => x.PIC_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();

                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getDetailPIC :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult simpanPIC(ClsMaster clsmaster)
        {
            try
            {
                var cekDev = db.TBL_M_PICs.Where(x => x.NAME.ToUpper() == clsmaster.KODE_PIC.ToUpper()).FirstOrDefault();
                var cekDevName = db.TBL_M_PICs.Where(x => x.PIC_DEV.ToUpper() == clsmaster.NAME_PIC.ToUpper()).FirstOrDefault();

                if (cekDev != null)
                {
                    return Json(new { Status = false, Message = clsmaster.KODE_PIC.ToUpper() + " sudah terdaftar sebelumnya ... " }, JsonRequestBehavior.AllowGet);
                }

                else if (cekDevName != null)
                {
                    return Json(new { Status = false, Message = clsmaster.NAME_PIC.ToUpper() + " sudah terdaftar sebelumnya ... " }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    TBL_M_PIC newdt = new TBL_M_PIC();
                    newdt.PIC_ID = Guid.NewGuid();
                    newdt.NAME = clsmaster.KODE_PIC.ToString().ToUpper();
                    newdt.PIC_DEV = clsmaster.NAME_PIC.ToString().ToUpper();
                    newdt.PIC_OPR = clsmaster.OPR_PIC?.ToString().ToUpper();

                    db.TBL_M_PICs.InsertOnSubmit(newdt);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);

                }
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err simpanPIC :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdatePIC(ClsMaster clsmaster)
        {
            try
            {
                var update = db.TBL_M_PICs.Where(x => x.PIC_ID.ToString().ToUpper() == clsmaster.ID_PIC.ToUpper()).FirstOrDefault();

                if (update != null)
                {
                    update.NAME = clsmaster.KODE_PIC.ToString().ToUpper();
                    update.PIC_DEV = clsmaster.NAME_PIC.ToString().ToUpper();
                    update.PIC_OPR = clsmaster.OPR_PIC?.ToString().ToUpper();
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Data tidak ditemukan..." }, JsonRequestBehavior.AllowGet);
                }


            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err UpdatePIC :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region master platform
        public ActionResult MasterPlatform()
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

        public JsonResult getMasterPlatform()
        {
            try
            {
                var get = db.TBL_M_PLATFORMs.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getMasterPlatform :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getDetailPlatform(string id)
        {
            try
            {
                var get = db.TBL_M_PLATFORMs.Where(x => x.PLATFORM_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();

                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getDetailPlatform :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeletePlatform(string id)
        {
            try
            {
                string kodeName = Session["kodename"].ToString().ToUpper();
                if (kodeName == "DEV004")
                {
                    var delete = db.TBL_M_PLATFORMs.Where(x => x.PLATFORM_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_PLATFORMs.DeleteOnSubmit(delete);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else if (kodeName == "DEV007")
                {
                    var delete = db.TBL_M_PLATFORMs.Where(x => x.PLATFORM_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_PLATFORMs.DeleteOnSubmit(delete);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Akses login tidak dapat hapus data ..." }, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err DeletePlatform :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult simpanPlatform(ClsMaster clsmaster)
        {
            try
            {
                TBL_M_PLATFORM newdata = new TBL_M_PLATFORM();
                newdata.PLATFORM_ID = Guid.NewGuid();
                newdata.NAME = clsmaster.NAME_PLATFORM;
                db.TBL_M_PLATFORMs.InsertOnSubmit(newdata);
                db.SubmitChanges();

                return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err simpanPlatform :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdatePlatform(ClsMaster clsmaster)
        {
            try
            {
                var update = db.TBL_M_PLATFORMs.Where(x => x.PLATFORM_ID.ToString().ToUpper() == clsmaster.ID_PLATFORM.ToUpper()).FirstOrDefault();

                if (update != null)
                {
                    update.NAME = clsmaster.NAME_PLATFORM.ToString();
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Data tidak ditemukan..." }, JsonRequestBehavior.AllowGet);
                }


            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err UpdatePlatform :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region master server
        public ActionResult MasterServer()
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

        public JsonResult getMasterServer()
        {
            try
            {
                var get = db.TBL_M_SERVERs.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getMasterServer :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteServer(string id)
        {
            try
            {
                string kodeName = Session["kodename"].ToString().ToUpper();
                if (kodeName == "DEV004")
                {
                    var delete = db.TBL_M_SERVERs.Where(x => x.SERVER_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_SERVERs.DeleteOnSubmit(delete);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else if (kodeName == "DEV007")
                {
                    var delete = db.TBL_M_SERVERs.Where(x => x.SERVER_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_SERVERs.DeleteOnSubmit(delete);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Akses login tidak dapat hapus data ..." }, JsonRequestBehavior.AllowGet);
                }

                
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err DeleteServer :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getDetailServer(string id)
        {
            try
            {
                var get = db.TBL_M_SERVERs.Where(x => x.SERVER_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();

                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getDetailServer :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult simpanServer(ClsMaster clsmaster)
        {
            try
            {
                var cekDev = db.TBL_M_SERVERs.Where(x => x.NAME.ToUpper() == clsmaster.NAME_SERVER.ToUpper()).FirstOrDefault();
                
                if (cekDev != null)
                {
                    return Json(new { Status = false, Message = clsmaster.NAME_SERVER.ToUpper() + " sudah terdaftar sebelumnya ... " }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    TBL_M_SERVER newdt = new TBL_M_SERVER();
                    newdt.SERVER_ID = Guid.NewGuid();
                    newdt.NAME = clsmaster.NAME_SERVER.ToUpper();
                    newdt.REMARKS = clsmaster.REMARKS;

                    db.TBL_M_SERVERs.InsertOnSubmit(newdt);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);

                }
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err simpanServer :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateServer(ClsMaster clsmaster)
        {
            try
            {
                var update = db.TBL_M_SERVERs.Where(x => x.SERVER_ID.ToString().ToUpper() == clsmaster.ID_SERVER.ToUpper()).FirstOrDefault();

                if (update != null)
                {
                    update.NAME = clsmaster.NAME_SERVER.ToString();
                    update.REMARKS = clsmaster.REMARKS.ToString();
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Data tidak ditemukan..." }, JsonRequestBehavior.AllowGet);
                }


            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err UpdateServer :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region master status
        public ActionResult MasterStatus()
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

        public JsonResult getMasterStatus()
        {
            try
            {
                var get = db.TBL_M_STATUS.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getMasterStatus :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getDetailStatus(string id)
        {
            try
            {
                var get = db.TBL_M_STATUS.Where(x => x.STATUS_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();

                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getDetailStatus :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteStatus(string id)
        {
            try
            {
                string kodeName = Session["kodename"].ToString().ToUpper();
                if (kodeName == "DEV004")
                {
                    var delete = db.TBL_M_STATUS.Where(x => x.STATUS_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_STATUS.DeleteOnSubmit(delete);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else if (kodeName == "DEV007")
                {
                    var delete = db.TBL_M_STATUS.Where(x => x.STATUS_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_STATUS.DeleteOnSubmit(delete);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Akses login tidak dapat hapus data ..." }, JsonRequestBehavior.AllowGet);
                }

                
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err DeleteStatus :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult simpanStatus(ClsMaster clsmaster)
        {
            try
            {
                TBL_M_STATUS newdata = new TBL_M_STATUS();
                newdata.STATUS_ID = Guid.NewGuid();
                newdata.NAME = clsmaster.NAME_STATUS;
                db.TBL_M_STATUS.InsertOnSubmit(newdata);
                db.SubmitChanges();

                return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err simpanStatus :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateStatus(ClsMaster clsmaster)
        {
            try
            {
                var update = db.TBL_M_STATUS.Where(x => x.STATUS_ID.ToString().ToUpper() == clsmaster.ID_STATUS.ToUpper()).FirstOrDefault();

                if (update != null)
                {
                    update.NAME = clsmaster.NAME_STATUS.ToString();
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Data tidak ditemukan..." }, JsonRequestBehavior.AllowGet);
                }


            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err UpdateStatus :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region master step
        public ActionResult MasterStep()
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

        public JsonResult getMasterStep()
        {
            try
            {
                var get = db.TBL_M_STEPs.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getMasterStep :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getDetailStep(string id)
        {
            try
            {
                var get = db.TBL_M_STEPs.Where(x => x.STEP_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();

                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getDetailStep :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteStep(string id)
        {
            try
            {
                string kodeName = Session["kodename"].ToString().ToUpper();
                if (kodeName == "DEV004")
                {
                    var delete = db.TBL_M_STEPs.Where(x => x.STEP_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_STEPs.DeleteOnSubmit(delete);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else if (kodeName == "DEV007")
                {
                    var delete = db.TBL_M_STEPs.Where(x => x.STEP_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                    db.TBL_M_STEPs.DeleteOnSubmit(delete);
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Akses login tidak dapat hapus data ..." }, JsonRequestBehavior.AllowGet);
                }

                
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err DeleteStep :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult simpanStep(ClsMaster clsmaster)
        {
            try
            {
                TBL_M_STEP newdata = new TBL_M_STEP();
                newdata.STEP_ID = Guid.NewGuid();
                newdata.NAME = clsmaster.NAME_STEP;
                db.TBL_M_STEPs.InsertOnSubmit(newdata);
                db.SubmitChanges();

                return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err simpanStep :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateStep(ClsMaster clsmaster)
        {
            try
            {
                var update = db.TBL_M_STEPs.Where(x => x.STEP_ID.ToString().ToUpper() == clsmaster.ID_STEP.ToUpper()).FirstOrDefault();

                if (update != null)
                {
                    update.NAME = clsmaster.NAME_STEP.ToString();
                    db.SubmitChanges();

                    return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Data tidak ditemukan..." }, JsonRequestBehavior.AllowGet);
                }


            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err UpdateStep :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion 

    }

    public class ClsMaster
    {
        //document
        public string ID_DOC { get; set; }
        public string TYPE { get; set; }

        //pic
        public string ID_PIC { get; set; }
        public string KODE_PIC { get; set; }
        public string NAME_PIC { get; set; }
        public string OPR_PIC { get; set; }

        //platform
        public string ID_PLATFORM { get; set; }
        public string NAME_PLATFORM { get; set; }

        //server
        public string ID_SERVER { get; set; }
        public string NAME_SERVER { get; set; }
        public string REMARKS { get; set; }

        //status
        public string ID_STATUS { get; set; }
        public string NAME_STATUS { get; set; }

        //step
        public string ID_STEP { get; set; }
        public string NAME_STEP { get; set; }

    }
}