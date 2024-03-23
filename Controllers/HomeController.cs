using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using Antlr.Runtime.Misc;
using FormsAuth;
using new_SDLC.Models;

namespace new_SDLC.Controllers
{
    
    public class HomeController : Controller
    {

        GeneralClass gc = new GeneralClass();
        db_sdlcDataContext dbSdlc = new db_sdlcDataContext(ConfigurationManager.ConnectionStrings["DB_ICT_eSDLCConnectionString"].ConnectionString);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult AppList()
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

        public ActionResult AppDetail(string id)
        {
            if (Session["kodename"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                Session["idApp"] = id;
                return View();
            }            
        }

        public ActionResult Report()
        {
            if (Session["kodename"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                string reportPath = ConfigurationManager.AppSettings["reportPath"].ToString();
                ViewBag.reportPath = reportPath;
                return View();
            }
        }

        public ActionResult ReportTimeSheet()
        {
            if (Session["kodename"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                //string reportPath = ConfigurationManager.AppSettings["reportPath"].ToString();
                //ViewBag.reportPath = reportPath;
                return View();
            }
        }

        //================================== GET DATA

        public JsonResult Logout()
        {
            try
            {
                Session.RemoveAll();
                return Json(new { Status = true, Message = "Berhasil" }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err Logout :" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult LoginLDAP(ClsLogin clsLogin)
        {
            try
            {
                bool abc = false;
                var ldap = new LdapAuthentication("LDAP://KPPMINING:389");

                if (ConfigurationManager.AppSettings["conditionServer"].ToString() == "false")
                {
                    abc = true;
                }

                else
                {
                    abc = ldap.IsAuthenticated("KPPMINING", clsLogin.Username, clsLogin.Password);
                    abc = true;
                }
                
                if (abc == true)
                {
                    var getName = dbSdlc.TBL_M_PICs.Where(x => x.NAME.ToUpper() == clsLogin.Username.ToUpper()).FirstOrDefault();
                    gc.unameGlobal = getName.PIC_DEV.ToUpper();

                    Session["kodename"] = getName.NAME.ToUpper();
                    Session["uname"] = getName.PIC_DEV.ToUpper();
                    FormsAuthentication.SetAuthCookie(clsLogin.Username, clsLogin.rememberMe);

                    return Json(new { Status = true, Message = "Berhasil" }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Gagal" }, JsonRequestBehavior.AllowGet);
                }
                
            }

            catch (Exception e)
            {
                return Json(new { Status = false, Message = "Err LoginLDAP :" + e.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public JsonResult GetAppList()
        {
            try
            {
                var get = dbSdlc.cufn_getAllApps();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err GetReviewActivity : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getPlatform()
        {
            try
            {
                var get = dbSdlc.TBL_M_PLATFORMs.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch ( Exception ex )
            {
                return Json(new { Status = false, Message = "Err getPlatform : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getServer()
        {
            try
            {
                var get = dbSdlc.TBL_M_SERVERs.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getServer : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getStatus()
        {
            try
            {
                var get = dbSdlc.TBL_M_STATUS.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getStatus : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getDetailApp()
        {
            try
            {
                string id = Session["idApp"].ToString();
                var get = dbSdlc.TBL_T_APPs.Where(x => x.APP_ID.ToString() == id.ToString().ToUpper()).FirstOrDefault();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getDetailApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDataDOCLIST(string id)
        {
            try
            {
                var get = dbSdlc.TBL_T_DOCs.Where(x => x.DOC_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err GetDataDOCLIST : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDataVERSIONLIST(string id)
        {
            try
            {
                var get = dbSdlc.TBL_T_VERSIONs.Where(x => x.VERSION_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err GetDataVERSIONLIST : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDataDBLIST(string id)
        {
            try
            {
                var get = dbSdlc.TBL_T_DBs.Where(x => x.DB_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err GetDataDBLIST : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        //================================== GET DATA

        public JsonResult InsertNewApp(ClsAppList AppList)
        {
            try
            {
                Guid newId = Guid.NewGuid();

                //insert app
                TBL_T_APP nd = new TBL_T_APP();                
                nd.APP_ID = newId;
                nd.APP_NAME = AppList.APP_NAME;
                nd.OWNER = AppList.OWNER;
                nd.PLATFORM_ID = AppList.PLATFORM.ToUpper();
                nd.SERVER_ID = AppList.SERVER.ToUpper();
                nd.STATUS_ID = AppList.STATUS.ToUpper();
                dbSdlc.TBL_T_APPs.InsertOnSubmit(nd);

                //insert version
                TBL_T_VERSION nv = new TBL_T_VERSION();
                nv.VERSION_ID = Guid.NewGuid();
                nv.DATE = DateTime.Now.Date;
                nv.VERSION = AppList.VERSION;
                nv.APP_ID = newId.ToString().ToUpper();
                dbSdlc.TBL_T_VERSIONs.InsertOnSubmit(nv);

                dbSdlc.SubmitChanges();

                return Json(new { Status = true, Message = "Berhasil Simpan ..." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteAppList(string id)
        {
            try
            {
                string kodeName = Session["kodename"].ToString().ToUpper();
                if (kodeName == "DEV004")
                {
                    var delete = dbSdlc.TBL_T_APPs.Where(x => x.APP_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                    dbSdlc.TBL_T_APPs.DeleteOnSubmit(delete);

                    var delVersion = dbSdlc.TBL_T_VERSIONs.Where(x => x.APP_ID.ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                    dbSdlc.TBL_T_VERSIONs.DeleteOnSubmit(delVersion);

                    dbSdlc.SubmitChanges();

                    return Json(new { Status = true, Message = "Berhasil Hapus ..." }, JsonRequestBehavior.AllowGet);
                }

                else if (kodeName == "DEV007")
                {
                    var delete = dbSdlc.TBL_T_APPs.Where(x => x.APP_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                    dbSdlc.TBL_T_APPs.DeleteOnSubmit(delete);

                    var delVersion = dbSdlc.TBL_T_VERSIONs.Where(x => x.APP_ID.ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                    dbSdlc.TBL_T_VERSIONs.DeleteOnSubmit(delVersion);

                    dbSdlc.SubmitChanges();

                    return Json(new { Status = true, Message = "Berhasil Hapus ..." }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Akses login tidak dapat hapus data ..." }, JsonRequestBehavior.AllowGet);
                }

                
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getListDocument()
        {
            try
            {
                string id = Session["idApp"].ToString();
                var get = dbSdlc.cufn_getAllDocs().Where(x => x.APP_ID.ToUpper() == id.ToUpper()).ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getListVersion()
        {
            try
            {
                string id = Session["idApp"].ToString();
                var get = dbSdlc.cufn_getAllVersions().Where(x => x.APP_ID.ToUpper() == id.ToUpper()).ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getListDatabase()
        {
            try
            {
                string id = Session["idApp"].ToString();
                var get = dbSdlc.cufn_getAllDatabases().Where(x => x.APP_ID.ToUpper() == id.ToUpper()).ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewApp : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getListDocType()
        {
            try
            {
                string id = Session["idApp"].ToString();
                var get = dbSdlc.TBL_M_DOC_TYPEs.ToList();
                return Json(new { data = get, Status = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err getListDocType : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult InsertNewDocList(ClsDetailApp dtlApp)
        {
            try
            {
                TBL_T_DOC nd = new TBL_T_DOC();
                nd.DOC_ID = Guid.NewGuid();
                nd.VERSION = dtlApp.VERSION_DOCLIST;
                nd.RELEASE_DATE = dtlApp.RELEASEDATE_DOCLIST;
                nd.APP_ID = Session["idApp"].ToString().ToUpper();
                nd.DOC_TYPE_ID = dtlApp.DOCTYPE_DOCLIST;

                dbSdlc.TBL_T_DOCs.InsertOnSubmit(nd);
                dbSdlc.SubmitChanges();

                return Json(new { Status = true, Message = "Berhasil Simpan Data ..." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewDocList : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult InsertNewVersionList(ClsDetailApp dtlApp)
        {
            try
            {
                TBL_T_VERSION nd = new TBL_T_VERSION();
                nd.VERSION_ID = Guid.NewGuid();
                nd.DATE = dtlApp.DATE_VERSIONLIST;
                nd.VERSION = dtlApp.VERSION_VERSIONLIST;
                nd.APP_ID = Session["idApp"].ToString().ToUpper();

                dbSdlc.TBL_T_VERSIONs.InsertOnSubmit(nd);
                dbSdlc.SubmitChanges();

                return Json(new { Status = true, Message = "Berhasil Simpan Data ..." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewVersionList : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult InsertNewDatabaseList(ClsDetailApp dtlApp)
        {
            try
            {
                TBL_T_DB nd = new TBL_T_DB();
                nd.DB_ID = Guid.NewGuid();
                nd.NAME = dtlApp.NAME_DBLIST;
                nd.APP_ID = Session["idApp"].ToString().ToUpper();

                dbSdlc.TBL_T_DBs.InsertOnSubmit(nd);
                dbSdlc.SubmitChanges();

                return Json(new { Status = true, Message = "Berhasil Simpan Data ..." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err InsertNewDatabaseList : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteDocList(string id)
        {
            try
            {
                string kodeName = Session["kodename"].ToString().ToUpper();
                if (kodeName == "DEV004")
                {
                    var delete = dbSdlc.TBL_T_DOCs.Where(x => x.DOC_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                    dbSdlc.TBL_T_DOCs.DeleteOnSubmit(delete);
                    dbSdlc.SubmitChanges();

                    return Json(new { Status = true, Message = "Berhasil Hapus Data ..." }, JsonRequestBehavior.AllowGet);
                }

                else if (kodeName == "DEV007")
                {
                    var delete = dbSdlc.TBL_T_DOCs.Where(x => x.DOC_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                    dbSdlc.TBL_T_DOCs.DeleteOnSubmit(delete);
                    dbSdlc.SubmitChanges();

                    return Json(new { Status = true, Message = "Berhasil Hapus Data ..." }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Akses login tidak dapat hapus data ..." }, JsonRequestBehavior.AllowGet);
                }

                
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err DeleteDocList : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteVersionList(string id)
        {
            try
            {
                string kodeName = Session["kodename"].ToString().ToUpper();
                if (kodeName == "DEV004")
                {
                    var delete = dbSdlc.TBL_T_VERSIONs.Where(x => x.VERSION_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                    dbSdlc.TBL_T_VERSIONs.DeleteOnSubmit(delete);
                    dbSdlc.SubmitChanges();

                    return Json(new { Status = true, Message = "Berhasil Hapus Data ..." }, JsonRequestBehavior.AllowGet);
                }

                else if (kodeName == "DEV007")
                {
                    var delete = dbSdlc.TBL_T_VERSIONs.Where(x => x.VERSION_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                    dbSdlc.TBL_T_VERSIONs.DeleteOnSubmit(delete);
                    dbSdlc.SubmitChanges();

                    return Json(new { Status = true, Message = "Berhasil Hapus Data ..." }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Akses login tidak dapat hapus data ..." }, JsonRequestBehavior.AllowGet);
                }

                
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err DeleteVersionList : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteDatabaseList(string id)
        {
            try
            {
                string kodeName = Session["kodename"].ToString().ToUpper();
                if (kodeName == "DEV004")
                {
                    var delete = dbSdlc.TBL_T_DBs.Where(x => x.DB_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                    dbSdlc.TBL_T_DBs.DeleteOnSubmit(delete);
                    dbSdlc.SubmitChanges();

                    return Json(new { Status = true, Message = "Berhasil Hapus Data ..." }, JsonRequestBehavior.AllowGet);
                }

                else if (kodeName == "DEV007")
                {
                    var delete = dbSdlc.TBL_T_DBs.Where(x => x.DB_ID.ToString().ToUpper() == id.ToString().ToUpper()).FirstOrDefault();
                    dbSdlc.TBL_T_DBs.DeleteOnSubmit(delete);
                    dbSdlc.SubmitChanges();

                    return Json(new { Status = true, Message = "Berhasil Hapus Data ..." }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { Status = false, Message = "Akses login tidak dapat hapus data ..." }, JsonRequestBehavior.AllowGet);
                }

                
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err DeleteDatabaseList : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateDocList(ClsDetailApp dtlApp)
        {
            try
            {
                var cek = dbSdlc.TBL_T_DOCs.Where(x => x.DOC_ID.ToString().ToUpper() == dtlApp.DOC_ID.ToUpper()).FirstOrDefault();
                cek.VERSION = dtlApp.VERSION_DOCLIST;
                cek.RELEASE_DATE = dtlApp.RELEASEDATE_DOCLIST;
                cek.DOC_TYPE_ID = dtlApp.DOCTYPE_DOCLIST;

                dbSdlc.SubmitChanges();

                return Json(new { Status = true, Message = "Berhasil Update Data ..." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err UpdateDocList : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateVersionList(ClsDetailApp dtlApp)
        {
            try
            {
                var cek = dbSdlc.TBL_T_VERSIONs.Where(x => x.VERSION_ID.ToString().ToUpper() == dtlApp.VERSION_ID.ToUpper()).FirstOrDefault();
                cek.VERSION = dtlApp.VERSION_VERSIONLIST;
                cek.DATE = dtlApp.DATE_VERSIONLIST;

                dbSdlc.SubmitChanges();

                return Json(new { Status = true, Message = "Berhasil Update Data ..." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err UpdateVersionList : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateDatabaseList(ClsDetailApp dtlApp)
        {
            try
            {                
                var cek = dbSdlc.TBL_T_DBs.Where(x => x.DB_ID.ToString().ToUpper() == dtlApp.DB_ID.ToUpper()).FirstOrDefault();
                cek.NAME = dtlApp.NAME_DBLIST;

                dbSdlc.SubmitChanges();

                return Json(new { Status = true, Message = "Berhasil Update Data ..." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err UpdateDatabaseList : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateAppDetail(ClsAppList updApp)
        {
            try
            {
                string id = Session["idApp"].ToString();
                var get = dbSdlc.TBL_T_APPs.Where(x => x.APP_ID.ToString().ToUpper() == id.ToUpper()).FirstOrDefault();
                get.APP_NAME = updApp.APP_NAME;
                get.OWNER = updApp.OWNER;
                get.PLATFORM_ID = updApp.PLATFORM;
                get.SERVER_ID = updApp.SERVER;
                get.STATUS_ID = updApp.STATUS;

                dbSdlc.SubmitChanges();

                return Json(new { Status = true, Message = "Berhasil Update Data ..." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Err UpdateAppDetail : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }

    public class ClsLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool rememberMe { get; set; }
    }

    public class ClsAppList
    {
        public string APP_ID { get; set; }
        public string APP_NAME { get; set; }
        public string OWNER { get; set; }
        public string PLATFORM { get; set; }
        public string SERVER { get; set; }
        public string STATUS { get; set; }
        public string VERSION { get; set; }
    }

    public class ClsDetailApp
    {
        //DOC LIST
        public string DOC_ID { get; set; }
        public string VERSION_DOCLIST { get; set; }
        public DateTime RELEASEDATE_DOCLIST { get; set; }
        public string APPID_DOCLIST { get; set; }
        public string DOCTYPE_DOCLIST { get; set; }

        //VERSION
        public string VERSION_ID { get; set; }
        public DateTime DATE_VERSIONLIST { get; set; }
        public string VERSION_VERSIONLIST { get; set; }
        public string APPID_VERSIONLIST { get; set; }

        //DATABASELIST
        public string DB_ID { get; set; }
        public string NAME_DBLIST { get; set; }
        public string APPID_DBLIST { get; set; }

    }

}